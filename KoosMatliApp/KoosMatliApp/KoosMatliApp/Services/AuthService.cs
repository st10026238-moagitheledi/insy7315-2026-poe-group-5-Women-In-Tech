
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

    // Windows: http://localhost:5000
    // Android emulator: http://10.0.2.2:5000
    private const string BaseUrl = "http://localhost:5000";

    private static readonly JsonSerializerOptions JsonOptions =
        new(JsonSerializerDefaults.Web);

    // One shared authentication service for the app.
    public static AuthService Instance { get; } = new();

    private AuthService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(BaseUrl),
            Timeout = TimeSpan.FromSeconds(30)
        };
    }

    // LOGIN
    public async Task<AuthResponse> LoginAsync(
        string email,
        string password)
    {
        var request = new LoginRequest
        {
            Email = email.Trim(),
            Password = password
        };

        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                "/api/auth/login",
                request);

            return await ProcessAuthResponseAsync(response);
        }
        catch (TaskCanceledException)
        {
            return CreateFailure(
                "The request timed out. Please try again.");
        }
        catch (HttpRequestException)
        {
            return CreateFailure(
                "Unable to connect to the server. Check your connection.");
        }
        catch (Exception)
        {
            return CreateFailure(
                "An unexpected error occurred during login.");
        }
    }

    // PARENT REGISTRATION
    public async Task<AuthResponse> RegisterParentAsync(
        string displayName,
        string email,
        string password)
    {
        var request = new RegisterRequest
        {
            DisplayName = displayName.Trim(),
            Email = email.Trim(),
            Password = password
        };

        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                "/api/auth/register-parent",
                request);

            return await ProcessAuthResponseAsync(response);
        }
        catch (TaskCanceledException)
        {
            return CreateFailure(
                "The request timed out. Please try again.");
        }
        catch (HttpRequestException)
        {
            return CreateFailure(
                "Unable to connect to the server. Check your connection.");
        }
        catch (Exception)
        {
            return CreateFailure(
                "An unexpected error occurred during registration.");
        }
    }

    // GET CURRENT USER
    public async Task<AuthResponse> GetCurrentUserAsync()
    {
        try
        {
            await RestoreAuthorizationHeaderAsync();

            var response = await _httpClient.GetAsync("/api/auth/me");

            return await ProcessAuthResponseAsync(response);
        }
        catch (TaskCanceledException)
        {
            return CreateFailure(
                "The request timed out. Please try again.");
        }
        catch (HttpRequestException)
        {
            return CreateFailure(
                "Unable to connect to the server.");
        }
        catch (Exception)
        {
            return CreateFailure(
                "Unable to retrieve your account details.");
        }
    }

    // PROCESS API RESPONSES
    private async Task<AuthResponse> ProcessAuthResponseAsync(
        HttpResponseMessage response)
    {
        using (response)
        {
            string responseBody =
                await response.Content.ReadAsStringAsync();

            AuthResponse? result = null;

            try
            {
                result = JsonSerializer.Deserialize<AuthResponse>(
                    responseBody,
                    JsonOptions);
            }
            catch (JsonException)
            {
                // The server response was not valid JSON.
            }

            if (!response.IsSuccessStatusCode)
            {
                return CreateFailure(
                    result?.Message ??
                    $"Request failed ({(int)response.StatusCode}).");
            }

            if (result == null)
            {
                return CreateFailure(
                    "The server returned an invalid response.");
            }

            if (result.Token is not null)
            {
                await SecureStorage.Default.SetAsync(
                    TokenKey,
                    result.Token);

                await RestoreAuthorizationHeaderAsync();
            }

            return result;
        }
    }

    // RESTORE SAVED TOKEN
    private async Task RestoreAuthorizationHeaderAsync()
    {
        try
        {
            string? token =
                await SecureStorage.Default.GetAsync(TokenKey);

            _httpClient.DefaultRequestHeaders.Authorization =
                string.IsNullOrWhiteSpace(token)
                    ? null
                    : new AuthenticationHeaderValue(
                        "Bearer",
                        token);
        }
        catch (Exception)
        {
            // Secure storage may be unavailable.
        }
    }

    // LOG OUT
    public void Logout()
    {
        SecureStorage.Default.Remove(TokenKey);
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
