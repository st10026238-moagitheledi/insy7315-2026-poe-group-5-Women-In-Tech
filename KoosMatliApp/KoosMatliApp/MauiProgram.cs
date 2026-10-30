using Microsoft.Extensions.Logging;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.LifecycleEvents;
using KoosMatliApp.Features.Teacher.Services;

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
                });


            builder.RegisterFirebaseServices();

            builder.Services.AddSingleton<FirebaseAuthService>();


#if DEBUG
            builder.Logging.AddDebug();
#endif


            return builder.Build();
        }


        private static MauiAppBuilder RegisterFirebaseServices(
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
                                            isFirestoreEnabled: true,
                                            isStorageEnabled: false);


                                    CrossFirebase.Initialize(
                                        activity,
                                        () => Platform.CurrentActivity,
                                        settings);
                                }));
                });


            builder.Services.AddSingleton(
                _ => CrossFirebaseAuth.Current);


            builder.Services.AddSingleton(
                _ => CrossFirebaseFirestore.Current);

#endif

            return builder;
        }
    }
}