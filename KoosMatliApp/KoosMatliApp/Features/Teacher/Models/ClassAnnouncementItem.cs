namespace KoosMatliApp.Features.Teacher.Models;

public class ClassAnnouncementItem
{
    public string Id { get; set; } =
        string.Empty;

    public string Title { get; set; } =
        string.Empty; // (Microsoft, 2026)

    public string Message { get; set; } =
        string.Empty;

    public string Audience { get; set; } =
        string.Empty;
    // (Microsoft, 2026)
    public string Grade { get; set; } =
        string.Empty;

    public string Subject { get; set; } =
        string.Empty;

    public DateTime EventDate { get; set; } =
        DateTime.Today;
} // (Microsoft, 2026)

//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].