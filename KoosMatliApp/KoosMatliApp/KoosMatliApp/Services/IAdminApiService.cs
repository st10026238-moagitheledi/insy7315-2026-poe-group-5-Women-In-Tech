using KoosMatliApp.Models;

namespace KoosMatliApp.Services;

public interface IAdminApiService //(Microsoft Learn, 2026a, Microsoft Learn, 2026c).
{
    Task<AdminDashboard?> GetDashboardAsync();

    Task<List<User>> GetUsersAsync(string? role = null); //(Microsoft Learn, 2026b).

    Task<List<AdmissionApplication>> GetApplicationsAsync();
    Task<AdmissionApplication?> GetApplicationAsync(int id);
    Task UpdateApplicationStatusAsync(int id, string status);

    Task<List<StaffMember>> GetStaffAsync();
    Task<List<SchoolClass>> GetClassesAsync();
    Task<List<Subject>> GetSubjectsAsync(); //(Microsoft Learn, 2026b).

    Task<SchoolDetails?> GetSchoolDetailsAsync();
    Task<SchoolDetails?> UpdateSchoolDetailsAsync(SchoolDetails details);
    Task<List<SchoolDate>> GetSchoolDatesAsync();
    Task<SchoolDate?> AddSchoolDateAsync(SchoolDate schoolDate);
    Task<List<CalendarEvent>> GetCalendarEventsAsync();

    Task<List<SchoolDocument>> GetDocumentsAsync();
    Task<CalendarEvent?> AddCalendarEventAsync(CalendarEvent calendarEvent);
    Task<User?> AddUserAsync(User user);
}
/*Reference List
 
Microsoft Corporation, "Interfaces (C# Programming Guide)," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/interfaces/

Microsoft Corporation, "Asynchronous Programming with async and await (C#)," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/

Microsoft Corporation, "Dependency Injection and Services in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/dependency-injection
 */