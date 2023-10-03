using Microsoft.Net.Http.Headers;

namespace WebApplication1;

public class MyHttpClientService
{
    private readonly HttpClient _httpClient;
 
    public MyHttpClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
 
        _httpClient.BaseAddress = new Uri("http://localhost:5031/");
 
        // using Microsoft.Net.Http.Headers;
        // The GitHub API requires two headers.
        _httpClient.DefaultRequestHeaders.Add(
            HeaderNames.Accept, "application/json");
        _httpClient.DefaultRequestHeaders.Add(
            HeaderNames.UserAgent, "HttpRequestsSample");
    }
 
    public async Task<string> GetUsersAsync() =>
        await _httpClient.GetStringAsync("operation/users");
}