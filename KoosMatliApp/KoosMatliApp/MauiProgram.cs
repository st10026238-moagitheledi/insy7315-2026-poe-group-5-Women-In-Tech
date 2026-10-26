using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using KoosMatliApp.Services;
using KoosMatliApp.ViewModels;
using System;

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

            // Register HTTP client for the Admin API. Update BaseAddress to match API host/port if needed.
            builder.Services.AddHttpClient<IAdminApiService, AdminApiService>(c =>
            {
                c.BaseAddress = new Uri("https://localhost:7251/");
            });

            // Register ViewModels
            builder.Services.AddTransient<AdminDashboardViewModel>();
            builder.Services.AddTransient<UsersViewModel>();
            builder.Services.AddTransient<ApplicationsViewModel>();
            builder.Services.AddTransient<StaffViewModel>();
            builder.Services.AddTransient<SchoolDetailsViewModel>();
            builder.Services.AddTransient<DocumentsViewModel>();

            var mauiApp = builder.Build();

            // expose service provider for code-behind pages to resolve viewmodels
            App.Services = mauiApp.Services;

            return mauiApp;
    }
}