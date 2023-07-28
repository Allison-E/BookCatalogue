using System.Text.Json;
using System.Text;

namespace BookCatalogue.Web.Services;

internal class SubjectService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SubjectService> _logger;

    public SubjectService(HttpClient httpClient, ILogger<SubjectService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    internal async Task<IEnumerable<Subject>?> GetSubjectsAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<Subject>>("/api/v1/subjects");

        return response;
    }

    internal async Task<bool> AddSubjectAsync(Subject subject)
    {
        var payload = JsonSerializer.Serialize(subject);

        var response = await _httpClient.PostAsync($"/api/v1/subjects",
            new StringContent(payload, Encoding.UTF8, "application/json"));

        return response.IsSuccessStatusCode;
    }

    internal async Task<Subject?> GetSubjectByIdAsync(string id)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<Subject>($"/api/v1/subjects/{id}");

            return response;
        }
        catch (Exception e)
        {
            _logger.LogInformation(e, "An error occurred while fetching subject {id}", id);
            return null;
        }
    }

    internal async Task<IEnumerable<SubjectBook>?> GetSubjectBooksByIdAsync(string id)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<List<SubjectBook>>($"/api/v1/subjects/{id}/books");

            return response;
        }
        catch (Exception e)
        {
            _logger.LogInformation(e, "An error occurred while fetching subject {id}", id);
            return null;
        }
    }

    internal async Task<bool> DeleteSubjectByIdAsync(string id)
    {
        var response = await _httpClient.DeleteAsync($"/api/v1/subjects/{id}");

        return response.IsSuccessStatusCode;
    }

    internal async Task<bool> EditSubjectAsync(Subject subject)
    {
        var payload = JsonSerializer.Serialize(subject);

        var response = await _httpClient.PutAsync($"/api/v1/subjects/{subject.Id}",
             new StringContent(payload, Encoding.UTF8, "application/json"));

        return response.IsSuccessStatusCode;
    }
}
