namespace BookCatalogue.API.Dtos.Subject;

/// <summary>
/// Read subject response.
/// </summary>
public class ReadSubjectPreviewDto
{
    /// <example>2df89f55-163d-4f98-8513-987d0226975c</example>
    public Guid Id { get; set; }

    /// <example>Finance</example>s
    public string Title { get; set; }

}
