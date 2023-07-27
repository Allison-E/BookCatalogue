namespace BookCatalogue.API.Dtos.Wrappers;

/// <summary>
/// Response sent if an error occurred.
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Status code.
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// Error message.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// List of errors that occurred during the operation.
    /// </summary>
    /// <remarks>Used mainly when there is a validation error.</remarks>
    public List<string>? Errors { get; set; } = null;
}