namespace KoosMatliAdminApi.DTOs
{
    public class AdminDashboardDto
    {
        public int TotalLearners { get; set; }
        public int TotalTeachers { get; set; } //(Microsoft Learn, 2026a).
        public int TotalParents { get; set; }

        public int NewApplications { get; set; }
        public int PendingApplications { get; set; } //(Microsoft Learn, 2026a).
        public int AnnouncementsSent { get; set; }
    }
}

/*Reference List
 
Microsoft Corporation, "Create Web APIs with ASP.NET Core: Data Transfer Objects (DTOs)," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/aspnet/core/web-api/

Microsoft Corporation, "Auto-Implemented Properties (C# Programming Guide)," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/auto-implemented-properties
 */