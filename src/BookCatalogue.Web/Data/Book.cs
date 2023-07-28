namespace BookCatalogue.Web.Data;

/// <summary>
/// A written and published book.
/// </summary>
public class Book
{
    /// <summary>
    /// ID.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Book title.
    /// </summary>
    [Required]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Year of publication.
    /// </summary>
    [Required]
    public DateOnly YearPublished { get; set; }

    /// <summary>
    /// Book's author(s).
    /// </summary>
    public IList<Author> Authors { get; set; } = new List<Author>();

    /// <summary>
    /// Subject matter(s) discussed in the book.
    /// </summary>
    /// <remarks>
    /// This may also refer to the genre of the book.
    /// </remarks>
    public IList<Subject>? Subjects { get; set; }
}
