namespace KoosMatliApp.Models;

public class SchoolDocument
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string FileName { get; set; } = string.Empty;

    public string FileUrl { get; set; } = string.Empty;

    public DateTime UploadedDate { get; set; }

    public string Category { get; set; } = string.Empty;
}