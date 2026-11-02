namespace KoosMatliApp.Models;

public class Subject
{
    public int Id { get; set; } //(Microsoft Learn, 2026).

    public string Name { get; set; } = string.Empty;

    public string Grade { get; set; } = string.Empty;

    public List<string> TeacherNames { get; set; } = new();

    public string TeacherNamesText =>
        TeacherNames.Count == 0
            ? "No teacher assigned"
            : string.Join(", ", TeacherNames);
}
/*Reference List
 
 Microsoft Corporation, "Model-View-ViewModel (MVVM) in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/xaml/mvvm
 */