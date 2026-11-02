namespace KoosMatliApp.Models;

public class CalendarEvent
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public DateTime Start { get; set; } //(Microsoft Learn, 2026).

    public DateTime? End { get; set; }

    public string Description { get; set; } = string.Empty;

    public string DateText =>
        End.HasValue && End.Value.Date != Start.Date
            ? $"{Start:dd MMMM yyyy} - {End.Value:dd MMMM yyyy}"
            : Start.ToString("dd MMMM yyyy");
}
/*Reference List
 
 Microsoft Corporation, "Model-View-ViewModel (MVVM) in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/xaml/mvvm
 */