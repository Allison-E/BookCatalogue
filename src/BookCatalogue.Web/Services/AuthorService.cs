using System.Text;
using System.Text.Json;

namespace BookCatalogue.Web.Services;

internal class AuthorService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AuthorService> _logger;

    public AuthorService(HttpClient httpClient, ILogger<AuthorService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    internal async Task<IEnumerable<Author>?> GetAuthorsAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<Author>>("/api/v1/authors");

        return response;
    }

    internal async Task<bool> AddAuthorAsync(Author author)
    {
        var payload = JsonSerializer.Serialize(author);

        var response = await _httpClient.PostAsync($"/api/v1/authors",
            new StringContent(payload, Encoding.UTF8, "application/json"));

        return response.IsSuccessStatusCode;
    }

    internal async Task<Author?> GetAuthorByIdAsync(string id)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<Author>($"/api/v1/authors/{id}");

            return response;
        }
        catch (Exception e)
        {
            _logger.LogInformation(e, "An error occurred while fetching author {id}", id);
            return null;
        }
    }
    
    internal async Task<IEnumerable<AuthorBook>?> GetAuthorBooksByIdAsync(string id)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<List<AuthorBook>>($"/api/v1/authors/{id}/books");

            return response;
        }
        catch (Exception e)
        {
            _logger.LogInformation(e, "An error occurred while fetching author {id}", id);
            return null;
        }
    }

    internal async Task<bool> DeleteAuthorByIdAsync(string id)
    {
        var response = await _httpClient.DeleteAsync($"/api/v1/authors/{id}");

        return response.IsSuccessStatusCode;
    }

    internal async Task<bool> EditAuthorAsync(Author author)
    {
        var payload = JsonSerializer.Serialize(author);

        var response = await _httpClient.PutAsync($"/api/v1/authors/{author.Id}",
             new StringContent(payload, Encoding.UTF8, "application/json"));

        return response.IsSuccessStatusCode;
    }
}
