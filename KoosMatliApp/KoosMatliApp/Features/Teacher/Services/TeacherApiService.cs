using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using KoosMatliApp.Features.Teacher.Models;

namespace KoosMatliApp.Features.Teacher.Services
{
    public class TeacherApiService
    {
        private readonly HttpClient _httpClient;

        private readonly JsonSerializerOptions _jsonOptions =
            new()
            {
                PropertyNameCaseInsensitive = true
            }; // (Microsoft, 2026)

        private string? _idToken;

        public bool IsAuthenticated =>
            !string.IsNullOrWhiteSpace(_idToken);


        public void SetAuthenticationToken(
            string idToken)
        {
            if (string.IsNullOrWhiteSpace(idToken))
            {
                throw new ArgumentException(
                    "A valid authentication token is required.",
                    nameof(idToken));
            }


            _idToken = idToken;
        }


        public TeacherApiService(
            HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        // Login

        public async Task<ApiLoginResponse> LoginAsync(
            string email,
            string password)
        {
            var loginRequest =
                new
                {
                    email,
                    password
                }; // (Microsoft, 2026)


            var response =
                await _httpClient.PostAsJsonAsync(
                    "api/auth/login",
                    loginRequest);


            var responseText =
                await response.Content.ReadAsStringAsync();


            var result =
                JsonSerializer.Deserialize<ApiLoginResponse>(
                    responseText,
                    _jsonOptions);


            if (result == null)
            {
                throw new InvalidOperationException(
                    "The server returned an invalid response.");
            }


            if (!response.IsSuccessStatusCode ||
                !result.Success)
            {
                throw new InvalidOperationException(
                    string.IsNullOrWhiteSpace(
                        result.Message)
                        ? "Unable to sign in."
                        : result.Message);
            }


            if (string.IsNullOrWhiteSpace(
                    result.IdToken))
            {
                throw new InvalidOperationException(
                    "Authentication token was not returned.");
            }


            _idToken =
                result.IdToken;


            return result;
        }


        // Profile

        public async Task<TeacherProfileResponse?>
            GetProfileAsync()
        {
            return await GetAuthorizedAsync
                <TeacherProfileResponse>(
                    "api/teacher/profile");
        }


        public async Task<TeacherProfileResponse?>
            UpdateProfileAsync(
                string displayName,
                string phoneNumber)
        {
            var body =
                new
                {
                    displayName,
                    phoneNumber
                };


            return await SendAuthorizedAsync
                <TeacherProfileResponse>(
                    HttpMethod.Patch,
                    "api/teacher/profile",
                    body);
        } // (Microsoft, 2026)


        // Classes

        public async Task<TeacherClassesResponse?>
            GetClassesAsync()
        {
            return await GetAuthorizedAsync
                <TeacherClassesResponse>(
                    "api/teacher/classes");
        }


        // Learners

        public async Task<TeacherLearnersResponse?>
            GetLearnersAsync()
        {
            return await GetAuthorizedAsync
                <TeacherLearnersResponse>(
                    "api/teacher/learners");
        }


        public async Task<TeacherClassLearnersResponse?>
            GetClassLearnersAsync(
                string classId)
        {
            if (string.IsNullOrWhiteSpace(
                    classId))
            {
                throw new ArgumentException(
                    "A class ID is required.",
                    nameof(classId));
            }


            var safeClassId =
                Uri.EscapeDataString(
                    classId.Trim());


            return await GetAuthorizedAsync
                <TeacherClassLearnersResponse>(
                    $"api/teacher/classes/{safeClassId}/learners");
        }


        public async Task<TeacherLearnerMutationResponse?>
            CreateLearnerAsync(
                string firstName,
                string lastName,
                string grade,
                string className,
                string admissionNumber)
        {
            var body =
                new
                {
                    firstName,
                    lastName,
                    grade,
                    className,
                    admissionNumber
                };


            return await SendAuthorizedAsync
                <TeacherLearnerMutationResponse>(
                    HttpMethod.Post,
                    "api/teacher/learners",
                    body);
        }


        public async Task<TeacherLearnerMutationResponse?>
            UpdateLearnerAsync(
                string learnerId,
                string firstName,
                string lastName,
                string grade,
                string className,
                string admissionNumber)
        {
            if (string.IsNullOrWhiteSpace(
                    learnerId))
            {
                throw new ArgumentException(
                    "A learner ID is required.",
                    nameof(learnerId));
            } // (Microsoft, 2026)


            var safeLearnerId =
                Uri.EscapeDataString(
                    learnerId.Trim());


            var body =
                new
                {
                    firstName,
                    lastName,
                    grade,
                    className,
                    admissionNumber
                };


            return await SendAuthorizedAsync
                <TeacherLearnerMutationResponse>(
                    HttpMethod.Patch,
                    $"api/teacher/learners/{safeLearnerId}",
                    body);
        }


        public async Task<ApiActionResponse?>
            DeleteLearnerAsync(
                string learnerId)
        {
            if (string.IsNullOrWhiteSpace(
                    learnerId))
            {
                throw new ArgumentException(
                    "A learner ID is required.",
                    nameof(learnerId));
            }


            var safeLearnerId =
                Uri.EscapeDataString(
                    learnerId.Trim());


            return await SendAuthorizedAsync
                <ApiActionResponse>(
                    HttpMethod.Delete,
                    $"api/teacher/learners/{safeLearnerId}");
        }


        // Announcements

        public async Task<TeacherAnnouncementsResponse?>
            GetAnnouncementsAsync()
        {
            return await GetAuthorizedAsync
                <TeacherAnnouncementsResponse>(
                    "api/teacher/announcements");
        }


        public async Task<TeacherAnnouncementMutationResponse?>
            CreateAnnouncementAsync(
                string title,
                string message,
                string audience,
                string grade,
                string subject,
                DateTime eventDate)
        {
            var category =
                string.IsNullOrWhiteSpace(
                    subject)
                    ? "General"
                    : subject;


            var body =
                new
                {
                    title,
                    message,
                    category,
                    audience,
                    grade,
                    subject,

                    eventDate =
                        eventDate.ToString("O")
                };


            return await SendAuthorizedAsync
                <TeacherAnnouncementMutationResponse>(
                    HttpMethod.Post,
                    "api/teacher/announcements",
                    body);
        } // (Microsoft, 2026)


        public async Task<TeacherAnnouncementMutationResponse?>
            UpdateAnnouncementAsync(
                string announcementId,
                string title,
                string message,
                string audience,
                string grade,
                string subject,
                DateTime eventDate)
        {
            if (string.IsNullOrWhiteSpace(
                    announcementId))
            {
                throw new ArgumentException(
                    "An announcement ID is required.",
                    nameof(announcementId));
            }


            var safeAnnouncementId =
                Uri.EscapeDataString(
                    announcementId.Trim());


            var category =
                string.IsNullOrWhiteSpace(
                    subject)
                    ? "General"
                    : subject;


            var body =
                new
                {
                    title,
                    message,
                    category,
                    audience,
                    grade,
                    subject,

                    eventDate =
                        eventDate.ToString("O")
                };


            return await SendAuthorizedAsync
                <TeacherAnnouncementMutationResponse>(
                    HttpMethod.Patch,
                    $"api/teacher/announcements/{safeAnnouncementId}",
                    body);
        } // (Microsoft, 2026)


        public async Task<ApiActionResponse?>
            DeleteAnnouncementAsync(
                string announcementId)
        {
            if (string.IsNullOrWhiteSpace(
                    announcementId))
            {
                throw new ArgumentException(
                    "An announcement ID is required.",
                    nameof(announcementId));
            }


            var safeAnnouncementId =
                Uri.EscapeDataString(
                    announcementId.Trim());


            return await SendAuthorizedAsync
                <ApiActionResponse>(
                    HttpMethod.Delete,
                    $"api/teacher/announcements/{safeAnnouncementId}");
        }


        // Homework

        public async Task<TeacherHomeworkResponse?>
            GetHomeworkAsync(
                string grade,
                string subject)
        {
            var safeGrade =
                Uri.EscapeDataString(
                    grade ?? string.Empty);

            var safeSubject =
                Uri.EscapeDataString(
                    subject ?? string.Empty);


            return await GetAuthorizedAsync
                <TeacherHomeworkResponse>(
                    $"api/teacher/homework?grade={safeGrade}&subject={safeSubject}");
        } // (Microsoft, 2026)


        public async Task<TeacherHomeworkMutationResponse?>
            CreateHomeworkAsync(
                string title,
                string description,
                DateTime dueDate,
                string status,
                string grade,
                string subject,
                string attachmentName)
        {
            var body =
                new
                {
                    title,
                    description,

                    dueDate =
                        dueDate.ToString("yyyy-MM-dd"),

                    status,
                    grade,
                    subject,
                    attachmentName
                };


            return await SendAuthorizedAsync
                <TeacherHomeworkMutationResponse>(
                    HttpMethod.Post,
                    "api/teacher/homework",
                    body);
        }


        // Schedule

        public async Task<TeacherScheduleResponse?>
            GetScheduleAsync()
        {
            return await GetAuthorizedAsync
                <TeacherScheduleResponse>(
                    "api/teacher/schedule");
        }


        public async Task<TeacherScheduleMutationResponse?>
            CreateScheduleItemAsync(
                string title,
                string description,
                string eventType,
                DateTime date,
                TimeSpan startTime,
                TimeSpan endTime,
                string grade,
                string subject,
                string location) // (Microsoft, 2026)
        {
            var body =
                new
                {
                    title,
                    description,
                    eventType,

                    date =
                        date.ToString("yyyy-MM-dd"),

                    startTime =
                        startTime.ToString(
                            @"hh\:mm"),

                    endTime =
                        endTime.ToString(
                            @"hh\:mm"),

                    grade,
                    subject,
                    location
                };


            return await SendAuthorizedAsync
                <TeacherScheduleMutationResponse>(
                    HttpMethod.Post,
                    "api/teacher/schedule",
                    body);
        }


        public async Task<TeacherScheduleMutationResponse?>
            UpdateScheduleItemAsync(
                string scheduleId,
                string title,
                string description,
                string eventType,
                DateTime date,
                TimeSpan startTime,
                TimeSpan endTime,
                string grade,
                string subject,
                string location)
        {
            if (string.IsNullOrWhiteSpace(
                    scheduleId))
            {
                throw new ArgumentException(
                    "A schedule item ID is required.",
                    nameof(scheduleId));
            }


            var safeScheduleId =
                Uri.EscapeDataString(
                    scheduleId.Trim());


            var body =
                new
                {
                    title,
                    description,
                    eventType,

                    date =
                        date.ToString("yyyy-MM-dd"),

                    startTime =
                        startTime.ToString(
                            @"hh\:mm"),

                    endTime =
                        endTime.ToString(
                            @"hh\:mm"),

                    grade,
                    subject,
                    location
                }; // (Microsoft, 2026)


            return await SendAuthorizedAsync
                <TeacherScheduleMutationResponse>(
                    HttpMethod.Patch,
                    $"api/teacher/schedule/{safeScheduleId}",
                    body);
        }


        public async Task<ApiActionResponse?>
            DeleteScheduleItemAsync(
                string scheduleId)
        {
            if (string.IsNullOrWhiteSpace(
                    scheduleId))
            {
                throw new ArgumentException(
                    "A schedule item ID is required.",
                    nameof(scheduleId));
            }


            var safeScheduleId =
                Uri.EscapeDataString(
                    scheduleId.Trim());


            return await SendAuthorizedAsync
                <ApiActionResponse>(
                    HttpMethod.Delete,
                    $"api/teacher/schedule/{safeScheduleId}");
        }


        // Attendance

        public async Task<TeacherAttendanceResponse?>
            GetAttendanceAsync(
                string classId,
                DateTime date)
        {
            if (string.IsNullOrWhiteSpace(
                    classId))
            {
                throw new ArgumentException(
                    "A class ID is required.",
                    nameof(classId));
            }


            var safeClassId =
                Uri.EscapeDataString(
                    classId.Trim());

            var safeDate =
                Uri.EscapeDataString(
                    date.ToString(
                        "yyyy-MM-dd")); // (Microsoft, 2026)


            return await GetAuthorizedAsync
                <TeacherAttendanceResponse>(
                    $"api/teacher/classes/{safeClassId}/attendance?date={safeDate}");
        }


        public async Task<ApiActionResponse?>
            SaveAttendanceAsync(
                string classId,
                string grade,
                string subject,
                DateTime date,
                IEnumerable<LearnerItem> learners)
        {
            if (string.IsNullOrWhiteSpace(
                    classId))
            {
                throw new ArgumentException(
                    "A class ID is required.",
                    nameof(classId));
            }


            var records =
                learners
                    .Where(
                        learner =>
                            !string.IsNullOrWhiteSpace(
                                learner.Id) &&
                            (
                                learner.AttendanceStatus ==
                                    "Present" ||
                                learner.AttendanceStatus ==
                                    "Absent"
                            ))
                    .Select(
                        learner =>
                            new
                            {
                                learnerId =
                                    learner.Id,

                                status =
                                    learner.AttendanceStatus
                            })
                    .ToList();


            if (records.Count == 0)
            {
                throw new InvalidOperationException(
                    "Mark at least one learner as present or absent before saving.");
            }


            var body =
                new
                {
                    classId,
                    grade,
                    subject,

                    date =
                        date.ToString(
                            "yyyy-MM-dd"),

                    records
                };


            return await SendAuthorizedAsync
                <ApiActionResponse>(
                    HttpMethod.Post,
                    "api/teacher/attendance", // (Microsoft, 2026)
                    body);
        }


        // Authorised GET Request

        private async Task<T?>
            GetAuthorizedAsync<T>(
                string endpoint)
        {
            if (string.IsNullOrWhiteSpace(
                    _idToken))
            {
                throw new InvalidOperationException(
                    "You must sign in before accessing teacher data.");
            }


            using var request =
                new HttpRequestMessage(
                    HttpMethod.Get,
                    endpoint);


            request.Headers.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    _idToken);


            using var response =
                await _httpClient.SendAsync(
                    request);


            var responseText =
                await response.Content.ReadAsStringAsync();


            if (!response.IsSuccessStatusCode) // (Microsoft, 2026)
            {
                try
                {
                    var error =
                        JsonSerializer.Deserialize
                            <ApiErrorResponse>(
                                responseText,
                                _jsonOptions);


                    throw new InvalidOperationException(
                        error?.Message ??
                        $"API request failed: {(int)response.StatusCode}");
                }
                catch (JsonException)
                {
                    throw new InvalidOperationException(
                        $"API request failed: {(int)response.StatusCode}");
                }
            }


            return JsonSerializer.Deserialize<T>(
                responseText,
                _jsonOptions);
        }


        // Authorised POST, PATCH and DELETE Request

        private async Task<T?>
            SendAuthorizedAsync<T>(
                HttpMethod method,
                string endpoint,
                object? body = null)
        {
            if (string.IsNullOrWhiteSpace(
                    _idToken))
            {
                throw new InvalidOperationException(
                    "You must sign in before accessing teacher data.");
            }


            using var request =
                new HttpRequestMessage(
                    method,
                    endpoint);


            request.Headers.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    _idToken); // (Microsoft, 2026)


            if (body != null)
            {
                request.Content =
                    JsonContent.Create(
                        body);
            }


            using var response =
                await _httpClient.SendAsync(
                    request);


            var responseText =
                await response.Content.ReadAsStringAsync();


            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    var error =
                        JsonSerializer.Deserialize
                            <ApiErrorResponse>(
                                responseText,
                                _jsonOptions);


                    throw new InvalidOperationException(
                        error?.Message ??
                        $"API request failed: {(int)response.StatusCode}");
                }
                catch (JsonException)
                {
                    throw new InvalidOperationException(
                        $"API request failed: {(int)response.StatusCode}");
                } // (Microsoft, 2026)
            }


            return JsonSerializer.Deserialize<T>(
                responseText,
                _jsonOptions);
        }


        // Logout

        public void ClearSession()
        {
            _idToken = null;
        }


        private class ApiErrorResponse
        {
            public string Message { get; set; } =
                string.Empty;
        } // (Microsoft, 2026)
    }
}


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].

