using Microsoft.Extensions.Logging;
using KoosMatliApp.Services;
using KoosMatliApp.ViewModels;

namespace KoosMatliApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder()
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton(new AppConfiguration());
        builder.Services.AddSingleton<AdminSession>();

        builder.Services.AddSingleton(sp =>
            new HttpClient(
                new ApiRequestHandler(
                    sp.GetRequiredService<AdminSession>())
                {
                    InnerHandler = new HttpClientHandler()
                })
            {
                BaseAddress =
                    sp.GetRequiredService<AppConfiguration>().ApiUri,
                Timeout = Timeout.InfiniteTimeSpan
            });

        builder.Services.AddSingleton<IAdminApiService, AdminApiService>();
        builder.Services.AddSingleton<SettingsService>();

        builder.Services.AddTransient<AdminDashboardViewModel>();
        builder.Services.AddTransient<UsersViewModel>();
        builder.Services.AddTransient<ApplicationsViewModel>();
        builder.Services.AddTransient<ApplicationsDetailViewModel>();
        builder.Services.AddTransient<SchoolDetailsViewModel>();
        builder.Services.AddTransient<SchoolDatesViewModel>();
        builder.Services.AddTransient<StaffViewModel>();
        builder.Services.AddTransient<DocumentsViewModel>();
        builder.Services.AddTransient<ProfileViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();
        App.Services = app.Services;

        return app;
    }
}