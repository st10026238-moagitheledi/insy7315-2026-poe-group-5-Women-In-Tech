namespace KoosMatliApp.Models;

public class SchoolDate
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public string Description { get; set; } = string.Empty;

    public string FormattedDate => Date.ToString("dd MMM yyyy");

    public string DayNumber => Date.ToString("dd");

    public string MonthName => Date.ToString("MMM");
}