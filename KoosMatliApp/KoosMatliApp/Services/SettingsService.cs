using System.Net.Http.Json;
using System.Text.Json;
using KoosMatliApp.Models;

namespace KoosMatliApp.Services;

public sealed class SettingsService(HttpClient http, AdminSession auth)
{
    private readonly SemaphoreSlim _gate = new(1, 1);

    public UserSettings Current { get; private set; } = new();

    public async Task LoadAsync()
    {
        await _gate.WaitAsync();

        try
        {
            using var response = await http.GetAsync("api/settings");

            await AdminApiService.EnsureSuccessAsync(response);

            Current = await response.Content
                .ReadFromJsonAsync<UserSettings>() ?? new();

            await ApplyAsync();
        }
        finally
        {
            _gate.Release();
        }
    }

    public async Task SaveAsync(UserSettings next)
    {
        await _gate.WaitAsync();

        try
        {
            if (JsonSerializer.Serialize(Current) !=
                JsonSerializer.Serialize(next))
            {
                using var response = await http.PutAsJsonAsync(
                    "api/settings", next);

                await AdminApiService.EnsureSuccessAsync(response);

                Current = await response.Content
                    .ReadFromJsonAsync<UserSettings>()
                    ?? throw new HttpRequestException(
                        L.T("The server returned no saved record."));

                SettingsAuditStore.Log(auth.Uid, "Settings updated");
            }

            await ApplyAsync();
        }
        finally
        {
            _gate.Release();
        }
    }

    public UserSettings Copy() =>
        JsonSerializer.Deserialize<UserSettings>(
            JsonSerializer.Serialize(Current))!;

    public void Reset()
    {
        Current = new();
    }

    private Task ApplyAsync() =>
        MainThread.InvokeOnMainThreadAsync(() =>
        {
            L.SetCulture(Current.Language);

            if (Application.Current is { } app)
            {
                app.UserAppTheme = Current.Theme switch
                {
                    "Light" => AppTheme.Light,
                    "Dark" => AppTheme.Dark,
                    _ => AppTheme.Unspecified
                };
            }
        });
}