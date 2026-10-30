using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using KoosMatliApp.Localization;
using KoosMatliApp.Models;

namespace KoosMatliApp.Services;

public class AdminApiService : IAdminApiService
{
    private readonly HttpClient _http;
    private readonly AdminSession _session;

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public AdminApiService(HttpClient http, AdminSession session)
    {
        _http = http;
        _session = session;
    }

    public static async Task EnsureSuccessAsync(
        HttpResponseMessage response,
        string action = "")
    {
        if (response.IsSuccessStatusCode)
            return;

        var body = await response.Content.ReadAsStringAsync();
        string? message = null;

        try
        {
            using var json = JsonDocument.Parse(body);
            var value = json.RootElement;

            if (value.ValueKind == JsonValueKind.String)
            {
                message = value.GetString();
            }
            else if (value.ValueKind == JsonValueKind.Object)
            {
                if (value.TryGetProperty("message", out var error))
                    message = error.GetString();
                else if (value.TryGetProperty("title", out var title))
                    message = title.GetString();
            }
        }
        catch (JsonException)
        {
            if (response.Content.Headers.ContentType?.MediaType
                == "text/plain")
            {
                message = body;
            }
        }

        throw new HttpRequestException(
            message ?? L.T("The server could not complete the request."),
            null,
            response.StatusCode);
    }

    private async Task<T?> GetAsync<T>(string url, string action)
    {
        // Pages may open before the authentication teammate connects a user.
        // Return an empty state instead of attempting an API request.
        if (!_session.IsConnected)
            return default;

        using var response = await _http.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.NotFound
            && typeof(T) == typeof(SchoolDetails))
        {
            return default;
        }

        await EnsureSuccessAsync(response, action);

        return await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
    }

    public Task<AdminDashboard?> GetDashboardAsync() =>
        GetAsync<AdminDashboard>(
            "api/admin/dashboard",
            "Loading the dashboard");

    public async Task<List<User>> GetUsersAsync(string? role = null)
    {
        var url = string.IsNullOrEmpty(role)
            ? "api/admin/users"
            : $"api/admin/users/role/{Uri.EscapeDataString(role)}";

        return await GetAsync<List<User>>(url, "Loading users")
            ?? new List<User>();
    }

    public async Task<List<AdmissionApplication>> GetApplicationsAsync() =>
        await GetAsync<List<AdmissionApplication>>(
            "api/admin/applications",
            "Loading applications")
        ?? new List<AdmissionApplication>();

    public Task<AdmissionApplication?> GetApplicationAsync(int id) =>
        GetAsync<AdmissionApplication>(
            $"api/admin/applications/{id}",
            "Loading the application");

    public async Task UpdateApplicationStatusAsync(int id, string status)
    {
        using var response = await _http.PutAsJsonAsync(
            $"api/admin/applications/{id}/status",
            new { Status = status });

        await EnsureSuccessAsync(
            response,
            "Updating the application status");
    }

    public async Task<List<StaffMember>> GetStaffAsync() =>
        await GetAsync<List<StaffMember>>(
            "api/admin/staff",
            "Loading staff")
        ?? new List<StaffMember>();

    public async Task<List<SchoolClass>> GetClassesAsync() =>
        await GetAsync<List<SchoolClass>>(
            "api/admin/staff/classes",
            "Loading classes")
        ?? new List<SchoolClass>();

    public async Task<List<Subject>> GetSubjectsAsync() =>
        await GetAsync<List<Subject>>(
            "api/admin/staff/subjects",
            "Loading subjects")
        ?? new List<Subject>();

    public Task<SchoolDetails?> GetSchoolDetailsAsync() =>
        GetAsync<SchoolDetails>(
            "api/admin/school/details",
            "Loading school details");

    public async Task<SchoolDetails?> UpdateSchoolDetailsAsync(
        SchoolDetails details)
    {
        using var response = await _http.PutAsJsonAsync(
            "api/admin/school/details",
            details,
            _jsonOptions);

        await EnsureSuccessAsync(response, "Saving school details");

        return await response.Content
            .ReadFromJsonAsync<SchoolDetails>(_jsonOptions);
    }

    public async Task<List<SchoolDate>> GetSchoolDatesAsync() =>
        await GetAsync<List<SchoolDate>>(
            "api/admin/school/dates",
            "Loading school dates")
        ?? new List<SchoolDate>();

    public async Task<SchoolDate?> AddSchoolDateAsync(SchoolDate schoolDate)
    {
        using var response = await _http.PostAsJsonAsync(
            "api/admin/school/dates",
            schoolDate,
            _jsonOptions);

        await EnsureSuccessAsync(response, "Adding the school date");

        return await response.Content
            .ReadFromJsonAsync<SchoolDate>(_jsonOptions);
    }

    public async Task<List<CalendarEvent>> GetCalendarEventsAsync() =>
        await GetAsync<List<CalendarEvent>>(
            "api/admin/school/calendar",
            "Loading calendar events")
        ?? new List<CalendarEvent>();

    public async Task<CalendarEvent?> AddCalendarEventAsync(
        CalendarEvent calendarEvent)
    {
        using var response = await _http.PostAsJsonAsync(
            "api/admin/school/calendar",
            calendarEvent,
            _jsonOptions);

        await EnsureSuccessAsync(response, "Adding the calendar event");

        return await response.Content
            .ReadFromJsonAsync<CalendarEvent>(_jsonOptions);
    }

    public async Task<List<SchoolDocument>> GetDocumentsAsync() =>
        await GetAsync<List<SchoolDocument>>(
            "api/admin/documents",
            "Loading documents")
        ?? new List<SchoolDocument>();

    public async Task<User?> AddUserAsync(User user)
    {
        using var response = await _http.PostAsJsonAsync(
            "api/admin/users",
            user,
            _jsonOptions);

        await EnsureSuccessAsync(response, "Adding the user");

        return await response.Content
            .ReadFromJsonAsync<User>(_jsonOptions);
    }
}