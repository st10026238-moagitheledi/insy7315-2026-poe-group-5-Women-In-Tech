namespace KoosMatliApp.Models;

public class CalendarEvent
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Description { get; set; } = string.Empty;

    public string EventType { get; set; } = string.Empty;
}