namespace KoosMatliApp.Features.Teacher.Models;

public class ClassResourceItem
{
    public string Title { get; set; } = string.Empty;


    public string Description { get; set; } = string.Empty; // (Microsoft, 2026)


    public string ResourceType { get; set; } = string.Empty;


    public string FileName { get; set; } = string.Empty; // (Microsoft, 2026)


    public string FilePath { get; set; } = string.Empty;
}


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].