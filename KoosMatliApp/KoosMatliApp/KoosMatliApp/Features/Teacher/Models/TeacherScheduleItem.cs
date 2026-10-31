namespace KoosMatliApp.Features.Teacher.Models;

public class TeacherScheduleItem
{
    public string Id { get; set; } =
        string.Empty;

    public string Title { get; set; } =
        string.Empty;

    public string Description { get; set; } =  // (Microsoft, 2026)
        string.Empty;

    public string EventType { get; set; } =
        string.Empty;

    public DateTime Date { get; set; } =
        DateTime.Today;

    public TimeSpan StartTime { get; set; } =
        TimeSpan.Zero;

    public TimeSpan EndTime { get; set; } =  // (Microsoft, 2026)
        TimeSpan.Zero;

    public string Grade { get; set; } =
        string.Empty;

    public string Subject { get; set; } =
        string.Empty;

    public string Location { get; set; } =
        string.Empty;


    public string DateDisplay =>
        Date.ToString(
            "ddd, dd MMM yyyy"); // (Microsoft, 2026)


    public string TimeDisplay =>
        $"{DateTime.Today.Add(StartTime):HH:mm} - " +
        $"{DateTime.Today.Add(EndTime):HH:mm}";


    public string ClassDisplay
    {
        get
        {
            if (string.IsNullOrWhiteSpace(
                    Grade))
            {
                return EventType;
            }


            if (string.IsNullOrWhiteSpace(
                    Subject))
            {
                return Grade;
            }


            return $"{Grade} • {Subject}";
        }
    } // (Microsoft, 2026)


    public static TeacherScheduleItem FromApi(
        TeacherScheduleApiModel apiItem)
    {
        var date =
            DateTime.Today;

        var startTime =
            TimeSpan.Zero;

        var endTime =
            TimeSpan.Zero;


        DateTime.TryParse(
            apiItem.Date,
            out date);


        TimeSpan.TryParse(
            apiItem.StartTime,
            out startTime);


        TimeSpan.TryParse(
            apiItem.EndTime,
            out endTime);


        return new TeacherScheduleItem
        {
            Id =
                apiItem.Id,

            Title =
                apiItem.Title,

            Description =
                apiItem.Description,

            EventType =
                apiItem.EventType,

            Date =
                date,

            StartTime =
                startTime,

            EndTime =
                endTime,  // (Microsoft, 2026)

            Grade =
                apiItem.Grade,

            Subject =
                apiItem.Subject,

            Location =
                apiItem.Location
        };
    }
}


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].

