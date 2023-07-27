namespace BookCatalogue.Core.Models;

/// <summary>
/// A subject, field of study or genre.
/// </summary>
public class Subject: BaseEntity
{
    /// <summary>
    /// Title of the subject matter.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Subject description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Books written on the subject.
    /// </summary>
    public IList<Book> Books { get; set; } = new List<Book>();
}
