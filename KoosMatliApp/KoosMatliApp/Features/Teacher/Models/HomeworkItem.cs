namespace KoosMatliApp.Features.Teacher.Models;

public class HomeworkItem
{
    public string Id { get; set; } =
        string.Empty;

    public string Title { get; set; } =
        string.Empty;

    public string Description { get; set; } =
        string.Empty;

    public string DueDate { get; set; } =
        string.Empty; // (Microsoft, 2026)

    public string Status { get; set; } =
        string.Empty;

    public string Grade { get; set; } =
        string.Empty;

    public string Subject { get; set; } =
        string.Empty;

    public string AttachmentName { get; set; } =
        string.Empty;

    public string AttachmentPath { get; set; } =
        string.Empty; // (Microsoft, 2026)
}



//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].
