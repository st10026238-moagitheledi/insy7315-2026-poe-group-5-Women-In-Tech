namespace KoosMatliApp.Models;

public class SchoolDocument
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string FileName { get; set; } = string.Empty;

    public string FileUrl { get; set; } = string.Empty;

    public DateTime UploadedDate { get; set; } //(Microsoft Learn, 2026).

    public string Category { get; set; } = string.Empty;

    public string Details => L.F("{0} — uploaded {1:d}", Category, UploadedDate.ToLocalTime());
}
/*Reference List
 
 Microsoft Corporation, "Model-View-ViewModel (MVVM) in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/xaml/mvvm
 */