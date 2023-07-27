namespace BookCatalogue.API.Dtos.Subject;

/// <summary>
/// Read subject response.
/// </summary>
public class ReadSubjectDto
{
    /// <example>2df89f55-163d-4f98-8513-987d0226975c</example>
    public Guid Id { get; set; }

    /// <example>Finance</example>s
    public string Title { get; set; }

    /// <example>All about money.</example>
    public string Description { get; set; }

}
