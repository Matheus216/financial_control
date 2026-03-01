using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using FinancialControl.API.Application.Interfaces;
using FinancialControl.API.Configuration.HttpIntegration;
using FinancialControl.API.Contracts.User.Responses;
using FinancialControl.API.Data.Entities;

namespace FinancialControl.API.Application.Providers.Keycloak;

public class KeycloakService : IKeycloakService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly KeycloakConfiguration _config;
    private readonly ILogger<KeycloakService> _logger;

    public KeycloakService(IHttpClientFactory httpClientFactory, KeycloakConfiguration config, ILogger<KeycloakService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _config = config;
        _logger = logger;
    }

    private async Task<string> GetAdminTokenAsync() 
    {
        var client = _httpClientFactory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Post, $"{_config.BaseUrl}/realms/master/protocol/openid-connect/token");
        
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("client_id", "admin-cli"),
            new KeyValuePair<string, string>("username", _config.AdminUsername),
            new KeyValuePair<string, string>("password", _config.AdminPassword)
        });

        request.Content = content;
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        return doc.RootElement.GetProperty("access_token").GetString() ?? throw new Exception("Failed to get access token");
    }

    public async Task<Guid> CreateUserAsync(PersonCreateRequest request)
    {
        var client = _httpClientFactory.CreateClient();
        var token = await GetAdminTokenAsync();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var userRepresentation = new
        {
            username = request.Username,
            email = request.Email,
            firstName = request.FirstName,
            lastName = request.LastName,
            enabled = true,
            credentials = new[]
            {
                new { type = "password", value = request.Password, temporary = false }
            }
        };

        var content = new StringContent(JsonSerializer.Serialize(userRepresentation), Encoding.UTF8, "application/json");
        var response = await client.PostAsync($"{_config.BaseUrl}/admin/realms/{_config.Realm}/users", content);
        
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            _logger.LogError("Error creating user in Keycloak: {Error}", error);
            throw new Exception($"Failed to create user: {response.ReasonPhrase}");
        }

        // Keycloak returns the user ID in the Location header
        var location = response.Headers.Location;
        return new Guid(location?.Segments.Last() ?? string.Empty);
    }

    public async Task<UserResponse?> GetUserByIdAsync(string id)
    {
        var client = _httpClientFactory.CreateClient();
        var token = await GetAdminTokenAsync();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetAsync($"{_config.BaseUrl}/admin/realms/{_config.Realm}/users/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;
        
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<UserResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task<IEnumerable<UserResponse>> ListUsersAsync()
    {
        var client = _httpClientFactory.CreateClient();
        var token = await GetAdminTokenAsync();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetAsync($"{_config.BaseUrl}/admin/realms/{_config.Realm}/users");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<UserResponse>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? [];
    }

    public async Task UpdateUserAsync(Guid id, PersonUpdateRequest request)
    {
        var client = _httpClientFactory.CreateClient();
        var token = await GetAdminTokenAsync();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var userRepresentation = new
        {
            email = request.Email,
            firstName = request.FirstName,
            lastName = request.LastName,
            enabled = request.Enabled
        };

        var content = new StringContent(JsonSerializer.Serialize(userRepresentation), Encoding.UTF8, "application/json");
        var response = await client.PutAsync($"{_config.BaseUrl}/admin/realms/{_config.Realm}/users/{id}", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteUserAsync(string id)
    {
        var client = _httpClientFactory.CreateClient();
        var token = await GetAdminTokenAsync();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.DeleteAsync($"{_config.BaseUrl}/admin/realms/{_config.Realm}/users/{id}");
        response.EnsureSuccessStatusCode();
    }
}
