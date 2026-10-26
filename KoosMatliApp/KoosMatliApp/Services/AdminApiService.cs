using System.Net.Http.Json;
using System.Text.Json;
using KoosMatliApp.Models;

namespace KoosMatliApp.Services;

public class AdminApiService : IAdminApiService
{
    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public AdminApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<AdminDashboard?> GetDashboardAsync()
    {
        var res = await _http.GetAsync("api/admin/dashboard");
        if (!res.IsSuccessStatusCode) return null;
        return await res.Content.ReadFromJsonAsync<AdminDashboard>(_jsonOptions);
    }

    public async Task<List<User>> GetUsersAsync(string? role = null)
    {
        var url = string.IsNullOrEmpty(role) ? "api/admin/users" : $"api/admin/users/role/{role}";
        var res = await _http.GetAsync(url);
        if (!res.IsSuccessStatusCode) return new List<User>();
        return await res.Content.ReadFromJsonAsync<List<User>>(_jsonOptions) ?? new List<User>();
    }

    public async Task<List<AdmissionApplication>> GetApplicationsAsync()
    {
        var res = await _http.GetAsync("api/admin/applications");
        if (!res.IsSuccessStatusCode) return new List<AdmissionApplication>();
        return await res.Content.ReadFromJsonAsync<List<AdmissionApplication>>(_jsonOptions) ?? new List<AdmissionApplication>();
    }

    public async Task<AdmissionApplication?> GetApplicationAsync(int id)
    {
        var res = await _http.GetAsync($"api/admin/applications/{id}");
        if (!res.IsSuccessStatusCode) return null;
        return await res.Content.ReadFromJsonAsync<AdmissionApplication>(_jsonOptions);
    }

    public async Task UpdateApplicationStatusAsync(int id, string status)
    {
        var dto = new { Status = status };
        var res = await _http.PutAsJsonAsync($"api/admin/applications/{id}/status", dto);
        res.EnsureSuccessStatusCode();
    }

    public async Task<List<StaffMember>> GetStaffAsync()
    {
        var res = await _http.GetAsync("api/admin/staff");
        if (!res.IsSuccessStatusCode) return new List<StaffMember>();
        return await res.Content.ReadFromJsonAsync<List<StaffMember>>(_jsonOptions) ?? new List<StaffMember>();
    }

    public async Task<SchoolDetails?> GetSchoolDetailsAsync()
    {
        var res = await _http.GetAsync("api/admin/school/details");
        if (!res.IsSuccessStatusCode) return null;
        return await res.Content.ReadFromJsonAsync<SchoolDetails>(_jsonOptions);
    }

    public async Task<List<SchoolDate>> GetSchoolDatesAsync()
    {
        var res = await _http.GetAsync("api/admin/school/dates");
        if (!res.IsSuccessStatusCode) return new List<SchoolDate>();
        return await res.Content.ReadFromJsonAsync<List<SchoolDate>>(_jsonOptions) ?? new List<SchoolDate>();
    }

    public async Task<SchoolDate?> AddSchoolDateAsync(SchoolDate schoolDate)
    {
        var response = await _http.PostAsJsonAsync(
            "api/admin/school/dates",
            schoolDate,
            _jsonOptions);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();

            throw new HttpRequestException(
                $"Adding the school date failed: {(int)response.StatusCode} " +
                $"{response.ReasonPhrase}. {error}");
        }

        return await response.Content.ReadFromJsonAsync<SchoolDate>(_jsonOptions);
    }

    public async Task<List<CalendarEvent>> GetCalendarEventsAsync()
    {
        var res = await _http.GetAsync("api/admin/school/calendar");
        if (!res.IsSuccessStatusCode) return new List<CalendarEvent>();
        return await res.Content.ReadFromJsonAsync<List<CalendarEvent>>(_jsonOptions) ?? new List<CalendarEvent>();
    }

    public async Task<List<SchoolDocument>> GetDocumentsAsync()
    {
        var res = await _http.GetAsync("api/admin/documents");
        if (!res.IsSuccessStatusCode) return new List<SchoolDocument>();
        return await res.Content.ReadFromJsonAsync<List<SchoolDocument>>(_jsonOptions) ?? new List<SchoolDocument>();
    }
}
