namespace BookCatalogue.API.Dtos.Author;

/// <summary>
/// Update author payload.
/// </summary>
public class UpdateAuthorDto
{
    /// <example>Adam</example>s
    public string Firstname { get; set; }

    /// <example>Grant</example>
    public string Lastname { get; set; }
}
