namespace BookCatalogue.API.Dtos.Subject;

/// <summary>
/// Update book payload.
/// </summary>
public class UpdateSubjectDto
{
    /// <example>Finance</example>s
    public string Title { get; set; }

    /// <example>All about money</example>
    public string Description { get; set; }
}
