namespace BookCatalogue.API.Dtos.Author;

/// <summary>
/// Read author payload.s
/// </summary>
public class ReadAuthorDto
{
    /// <example>2df89f55-163d-4f98-8513-987d0226975c</example>
    public Guid Id { get; set; }

    /// <example>Adam</example>s
    public string Firstname { get; set; }

    /// <example>Grant</example>
    public string Lastname { get; set; }
}
