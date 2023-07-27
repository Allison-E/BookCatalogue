namespace BookCatalogue.Core.Models;

/// <summary>
/// Writer of or contributor to a book.
/// </summary>
public class Author: BaseEntity
{
    /// <summary>
    /// Author's first name.
    /// </summary>
    public string Firstname { get; set; } = string.Empty;

    /// <summary>
    /// Author's last name.
    /// </summary>
    public string Lastname { get; set; } = string.Empty;

    /// <summary>
    /// Author's full name.
    /// </summary>
    public string Name => $"{Firstname} {Lastname}";

    /// <summary>
    /// Books written or contributed to by the author.
    /// </summary>
    public IList<Book> Books { get; set; } = new List<Book>();
}
