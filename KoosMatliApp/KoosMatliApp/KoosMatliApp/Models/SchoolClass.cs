namespace KoosMatliApp.Models;

public class SchoolClass
{
    public int Id { get; set; }

    public string ClassName { get; set; } = string.Empty;

    public string Grade { get; set; } = string.Empty; //(Microsoft Learn, 2026).

    public string TeacherName { get; set; } = string.Empty;

    public int LearnerCount { get; set; }
}
/*Reference List
 
 Microsoft Corporation, "Model-View-ViewModel (MVVM) in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/xaml/mvvm
 */
