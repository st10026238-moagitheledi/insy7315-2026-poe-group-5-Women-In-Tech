using Microsoft.Maui.Storage;
using KoosMatliApp.Features.Teacher.Models;

namespace KoosMatliApp.Features.Teacher.Services;

public static class TeacherSettingsService
{
    private const string LanguageKey =
        "TeacherLanguage";


    private static readonly TeacherSettings _settings =
        new()
        {
            Language =
                Preferences.Default.Get(
                    LanguageKey,
                    "English")
        }; // (Microsoft, 2026)


    public static TeacherSettings GetSettings()
    {
        _settings.Language =
            Preferences.Default.Get(
                LanguageKey,
                "English");


        return _settings;
    } // (Microsoft, 2026)


    public static void UpdateNotifications(
        bool notificationsEnabled,
        bool homeworkNotifications,
        bool announcementNotifications)
    {
        _settings.NotificationsEnabled = notificationsEnabled;

        _settings.HomeworkNotifications = homeworkNotifications;

        _settings.AnnouncementNotifications = announcementNotifications;
    }


    public static void UpdateLanguage(
        string language)
    {
        _settings.Language =
            language;


        Preferences.Default.Set(
            LanguageKey,
            language); // (Microsoft, 2026)
    }
}


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].

