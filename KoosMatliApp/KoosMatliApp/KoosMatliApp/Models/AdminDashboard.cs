namespace KoosMatliApp.Models;

public class AdminDashboard
{
    public int TotalLearners { get; set; }
    public int TotalTeachers { get; set; }
    public int TotalParents { get; set; }

    public int NewApplications { get; set; }
    public int PendingApplications { get; set; } //(Microsoft Learn, 2026).
    public int AnnouncementsSent { get; set; }
}
/*Reference List
 
 Microsoft Corporation, "Model-View-ViewModel (MVVM) in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/xaml/mvvm
 */