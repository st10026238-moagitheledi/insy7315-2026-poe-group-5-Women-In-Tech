namespace KoosMatliApp.Features.Teacher.Models;

public class TeacherSettings
{
    public bool NotificationsEnabled { get; set; } = true;

    public bool HomeworkNotifications { get; set; } = true;

    public bool AnnouncementNotifications { get; set; } = true;

    public string Language { get; set; } = "English";
}