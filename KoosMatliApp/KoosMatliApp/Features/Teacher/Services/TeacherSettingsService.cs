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
        };


    public static TeacherSettings GetSettings()
    {
        _settings.Language =
            Preferences.Default.Get(
                LanguageKey,
                "English");


        return _settings;
    }


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
            language);
    }
}
