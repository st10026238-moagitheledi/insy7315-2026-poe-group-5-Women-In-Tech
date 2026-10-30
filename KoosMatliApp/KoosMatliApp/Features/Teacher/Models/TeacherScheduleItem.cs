namespace KoosMatliApp.Features.Teacher.Models;

public class TeacherScheduleItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string EventType { get; set; } = string.Empty;

    public DateTime Date { get; set; } = DateTime.Today;

    public TimeSpan StartTime { get; set; } = TimeSpan.Zero;

    public TimeSpan EndTime { get; set; } = TimeSpan.Zero;

    public string Grade { get; set; } = string.Empty;

    public string Subject { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public string DateDisplay => Date.ToString("ddd, dd MMM yyyy");

    public string TimeDisplay =>
        $"{DateTime.Today.Add(StartTime):HH:mm} - {DateTime.Today.Add(EndTime):HH:mm}";

    public string ClassDisplay
    {
        get
        {
            if (string.IsNullOrWhiteSpace(Grade))
            {
                return EventType;
            }

            if (string.IsNullOrWhiteSpace(Subject))
            {
                return Grade;
            }

            return $"{Grade} • {Subject}";
        }
    }
}
