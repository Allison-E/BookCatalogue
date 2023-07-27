namespace BookCatalogue.API.Exceptions;

public abstract class BookCatalogueApiException: Exception
{
    public int StatusCode { get; set; }

    public List<string>? Errors { get; set; }

    public BookCatalogueApiException(int statusCode, string message)
        : base(message)
    {
        StatusCode = statusCode;
    }

    public BookCatalogueApiException(int statusCode, string message, List<string> errors) : this(statusCode, message)
    {
        Errors = errors;
    }
}
