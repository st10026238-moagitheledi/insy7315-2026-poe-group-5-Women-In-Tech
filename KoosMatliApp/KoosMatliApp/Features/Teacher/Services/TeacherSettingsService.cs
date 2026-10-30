using KoosMatliApp.Features.Teacher.Models;

namespace KoosMatliApp.Features.Teacher.Services;

public static class TeacherSettingsService
{
    private static readonly TeacherSettings _settings =
        new();


    public static TeacherSettings GetSettings()
    {
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
        _settings.Language = language;
    }
}
