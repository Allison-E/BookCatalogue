using System.Net;

namespace BookCatalogue.API.Exceptions;

/// <summary>
/// The exception that is thrown when a resource or object requested is not found.
/// </summary>
public class NotFoundException: BookCatalogueApiException
{
    /// <summary>
    /// Creates an instance of the <see cref="NotFoundException"/>.
    /// </summary>
    public NotFoundException(string message) : base(StatusCodes.Status404NotFound, message)
    {
    }

    /// <summary>
    /// Creates an instance of the <see cref="NotFoundException"/> with the default message,
    /// "Not found."
    /// </summary>
    public NotFoundException() : this("Not found.")
    {
    }
}