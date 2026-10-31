namespace KoosMatliApp.Features.Teacher.Models;

public class TeacherSettings
{
    public bool NotificationsEnabled { get; set; } = true;

    public bool HomeworkNotifications { get; set; } = true;

    public bool AnnouncementNotifications { get; set; } = true; // (Microsoft, 2026)

    public string Language { get; set; } = "English";
}


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].

