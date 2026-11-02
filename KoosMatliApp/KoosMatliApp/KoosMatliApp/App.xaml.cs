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
        KoosMatliApp.Resources.Localization.LocalizationManager.Instance.Initialize();
        InitializeComponent();
        L.SetCulture(Preferences.Default.Get("KoosMatli.Language", "en-ZA")); //(Microsoft Learn, 2026c).
    }

    protected override Window CreateWindow(IActivationState? activationState) => //(Microsoft Learn, 2026a).
        new Window(new NavigationPage(new KoosMatliApp.Views.Authentication.SplashPage()));

    public static void ShowLogin()
    {
        ClearAdminSession();
        AuthService.Instance.Logout();
        Services?.GetRequiredService<KoosMatliApp.Features.Teacher.Services.TeacherApiService>().ClearSession(); //(Microsoft Learn, 2026b).
        if (Current is { } app)
        {
            app.UserAppTheme = AppTheme.Unspecified;
            if (app.Windows.FirstOrDefault() is { } window)
                window.Page = new NavigationPage(new KoosMatliApp.Views.Authentication.LoginPage());
        }
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

            await MainThread.InvokeOnMainThreadAsync(ShowShell); //(Microsoft Learn, 2026d).
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"Opening admin failed: {ex}");

            session.Clear();
            settings.Reset();

            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                ShowLogin();

                if (Current?.Windows.FirstOrDefault()?.Page is { } page)
                {
                    await page.DisplayAlertAsync(
                        "Unable to open administrator dashboard",
                        ex.Message,
                        "OK");
                }
            });

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
            window.Page = new AdminShell();
    }
}

/*Reference List
 
 Microsoft Corporation, "App and Window Lifecycle in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/app-lifecycle
 Microsoft Corporation, "Dependency Injection in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/dependency-injection

 Microsoft Corporation, "Preferences in .NET MAUI Essentials," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/storage/preferences

 Microsoft Corporation, "MainThread in .NET MAUI Essentials," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/appmodel/main-thread
 */