namespace KoosMatliApp.Models;

public class StaffMember
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty; //(Microsoft Learn, 2026).

    public string Position { get; set; } = string.Empty;

    public List<string> Subjects { get; set; } = new();

    public List<string> Classes { get; set; } = new();

    public string FullName => $"{FirstName} {LastName}";

    public string SubjectsText =>
        Subjects.Count == 0 ? "Subjects: none assigned" : $"Subjects: {string.Join(", ", Subjects)}";

    public string ClassesText =>
        Classes.Count == 0 ? "Classes: none assigned" : $"Classes: {string.Join(", ", Classes)}";
}
/*Reference List
 
 Microsoft Corporation, "Model-View-ViewModel (MVVM) in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/xaml/mvvm
 */