using System.Text.Json;
using System.Text;

namespace BookCatalogue.Web.Services;

internal class BookService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<BookService> _logger;

    public BookService(HttpClient httpClient, ILogger<BookService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    internal async Task<IEnumerable<Book>?> GetBooksAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<Book>>("/api/v1/books");

        return response;
    }

    internal async Task<bool> AddBookAsync(Book book)
    {
        var payload = JsonSerializer.Serialize(book);

        var response = await _httpClient.PostAsync($"/api/v1/books",
            new StringContent(payload, Encoding.UTF8, "application/json"));

        return response.IsSuccessStatusCode;
    }

    internal async Task<Book?> GetBookByIdAsync(string id)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<Book>($"/api/v1/books/{id}");

            return response;
        }
        catch (Exception e)
        {
            _logger.LogInformation(e, "An error occurred while fetching book {id}", id);
            return null;
        }
    }

    internal async Task<bool> DeleteBookByIdAsync(string id)
    {
        var response = await _httpClient.DeleteAsync($"/api/v1/books/{id}");

        return response.IsSuccessStatusCode;
    }
}
