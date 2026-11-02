using System.Text.Json;

namespace KoosMatliApp.Features.Teacher.Models;


public class ApiLoginResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } =
        string.Empty;

    public string IdToken { get; set; } =
        string.Empty;

    public string RefreshToken { get; set; } =
        string.Empty;

    public string ExpiresIn { get; set; } =
        string.Empty;

    public ApiUser? User { get; set; } // (Microsoft, 2026)
}


public class ApiUser
{
    public string Uid { get; set; } =
        string.Empty;

    public string Email { get; set; } =
        string.Empty;

    public string DisplayName { get; set; } =
        string.Empty;

    public string Role { get; set; } =
        string.Empty;

    public bool IsActive { get; set; }
}


public class TeacherProfileResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } =
        string.Empty;

    public TeacherProfileApiModel? Profile
    {
        get;
        set;
    }
}


public class TeacherProfileApiModel
{
    public string Uid { get; set; } =
        string.Empty;

    public string DisplayName { get; set; } =
        string.Empty;

    public string Email { get; set; } =
        string.Empty;

    public string PhoneNumber { get; set; } =
        string.Empty;

    public string StaffId { get; set; } =
        string.Empty;

    public string Role { get; set; } = // (Microsoft, 2026)
        string.Empty;

    public string PhotoUrl { get; set; } =
        string.Empty;

    public List<JsonElement> AssignedClasses
    {
        get;
        set;
    } = new();
}


public class TeacherClassesResponse
{
    public bool Success { get; set; }

    public List<JsonElement> AssignedClasses
    {
        get;
        set;
    } = new();

    public List<TeacherClassApiModel> MyClasses
    {
        get;
        set;
    } = new();

    public List<TeacherClassApiModel> AllClasses
    {
        get;
        set;
    } = new();  // (Microsoft, 2026)
}


public class TeacherClassApiModel
{
    public string Id { get; set; } =
        string.Empty;

    public string ClassId { get; set; } =
        string.Empty;

    public string ClassName { get; set; } =
        string.Empty;

    public string Name { get; set; } =
        string.Empty;

    public string Code { get; set; } =
        string.Empty;

    public string Grade { get; set; } =
        string.Empty;

    public string Subject { get; set; } =
        string.Empty;

    public string Schedule { get; set; } =
        string.Empty;

    public int? LearnerCount { get; set; }

    public bool? IsActive { get; set; } // (Microsoft, 2026)
}


public class TeacherLearnersResponse
{
    public bool Success { get; set; }

    public List<TeacherLearnerApiModel> Learners
    {
        get;
        set;
    } = new();
}


public class TeacherClassLearnersResponse
{
    public bool Success { get; set; }

    public TeacherClassApiModel? Class
    {
        get;
        set;
    }

    public List<TeacherLearnerApiModel> Learners
    {
        get;
        set;
    } = new();
}


public class TeacherLearnerApiModel
{
    public string Id { get; set; } =
        string.Empty;

    public string FirstName { get; set; } =
        string.Empty;

    public string LastName { get; set; } =
        string.Empty;

    public string Grade { get; set; } =
        string.Empty;

    public string ClassName { get; set; } =  // (Microsoft, 2026)
        string.Empty;

    public string AdmissionNumber { get; set; } =
        string.Empty;

    public bool IsActive { get; set; }
}


public class TeacherLearnerMutationResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } =
        string.Empty;

    public TeacherLearnerApiModel? Learner
    {
        get;
        set;
    }
}


public class TeacherAnnouncementsResponse
{
    public bool Success { get; set; }

    public List<TeacherAnnouncementApiModel>
        Announcements
    {
        get;
        set;
    } = new();
}


public class TeacherAnnouncementApiModel
{
    public string Id { get; set; } =
        string.Empty;

    public string Title { get; set; } =
        string.Empty;

    public string Message { get; set; } =  // (Microsoft, 2026)
        string.Empty;

    public string Category { get; set; } =
        string.Empty;

    public string Audience { get; set; } =
        string.Empty;

    public string Grade { get; set; } =
        string.Empty;

    public string Subject { get; set; } =
        string.Empty;

    public string EventDate { get; set; } =
        string.Empty;

    public string Status { get; set; } =
        string.Empty;

    public string CreatedAt { get; set; } =
        string.Empty;

    public string AuthorName { get; set; } =
        string.Empty;

    public bool IsPublished { get; set; }
}


public class TeacherAnnouncementMutationResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } =
        string.Empty;

    public TeacherAnnouncementApiModel?
        Announcement
    {
        get;
        set;
    }
}


public class TeacherHomeworkResponse
{
    public bool Success { get; set; }

    public List<TeacherHomeworkApiModel> Homework
    {
        get;
        set;
    } = new();  // (Microsoft, 2026)
}


public class TeacherHomeworkMutationResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } =
        string.Empty;

    public TeacherHomeworkApiModel? Homework
    {
        get;
        set;
    }
}


public class TeacherHomeworkApiModel
{
    public string Id { get; set; } =
        string.Empty;

    public string Title { get; set; } =
        string.Empty;

    public string Description { get; set; } =
        string.Empty;

    public string DueDate { get; set; } =
        string.Empty;

    public string Status { get; set; } =
        string.Empty;

    public string Grade { get; set; } =
        string.Empty;

    public string Subject { get; set; } =
        string.Empty;

    public string AttachmentName { get; set; } =
        string.Empty;

    public string CreatedAt { get; set; } =
        string.Empty;

    public string CreatedBy { get; set; } =
        string.Empty;
}


public class TeacherScheduleResponse
{
    public bool Success { get; set; }

    public List<TeacherScheduleApiModel> Schedule
    {
        get;
        set;
    } = new(); // (Microsoft, 2026)
}


public class TeacherScheduleMutationResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } =
        string.Empty;

    public TeacherScheduleApiModel? ScheduleItem
    {
        get;
        set;
    }
}


public class TeacherScheduleApiModel
{
    public string Id { get; set; } =
        string.Empty;

    public string Title { get; set; } =
        string.Empty;

    public string Description { get; set; } =
        string.Empty;

    public string EventType { get; set; } =
        string.Empty;

    public string Date { get; set; } =
        string.Empty;

    public string StartTime { get; set; } = // (Microsoft, 2026)
        string.Empty;

    public string EndTime { get; set; } =
        string.Empty;

    public string Grade { get; set; } =
        string.Empty;

    public string Subject { get; set; } =
        string.Empty;

    public string Location { get; set; } =
        string.Empty;

    public bool IsActive { get; set; }

    public string CreatedAt { get; set; } =
        string.Empty;

    public string CreatedBy { get; set; } =  // (Microsoft, 2026)
        string.Empty;
}


public class TeacherAttendanceResponse
{
    public bool Success { get; set; }

    public List<TeacherAttendanceRecordApiModel> Records
    {
        get;
        set;
    } = new();
} // (Microsoft, 2026)


public class TeacherAttendanceRecordApiModel
{
    public string LearnerId { get; set; } =
        string.Empty;

    public string Status { get; set; } =
        string.Empty;
}


public class ApiActionResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } =
        string.Empty; // (Microsoft, 2026)
}


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].

