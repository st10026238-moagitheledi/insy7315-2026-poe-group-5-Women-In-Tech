namespace KoosMatliApp.Models;

public class SchoolDate
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public DateTime Date { get; set; } // (Microsoft Learn, 2026).

    public string Description { get; set; } = string.Empty;

    public string FormattedDate => Date.ToString("dd MMM yyyy");

    public string DayNumber => Date.ToString("dd");

    public string MonthName => Date.ToString("MMM");
}
/*Reference List
 
 Microsoft Corporation, "Model-View-ViewModel (MVVM) in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/xaml/mvvm
 */