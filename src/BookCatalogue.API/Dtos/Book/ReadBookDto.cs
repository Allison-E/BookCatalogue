using BookCatalogue.API.Dtos.Author;
using BookCatalogue.API.Dtos.Subject;

namespace BookCatalogue.API.Dtos.Book;

/// <summary>
/// Read book payload.
/// </summary>
public class ReadBookDto
{
    /// <example>2df89f55-163d-4f98-8513-987d0226975c</example>
    public Guid Id { get; set; }

    /// <example>Harry Potter and the Goblet of Fire</example>s
    public string Title { get; set; }

    /// <example>2000-07-08</example>
    public DateOnly YearPublished { get; set; }

    public List<ReadAuthorDto> Authors { get; set; }

    public List<ReadSubjectPreviewDto> Subjects { get; set; }
}
