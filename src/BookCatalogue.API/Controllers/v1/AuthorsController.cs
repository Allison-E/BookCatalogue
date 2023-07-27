using BookCatalogue.API.Dtos.Author;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookCatalogue.API.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthorsController: ControllerBase
{
    private readonly ILogger<AuthorsController> _logger;
    private readonly BookCatalogueContext _dbContext;
    private readonly IMapper _mapper;

    public AuthorsController(
        ILogger<AuthorsController> logger,
        BookCatalogueContext dbContext,
        IMapper mapper)
    {
        _logger = logger;
        _dbContext = dbContext;
        _mapper = mapper;
    }

    /// <summary>
    /// Add author
    /// </summary>
    /// <param name="authorDto">Author's information</param>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        CreateAuthorDto authorDto,
        CancellationToken cancellationToken = default)
    {
        var author = _mapper.Map<Author>(authorDto);

        await _dbContext.Authors
            .AddAsync(author, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Author {id} created", author.Id);

        return CreatedAtRoute("GetAuthorById", new { Id = author.Id.ToString() });
    }

    /// <summary>
    /// Get authors
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching Authors");

        var authors = await _dbContext.Authors.ToListAsync(cancellationToken);
        var readAuthorsDto = _mapper.Map<List<ReadAuthorDto>>(authors);

        return Ok(readAuthorsDto);
    }

    /// <summary>
    /// Get author by ID
    /// </summary>
    /// <param name="id">Author ID</param>
    [HttpGet("{id}", Name = "GetAuthorById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        string id,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching Author {id}", id);

        Author author = await GetAuthorIfExistsAsync(id, cancellationToken);
        var readAuthorDto = _mapper.Map<ReadAuthorDto>(author);

        return Ok(readAuthorDto);
    }

    /// <summary>
    /// Update author
    /// </summary>
    /// <param name="id">Author id</param>
    /// <param name="authorUpdate">New author information</param>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        string id,
        [FromBody] UpdateAuthorDto authorUpdate,
        CancellationToken cancellationToken = default)
    {
        Author author = await GetAuthorIfExistsAsync(id, cancellationToken);

        _mapper.Map(authorUpdate, author);
        _dbContext.Update(author);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Author {id} updated. New values: {author}",
            author.Id,
            author);

        var readAuthorDto = _mapper.Map<CreateAuthorDto>(author);
        return Ok(readAuthorDto);
    }

    /// <summary>
    /// Delete author
    /// </summary>
    /// <param name="id">Author ID</param>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        string id,
        CancellationToken cancellationToken = default)
    {
        Author author = await GetAuthorIfExistsAsync(id, cancellationToken);

        _dbContext.Authors
            .Remove(author);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Author {id} deleted", id);

        return NoContent();
    }

    [HttpGet("{id:guid}/books")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAuthorBooks(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var author = await _dbContext.Authors
            .Where(a => a.Id == id)
            .Include(a => a.Books)
            .FirstOrDefaultAsync(cancellationToken);

        if (author == default)
            throw new NotFoundException("Author not found.");

        var books = _mapper.Map<List<ReadAuthorBooksDto>>(author.Books);

        return Ok(books);
    }

    private async Task<Author> GetAuthorIfExistsAsync(string id, CancellationToken cancellationToken = default)
    {
        bool parsedSuccessfully = Guid.TryParse(id, out Guid guid);

        if (!parsedSuccessfully)
            throw new NotFoundException("Author not found.");

        Author author = await _dbContext.Authors.FindAsync(guid, cancellationToken)
            ?? throw new NotFoundException("Author not found.");

        return author;
    }
}
