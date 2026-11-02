using KoosMatliApp.Services;

using Microsoft.Extensions.Logging;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.LifecycleEvents;

using KoosMatliApp.Features.Teacher.Services;
using KoosMatliApp.Features.Teacher.ViewModels;

#if ANDROID
using Plugin.Firebase.Auth;
using Plugin.Firebase.Firestore;
using Plugin.Firebase.Bundled.Shared;
using Plugin.Firebase.Bundled.Platforms.Android;
#endif


namespace KoosMatliApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder =
                MauiApp.CreateBuilder();


            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont(
                        "OpenSans-Regular.ttf",
                        "OpenSansRegular");

                    fonts.AddFont(
                        "OpenSans-Semibold.ttf",
                        "OpenSansSemibold");
                }); // (Microsoft, 2026)


            // Firebase

            builder.RegisterFirebaseServices();


            // API Connection

            builder.Services.AddSingleton(
                new HttpClient
                {
                    BaseAddress =
                        new Uri(
                            ApiSettings.BaseUrl),

                    Timeout =
                        TimeSpan.FromSeconds(30)
                });


            // Shared / Parent Services

            builder.Services
                .AddSingleton(AuthService.Instance);

            builder.Services
                .AddSingleton<ParentApiService>();


            // Teacher Services

            builder.Services
                .AddSingleton<TeacherApiService>();

            builder.Services
                .AddSingleton<FirebaseAuthService>();


            // Teacher View Models

            builder.Services
                .AddTransient<ProfileHubViewModel>();

            builder.Services
                .AddTransient<PersonalInformationViewModel>();

            builder.Services
                .AddTransient<ClassesViewModel>();

            builder.Services
                .AddTransient<ClassDetailsViewModel>();

            builder.Services
                .AddTransient<AddLearnerViewModel>();

            builder.Services
                .AddTransient<LearnerDetailsViewModel>();

            builder.Services
                .AddTransient<UpdatesViewModel>();

            builder.Services
                .AddTransient<AddAnnouncementViewModel>(); // (Microsoft, 2026)

            builder.Services
                .AddTransient<AnnouncementDetailsViewModel>();

            builder.Services
                .AddTransient<TeacherHomeViewModel>();

            builder.Services
                .AddTransient<HomeworkViewModel>();

            builder.Services
                .AddTransient<AddHomeworkViewModel>();

            builder.Services
                .AddTransient<ScheduleViewModel>();

            builder.Services
                .AddTransient<AddScheduleViewModel>();

            builder.Services
                .AddTransient<ScheduleDetailsViewModel>();


#if DEBUG

            builder.Logging.AddDebug();

#endif


            builder.Services.AddSingleton(new KoosMatliApp.Services.AppConfiguration());
            builder.Services.AddSingleton<KoosMatliApp.Services.AdminSession>();
            builder.Services.AddSingleton<KoosMatliApp.Services.AdminHttpClient>();
            builder.Services.AddSingleton<KoosMatliApp.Services.IAdminApiService>(sp =>
                new KoosMatliApp.Services.AdminApiService(sp.GetRequiredService<KoosMatliApp.Services.AdminHttpClient>(), sp.GetRequiredService<KoosMatliApp.Services.AdminSession>()));
            builder.Services.AddSingleton<KoosMatliApp.Services.SettingsService>(sp =>
                new KoosMatliApp.Services.SettingsService(sp.GetRequiredService<KoosMatliApp.Services.AdminHttpClient>(),
                    sp.GetRequiredService<KoosMatliApp.Services.AdminSession>()));
            builder.Services.AddTransient<KoosMatliApp.ViewModels.AdminDashboardViewModel>();
            builder.Services.AddTransient<KoosMatliApp.ViewModels.UsersViewModel>();
            builder.Services.AddTransient<KoosMatliApp.ViewModels.ApplicationsViewModel>();
            builder.Services.AddTransient<KoosMatliApp.ViewModels.ApplicationsDetailViewModel>();
            builder.Services.AddTransient<KoosMatliApp.ViewModels.SchoolDetailsViewModel>();
            builder.Services.AddTransient<KoosMatliApp.ViewModels.SchoolDatesViewModel>();
            builder.Services.AddTransient<KoosMatliApp.ViewModels.StaffViewModel>();
            builder.Services.AddTransient<KoosMatliApp.ViewModels.DocumentsViewModel>();
            builder.Services.AddTransient<KoosMatliApp.ViewModels.ProfileViewModel>();

            var app = builder.Build();
            App.Services = app.Services;
            app.Services.GetRequiredService<KoosMatliApp.Services.AdminSession>()
                .AuthorizationRequired += (_, _) => MainThread.BeginInvokeOnMainThread(App.ShowLogin);
            return app;
        }


        private static MauiAppBuilder
            RegisterFirebaseServices(
                this MauiAppBuilder builder)
        {
#if ANDROID

            builder.ConfigureLifecycleEvents(
                events =>
                {
                    events.AddAndroid(
                        android =>
                            android.OnCreate(
                                (activity, _) =>
                                {
                                    var settings =
                                        new CrossFirebaseSettings(
                                            isAuthEnabled: true,
                                            isFirestoreEnabled: true, // (Microsoft, 2026)
                                            isStorageEnabled: false);


                                    CrossFirebase.Initialize(
                                        activity,
                                        () =>
                                            Platform
                                                .CurrentActivity,
                                        settings);
                                }));
                });


            builder.Services.AddSingleton(
                _ =>
                    CrossFirebaseAuth.Current);


            builder.Services.AddSingleton(
                _ =>
                    CrossFirebaseFirestore.Current);

#endif


            return builder; // (Microsoft, 2026)
        }
    }
}


// Reference List:
// 1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].