
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using KoosMatliApp.Services;
using KoosMatliApp.ViewModels;

namespace KoosMatliApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton(new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7251/")
        });

        builder.Services.AddSingleton<IAdminApiService, AdminApiService>();
        builder.Services.AddTransient<SchoolDatesViewModel>();
       
       // builder.Services.AddTransient<SchoolDetailsViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();

        App.Services = app.Services;

        return app;
    }
}