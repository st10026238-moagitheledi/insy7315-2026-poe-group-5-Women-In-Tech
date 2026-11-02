using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using KoosMatliApp.Models;
using Microsoft.Maui.Storage;

namespace KoosMatliApp.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private const string TokenKey = "auth_token";

    // Windows desktop: localhost.
    // Android emulator: use http://10.0.2.2:5000 instead.
    private static string BaseUrl => ApiEndpoint.BaseUrl;

    private static readonly JsonSerializerOptions JsonOptions =
        new(JsonSerializerDefaults.Web);

    public static AuthService Instance { get; } = new();

    private AuthService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(BaseUrl),
            Timeout = TimeSpan.FromSeconds(30)
        };
    }

    public async Task<AuthResponse> LoginAsync(
        string email, string password)
    {
        try
        {
            var request = new LoginRequest
            {
                Email = email.Trim(),
                Password = password
            };

            using var response = await _httpClient.PostAsJsonAsync(
                "/api/auth/login", request);

            var result = await ReadResponseAsync(response);

            if (result.Success)
            {
                if (string.IsNullOrWhiteSpace(result.Token))
                {
                    return CreateFailure(
                        "Login response did not contain an authentication token. Check the API login response.");
                }

                await SaveTokenAsync(result.Token);
            }

            return result;
        }
        catch (TaskCanceledException)
        {
            return CreateFailure("The request timed out. Please try again.");
        }
        catch (HttpRequestException ex)
        {
            return CreateFailure($"Unable to connect to the server: {ex.Message}");
        }
        catch (Exception ex)
        {
            return CreateFailure($"Login error: {ex.Message}");
        }
    }

    public async Task<AuthResponse> RegisterParentAsync(
        string displayName, string email, string password)
    {
        try
        {
            var request = new RegisterRequest
            {
                DisplayName = displayName.Trim(),
                Email = email.Trim(),
                Password = password
            };

            using var response = await _httpClient.PostAsJsonAsync(
                "/api/auth/register-parent", request);

            var result = await ReadResponseAsync(response);

            if (result.Success &&
                !string.IsNullOrWhiteSpace(result.Token))
            {
                await SaveTokenAsync(result.Token);
            }

            return result;
        }
        catch (TaskCanceledException)
        {
            return CreateFailure("The request timed out. Please try again.");
        }
        catch (HttpRequestException ex)
        {
            return CreateFailure($"Unable to connect to the server: {ex.Message}");
        }
        catch (Exception ex)
        {
            return CreateFailure($"Registration error: {ex.Message}");
        }
    }

    public async Task<AuthResponse> ForgotPasswordAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return CreateFailure("Please enter your email address.");

        try
        {
            using var response = await _httpClient.PostAsJsonAsync(
                "/api/auth/forgot-password",
                new { Email = email.Trim() });

            return await ReadResponseAsync(response);
        }
        catch (TaskCanceledException)
        {
            return CreateFailure("The request timed out. Please try again.");
        }
        catch (HttpRequestException)
        {
            return CreateFailure("Unable to connect to the server.");
        }
        catch (Exception)
        {
            return CreateFailure("An unexpected error occurred. Please try again.");
        }
    }

    public async Task<AuthResponse> GetCurrentUserAsync()
    {
        try
        {
            string? token = await GetSavedTokenAsync();

            if (string.IsNullOrWhiteSpace(token))
            {
                return CreateFailure("No saved login session was found. Please log in again.");
            }

            using var request = new HttpRequestMessage(
                HttpMethod.Get, "/api/auth/me");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            using var response = await _httpClient.SendAsync(request);
            var result = await ReadResponseAsync(response);

            if (response.StatusCode ==
                System.Net.HttpStatusCode.Unauthorized)
            {
                Logout();
            }

            return result;
        }
        catch (TaskCanceledException)
        {
            return CreateFailure("The request timed out. Please try again.");
        }
        catch (HttpRequestException)
        {
            return CreateFailure("Unable to connect to the server.");
        }
        catch (Exception ex)
        {
            return CreateFailure($"Unable to retrieve account details: {ex.Message}");
        }
    }

    public async Task<string?> GetSavedTokenAsync()
    {
        try
        {
            return await SecureStorage.Default.GetAsync(TokenKey);
        }
        catch (Exception)
        {
            return null;
        }
    }

    private async Task SaveTokenAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new InvalidOperationException(
                "The server returned an empty authentication token.");

        await SecureStorage.Default.SetAsync(TokenKey, token);

        // Verify that the saved token can be read back.
        string? savedToken = await SecureStorage.Default.GetAsync(TokenKey);

        if (string.IsNullOrWhiteSpace(savedToken))
        {
            throw new InvalidOperationException(
                "The authentication token could not be saved.");
        }

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", savedToken);
    }

    private static async Task<AuthResponse> ReadResponseAsync(
        HttpResponseMessage response)
    {
        string body = await response.Content.ReadAsStringAsync();

        AuthResponse? result = null;

        try
        {
            result = JsonSerializer.Deserialize<AuthResponse>(
                body, JsonOptions);
        }
        catch (JsonException)
        {
            // Handled below.
        }

        if (!response.IsSuccessStatusCode)
        {
            return CreateFailure(
                result?.Message ??
                $"Request failed with HTTP {(int)response.StatusCode}.");
        }

        return result ?? CreateFailure(
            "The server returned an invalid response.");
    }

    public void Logout()
    {
        try
        {
            SecureStorage.Default.Remove(TokenKey);
        }
        catch (Exception)
        {
            // Continue clearing the in-memory session.
        }

        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    private static AuthResponse CreateFailure(string message)
    {
        return new AuthResponse
        {
            Success = false,
            Message = message
        };
    }
}
