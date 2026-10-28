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

    // For Windows, use http://localhost:5000.
    // For the Android emulator, use http://10.0.2.2:5000
    // when your API is running on your development computer.
    private const string BaseUrl = "http://localhost:5000";

    private static readonly JsonSerializerOptions JsonOptions =
        new(JsonSerializerDefaults.Web);

    public AuthService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(BaseUrl),
            Timeout = TimeSpan.FromSeconds(30)
        };

        SetAuthorizationHeader();
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
            return new AuthResponse
            {
                Success = false,
                Message = "The request timed out. Please try again."
            };
        }
        catch (HttpRequestException)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Unable to connect to the server. Check your connection."
            };
        }
        catch (Exception)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "An unexpected error occurred during login."
            };
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
                "/api/auth/register",
                request);

            return await ProcessAuthResponseAsync(response);
        }
        catch (TaskCanceledException)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "The request timed out. Please try again."
            };
        }
        catch (HttpRequestException)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Unable to connect to the server. Check your connection."
            };
        }
        catch (Exception)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "An unexpected error occurred during registration."
            };
        }
    }

    // GET CURRENT USER
    public async Task<AuthResponse> GetCurrentUserAsync()
    {
        try
        {
            SetAuthorizationHeader();

            var response = await _httpClient.GetAsync("/api/auth/me");

            return await ProcessAuthResponseAsync(response);
        }
        catch (TaskCanceledException)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "The request timed out. Please try again."
            };
        }
        catch (HttpRequestException)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Unable to connect to the server."
            };
        }
        catch (Exception)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Unable to retrieve your account details."
            };
        }
    }

    // PROCESS API RESPONSES
    private async Task<AuthResponse> ProcessAuthResponseAsync(
        HttpResponseMessage response)
    {
        var responseBody = await response.Content.ReadAsStringAsync();

        AuthResponse? result = null;

        try
        {
            result = JsonSerializer.Deserialize<AuthResponse>(
                responseBody,
                JsonOptions);
        }
        catch (JsonException)
        {
            // The response may not be JSON.
        }

        if (!response.IsSuccessStatusCode)
        {
            return new AuthResponse
            {
                Success = false,
                Message = result?.Message
                    ?? $"Request failed ({(int)response.StatusCode})."
            };
        }

        if (result == null)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "The server returned an invalid response."
            };
        }

        if (result.Token is not null)
        {
            await SecureStorage.Default.SetAsync(
                TokenKey,
                result.Token);

            SetAuthorizationHeader();
        }

        return result;
    }

    // RESTORE TOKEN FOR FUTURE API REQUESTS
    private async void SetAuthorizationHeader()
    {
        try
        {
            var token = await SecureStorage.Default.GetAsync(TokenKey);

            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }
        }
        catch (Exception)
        {
            // Secure storage may be unavailable on some platforms.
        }
    }

    // LOG OUT
    public void Logout()
    {
        SecureStorage.Default.Remove(TokenKey);
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }
}
