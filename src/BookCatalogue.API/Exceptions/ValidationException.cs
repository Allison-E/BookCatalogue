namespace BookCatalogue.API.Exceptions;

/// <summary>
/// A generic exception thrown when an error occurs
/// within the application.
/// </summary>
public class ValidationException: BookCatalogueApiException
{
    /// <summary>
    /// Creates an instance of the <see cref="ValidationException"/>.
    /// </summary>
    public ValidationException(string message, List<string> errors) 
        : base(StatusCodes.Status400BadRequest, message, errors)
    {
    }
}
