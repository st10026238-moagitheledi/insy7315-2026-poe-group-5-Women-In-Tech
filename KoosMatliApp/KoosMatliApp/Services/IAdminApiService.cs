using KoosMatliApp.Models;

namespace KoosMatliApp.Services;

public interface IAdminApiService
{
    Task<AdminDashboard?> GetDashboardAsync();

    Task<List<User>> GetUsersAsync(string? role = null);

    Task<List<AdmissionApplication>> GetApplicationsAsync();
    Task<AdmissionApplication?> GetApplicationAsync(int id);
    Task UpdateApplicationStatusAsync(int id, string status);

    Task<List<StaffMember>> GetStaffAsync();
    Task<List<SchoolClass>> GetClassesAsync();
    Task<List<Subject>> GetSubjectsAsync();

    Task<SchoolDetails?> GetSchoolDetailsAsync();
    Task<SchoolDetails?> UpdateSchoolDetailsAsync(SchoolDetails details);
    Task<List<SchoolDate>> GetSchoolDatesAsync();
    Task<SchoolDate?> AddSchoolDateAsync(SchoolDate schoolDate);
    Task<List<CalendarEvent>> GetCalendarEventsAsync();

    Task<List<SchoolDocument>> GetDocumentsAsync();
    Task<CalendarEvent?> AddCalendarEventAsync(CalendarEvent calendarEvent);
    Task<User?> AddUserAsync(User user);
}
