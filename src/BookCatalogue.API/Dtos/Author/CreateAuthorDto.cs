namespace BookCatalogue.API.Dtos.Author;

/// <summary>
/// Create author payload.
/// </summary>
public class CreateAuthorDto
{
    /// <example>Adam</example>s
    public string Firstname { get; set; }

    /// <example>Grant</example>
    public string Lastname { get; set; }
}
