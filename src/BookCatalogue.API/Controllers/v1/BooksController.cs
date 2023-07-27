using System.Runtime.CompilerServices;
using BookCatalogue.API.Dtos.Book;
using BookCatalogue.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;

namespace BookCatalogue.API.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class BooksController: ControllerBase
{
    private readonly ILogger<BooksController> _logger;
    private readonly BookCatalogueContext _dbContext;
    private readonly IMapper _mapper;

    public BooksController(
        ILogger<BooksController> logger,
        BookCatalogueContext dbContext,
        IMapper mapper)
    {
        _logger = logger;
        _dbContext = dbContext;
        _mapper = mapper;
    }

    /// <summary>
    /// Add book
    /// </summary>
    /// <param name="bookDto">Book's information</param>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        CreateBookDto bookDto,
        CancellationToken cancellationToken = default)
    {
        var (Subjects, Authors) = await GetSubjectsAndAuthorsAsync(bookDto.SubjectIds, bookDto.AuthorIds, cancellationToken);

        var book = _mapper.Map<Book>(bookDto);
        book.Subjects = Subjects;
        book.Authors = Authors;

        await _dbContext.Books.AddAsync(book, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Book {id} created", book.Id);

        return CreatedAtRoute("GetBookById", new { Id = book.Id.ToString(), CancellationToken = CancellationToken.None });
    }

    /// <summary>
    /// Get books
    /// </summary>
    /// <response code="200">Query successful</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<ReadBookDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching Books");

        var books = await _dbContext.Books
            .Include(b => b.Authors)
            .Include(b => b.Subjects)
            .ToListAsync(cancellationToken);
        var readBooksDto = _mapper.Map<List<ReadBookDto>>(books);

        return Ok(readBooksDto);
    }

    /// <summary>
    /// Get book by ID
    /// </summary>
    /// <param name="id">Book ID</param>
    [HttpGet("{id}", Name = "GetBookById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        string id,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching Book {id}", id);

        Book book = await GetBookIfExistsAsync(id, cancellationToken);
        var readBookDto = _mapper.Map<ReadBookDto>(book);

        return Ok(readBookDto);
    }

    /// <summary>
    /// Update book
    /// </summary>
    /// <param name="id">Book id</param>
    /// <param name="bookUpdate">New book information</param>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        string id,
        [FromBody] UpdateBookDto bookUpdate,
        CancellationToken cancellationToken = default)
    {
        var fetchNavTask = GetSubjectsAndAuthorsAsync(bookUpdate.SubjectIds, bookUpdate.AuthorIds, cancellationToken);
        Book book = await GetBookIfExistsAsync(id, cancellationToken);
        _mapper.Map(bookUpdate, book);

        var (Subjects, Authors) = await fetchNavTask;
        book.Subjects = Subjects;
        book.Authors = Authors;

        _dbContext.Update(book);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Book {id} updated. New values: {book}",
            book.Id,
            book);

        var readBookDto = _mapper.Map<ReadBookDto>(book);

        return Ok(readBookDto);
    }

    /// <summary>
    /// Delete book
    /// </summary>
    /// <param name="id">Book ID</param>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        string id,
        CancellationToken cancellationToken = default)
    {
        Book book = await GetBookIfExistsAsync(id, cancellationToken);

        _dbContext.Books
            .Remove(book);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Book {id} deleted", id);

        return NoContent();
    }

    /// <summary>
    /// Ensure authors and subjects exist, and return them.
    /// </summary>
    /// <param name="subjectIds">Subject IDs.</param>
    /// <param name="authorIds">Author IDs.</param>
    /// <returns>A tuple containing the subjects and authors.</returns>
    /// <exception cref="ValidationException">
    /// Thrown if an author or subject doesn't exist.
    /// </exception>
    private async Task<(List<Subject> Subjects, List<Author> Authors)> GetSubjectsAndAuthorsAsync(
        List<string>? subjectIds,
        List<string>? authorIds,
        CancellationToken cancellationToken = default)
    {
        // Todo: Ensure the books have at least one author.
        var fetchSubjectsTask = new List<Task<Subject>>();
        var fetchAuthorsTask = new List<Task<Author>>();

        if (subjectIds is not null)
        {
            foreach (var id in subjectIds)
            {
                fetchSubjectsTask.Add(GetSubjectIfExistsAsync(id, cancellationToken));
            }
        }

        if (authorIds is not null)
        {
            foreach (var id in authorIds)
            {
                fetchAuthorsTask.Add(GetAuthorIfExistsAsync(id, cancellationToken));
            }
        }

        // Ensure all subjects and authors referenced exist.
        var subjectsTask = Task.WhenAll(fetchSubjectsTask);
        var authorsTask = Task.WhenAll(fetchAuthorsTask);

        if (subjectsTask.IsFaulted || authorsTask.IsFaulted)
        {
            List<Exception> innerExceptions = new();

            if (subjectsTask.Exception is not null)
                innerExceptions.AddRange(subjectsTask.Exception.InnerExceptions);

            if (authorsTask.Exception is not null)
                innerExceptions.AddRange(authorsTask.Exception.InnerExceptions);

            List<string> innerExceptionMessages = innerExceptions
                .Select(e => e.Message)
                .ToList();

            throw new ValidationException("Bad request.", innerExceptionMessages);
        }

        var subjects = await subjectsTask;
        var authors = await authorsTask;

        return (subjects.ToList(), authors.ToList());
    }

    private async Task<Book> GetBookIfExistsAsync(string id, CancellationToken cancellationToken = default)
    {
        bool parsedSuccessfully = Guid.TryParse(id, out Guid guid);

        if (!parsedSuccessfully)
            throw new NotFoundException("Book not found.");

        Book book = await _dbContext.Books
                .Include(b => b.Authors)
                .Include(b => b.Subjects)
                .Where(b => b.Id == guid)
                .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException("Book not found.");

        return book;
    }

    private async Task<Subject> GetSubjectIfExistsAsync(string id, CancellationToken cancellationToken = default)
    {
        bool parsedSuccessfully = Guid.TryParse(id, out Guid guid);

        if (!parsedSuccessfully)
            throw new NotFoundException($"Subject '{id}' not found.");

        Subject subject = await _dbContext.Subjects.FindAsync(guid, cancellationToken)
            ?? throw new NotFoundException($"Subject '{id}' not found.");

        return subject;
    }

    private async Task<Author> GetAuthorIfExistsAsync(string id, CancellationToken cancellationToken = default)
    {
        bool parsedSuccessfully = Guid.TryParse(id, out Guid guid);

        if (!parsedSuccessfully)
            throw new NotFoundException($"Author '{id}' not found.");

        Author author = await _dbContext.Authors.FindAsync(guid, cancellationToken)
            ?? throw new NotFoundException($"Author '{id}' not found.");

        return author;
    }
}
