using BookCatalogue.API.Dtos.Author;

namespace BookCatalogue.API.Dtos.Book;

/// <summary>
/// Create book payload.
/// </summary>
public class CreateBookDto
{
    /// <example>Harry Potter and the Goblet of Fire.</example>s
    public string Title { get; set; }

    /// <example>2000-07-08</example>
    public DateOnly YearPublished { get; set; }

    public List<string> AuthorIds { get; set; }

    public List<string>? SubjectIds { get; set; }
}
