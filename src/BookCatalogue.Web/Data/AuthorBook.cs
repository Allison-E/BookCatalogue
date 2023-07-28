namespace BookCatalogue.Web.Data;

public class AuthorBook
{
    /// <example>2df89f55-163d-4f98-8513-987d0226975c</example>
    public Guid Id { get; set; }

    /// <example>
    /// Harry Potter and the Goblet of Fire
    /// </example>
    public string Title { get; set; }

    /// <example>
    /// 8/07/2000
    /// </example>
    public DateOnly YearPublished { get; set; }
}
