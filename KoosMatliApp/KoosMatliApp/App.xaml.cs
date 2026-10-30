using KoosMatliApp.Localization;
using KoosMatliApp.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Storage;

namespace KoosMatliApp;

public partial class App : Application
{
    public static IServiceProvider? Services { get; set; }

    public App()
    {
        InitializeComponent();

        L.SetCulture(
            Preferences.Default.Get(
                "KoosMatli.Language",
                "en-ZA"));
    }

    protected override Window CreateWindow(
        IActivationState? activationState)
    {
#if DEBUG
        return new Window(
            new KoosMatliApp.Views.AdminTestSignInPage());
#else
        return new Window(new AppShell());
#endif
    }

    public static async Task EnterAdminAsync(
        string uid,
        string email,
        Func<CancellationToken, Task<string?>> tokenProvider)
    {
        var services = Services
            ?? throw new InvalidOperationException(
                "The admin module has not started.");

        var session =
            services.GetRequiredService<AdminSession>();

        var settings =
            services.GetRequiredService<SettingsService>();

        session.Connect(uid, email, tokenProvider);
        settings.Reset();

        try
        {
            // Confirm that the backend accepts this administrator.
            await services
                .GetRequiredService<IAdminApiService>()
                .GetDashboardAsync();

            // Load the user's saved language, theme and preferences.
            await settings.LoadAsync();

            await MainThread.InvokeOnMainThreadAsync(ShowShell);
        }
        catch
        {
            session.Clear();
            settings.Reset();
            throw;
        }
    }

    public static void ClearAdminSession()
    {
        Services?
            .GetRequiredService<AdminSession>()
            .Clear();

        Services?
            .GetRequiredService<SettingsService>()
            .Reset();
    }

    public static void ShowShell()
    {
        if (Current?.Windows.FirstOrDefault() is { } window)
            window.Page = new AppShell();
    }
}