namespace BookCatalogue.API.Dtos.Book;

/// <summary>
/// Update book payload.
/// </summary>
public class UpdateBookDto
{
    /// <example>Harry Potter and the Goblet of Fire</example>s
    public string Title { get; set; }

    /// <example>2000-07-08</example>
    public DateOnly YearPublished { get; set; }

    /// <example>
    /// [
    ///     "2df89f55-163d-4f98-8513-987d0226975c", 
    ///     "490b8eb6-7c05-4ae3-82da-979187a2ea89"
    /// ]
    /// </example>
    public List<string>? AuthorIds { get; set; }

    /// <example>
    /// [
    ///     "2df89f55-163d-4f98-8513-987d0226975c", 
    ///     "490b8eb6-7c05-4ae3-82da-979187a2ea89"
    /// ]
    /// </example>
    public List<string>? SubjectIds { get; set; }
}
