namespace KoosMatliApp.Features.Teacher.Models;

public class ClassAnnouncementItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Title { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public string Audience { get; set; } = string.Empty;

    public string Grade { get; set; } = string.Empty;

    public string Subject { get; set; } = string.Empty;

    public DateTime EventDate { get; set; } = DateTime.Today;
}