namespace BookCatalogue.API.Exceptions;

/// <summary>
/// A generic exception thrown when an error occurs
/// within the application.
/// </summary>
public class ApiException: BookCatalogueApiException
{
    /// <summary>
    /// Creates an instance of the <see cref="ApiException"/>.
    /// </summary>
    public ApiException(string message) 
        : base(StatusCodes.Status500InternalServerError, message)
    {
    }
}
