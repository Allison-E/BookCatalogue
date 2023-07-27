using BookCatalogue.API.Dtos.Subject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookCatalogue.API.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class SubjectsController: ControllerBase
{
    private readonly ILogger<SubjectsController> _logger;
    private readonly BookCatalogueContext _dbContext;
    private readonly IMapper _mapper;

    public SubjectsController(
        ILogger<SubjectsController> logger,
        BookCatalogueContext dbContext,
        IMapper mapper)
    {
        _logger = logger;
        _dbContext = dbContext;
        _mapper = mapper;
    }

    /// <summary>
    /// Add subject
    /// </summary>
    /// <param name="subjectDto">Subject's information</param>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        CreateSubjectDto subjectDto,
        CancellationToken cancellationToken = default)
    {
        var subject = _mapper.Map<Subject>(subjectDto);

        await _dbContext.Subjects
            .AddAsync(subject, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Subject {id} created", subject.Id);

        return CreatedAtRoute("GetSubjectById", new { Id = subject.Id.ToString() });
    }

    /// <summary>
    /// Get subjects
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching Subjects");

        var subjects = await _dbContext.Subjects.ToListAsync(cancellationToken);
        var readSubjectsDto = _mapper.Map<List<ReadSubjectDto>>(subjects);

        return Ok(readSubjectsDto);
    }

    /// <summary>
    /// Get subject by ID
    /// </summary>
    /// <param name="id">Subject ID</param>
    [HttpGet("{id}", Name = "GetSubjectById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        string id,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching Subject {id}", id);

        Subject subject = await GetSubjectIfExistsAsync(id, cancellationToken);
        var readSubjectDto = _mapper.Map<ReadSubjectDto>(subject);

        return Ok(readSubjectDto);
    }

    /// <summary>
    /// Update subject
    /// </summary>
    /// <param name="id">Subject id</param>
    /// <param name="subjectUpdate">New subject information</param>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        string id,
        [FromBody] UpdateSubjectDto subjectUpdate,
        CancellationToken cancellationToken = default)
    {
        Subject subject = await GetSubjectIfExistsAsync(id, cancellationToken);

        _mapper.Map(subjectUpdate, subject);
        _dbContext.Update(subject);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Subject {id} updated. New values: {subject}",
            subject.Id,
            subject);

        var readSubjectDto = _mapper.Map<CreateSubjectDto>(subject);
        return Ok(readSubjectDto);
    }

    /// <summary>
    /// Delete subject
    /// </summary>
    /// <param name="id">Subject ID</param>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        string id,
        CancellationToken cancellationToken = default)
    {
        Subject subject = await GetSubjectIfExistsAsync(id, cancellationToken);

        _dbContext.Subjects
            .Remove(subject);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Subject {id} deleted", id);

        return NoContent();
    }

    [HttpGet("{id:guid}/books")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSubjectBooks(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var subject = await _dbContext.Subjects
            .Where(a => a.Id == id)
            .Include(a => a.Books)
            .FirstOrDefaultAsync(cancellationToken);

        if (subject == default)
            throw new NotFoundException("Subject not found.");

        var books = _mapper.Map<List<ReadSubjectBooksDto>>(subject.Books);

        return Ok(books);
    }

    private async Task<Subject> GetSubjectIfExistsAsync(string id, CancellationToken cancellationToken = default)
    {
        bool parsedSuccessfully = Guid.TryParse(id, out Guid guid);

        if (!parsedSuccessfully)
            throw new NotFoundException("Subject not found.");

        Subject subject = await _dbContext.Subjects.FindAsync(guid, cancellationToken)
            ?? throw new NotFoundException("Subject not found.");

        return subject;
    }
}
