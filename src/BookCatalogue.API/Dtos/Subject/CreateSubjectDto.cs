namespace BookCatalogue.API.Dtos.Subject;

/// <summary>
/// Create subject payload.
/// </summary>
public class CreateSubjectDto
{
    /// <example>Finance</example>s
    public string Title { get; set; }

    /// <example>All about money</example>
    public string Description { get; set; }
}
