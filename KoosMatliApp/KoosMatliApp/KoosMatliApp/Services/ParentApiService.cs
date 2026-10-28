using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Maui.Storage;

namespace KoosMatliApp.Services;

public class ParentApiService
{
    private readonly HttpClient _httpClient;

    private const string TokenKey = "auth_token";

    private const string WindowsBaseUrl = "http://localhost:5000";
    private const string AndroidEmulatorBaseUrl = "http://10.0.2.2:5000";

    public ParentApiService()
    {
        var baseUrl = DeviceInfo.Platform == DevicePlatform.Android
            ? AndroidEmulatorBaseUrl
            : WindowsBaseUrl;

        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseUrl),
            Timeout = TimeSpan.FromSeconds(30)
        };
    }

    public async Task<JsonDocument> GetDashboardAsync()
    {
        var token = await SecureStorage.Default.GetAsync(TokenKey);

        if (string.IsNullOrWhiteSpace(token))
        {
            throw new UnauthorizedAccessException(
                "Your session has expired. Please log in again.");
        }

        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            "/api/parent/dashboard");

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        using var response = await _httpClient.SendAsync(request);

        var responseBody =
            await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            string message =
                $"Unable to load the dashboard ({(int)response.StatusCode}).";

            try
            {
                using var errorJson = JsonDocument.Parse(responseBody);

                if (errorJson.RootElement.TryGetProperty(
                        "message", out var messageElement))
                {
                    message = messageElement.GetString() ?? message;
                }
            }
            catch (JsonException)
            {
                // Keep the default error message.
            }

            if (response.StatusCode ==
                System.Net.HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException(message);
            }

            throw new HttpRequestException(message);
        }

        return JsonDocument.Parse(responseBody);
    }
}
