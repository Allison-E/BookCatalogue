namespace BookCatalogue.Web.Data;

/// <summary>
/// A subject, field of study or genre.
/// </summary>
public class Subject
{
    /// <summary>
    /// ID.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Title of the subject matter.
    /// </summary>
    [Required]
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
