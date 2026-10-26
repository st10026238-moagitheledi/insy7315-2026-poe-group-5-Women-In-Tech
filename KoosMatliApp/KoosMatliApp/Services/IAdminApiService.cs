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

    Task<SchoolDetails?> GetSchoolDetailsAsync();
    Task<List<SchoolDate>> GetSchoolDatesAsync();
    Task<SchoolDate?> AddSchoolDateAsync(SchoolDate schoolDate);
    Task<List<CalendarEvent>> GetCalendarEventsAsync();

    Task<List<SchoolDocument>> GetDocumentsAsync();
}
