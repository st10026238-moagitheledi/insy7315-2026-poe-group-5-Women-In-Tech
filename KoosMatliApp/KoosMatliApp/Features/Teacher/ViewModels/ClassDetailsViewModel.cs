using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Storage;
using KoosMatliApp.Features.Teacher.Models;
using KoosMatliApp.Features.Teacher.Services;

namespace KoosMatliApp.Features.Teacher.ViewModels;

public class ClassDetailsViewModel : ObservableObject
{
    private readonly TeacherApiService
        _teacherApiService;


    private string _classId =
        string.Empty;

    public string ClassId
    {
        get => _classId;
        set => SetProperty(
            ref _classId,
            value);
    } // (Microsoft, 2026)


    private string _grade =
        string.Empty;

    public string Grade
    {
        get => _grade;
        set => SetProperty(
            ref _grade,
            value);
    }


    private string _subject =
        string.Empty;

    public string Subject
    {
        get => _subject;
        set => SetProperty(
            ref _subject,
            value);
    } // (Microsoft, 2026)


    private int _learnerCount;

    public int LearnerCount
    {
        get => _learnerCount;
        set => SetProperty(
            ref _learnerCount,
            value);
    }


    private int _presentToday;

    public int PresentToday
    {
        get => _presentToday;
        set => SetProperty(
            ref _presentToday,
            value);
    }


    private int _absentToday;

    public int AbsentToday
    {
        get => _absentToday;
        set => SetProperty(
            ref _absentToday,
            value);
    }


    private string _averageAttendance =
        "0%";

    public string AverageAttendance
    {
        get => _averageAttendance;
        set => SetProperty(
            ref _averageAttendance,
            value);
    } // (Microsoft, 2026)


    private bool _isLoadingLearners;

    public bool IsLoadingLearners
    {
        get => _isLoadingLearners;
        set => SetProperty(
            ref _isLoadingLearners,
            value);
    }


    private string _learnerErrorMessage =
        string.Empty;

    public string LearnerErrorMessage
    {
        get => _learnerErrorMessage;
        set => SetProperty(
            ref _learnerErrorMessage,
            value);
    }


    private string _attendanceErrorMessage =
        string.Empty;

    public string AttendanceErrorMessage
    {
        get => _attendanceErrorMessage;
        set => SetProperty(
            ref _attendanceErrorMessage,
            value);
    } // (Microsoft, 2026)


    private bool _isOverviewSelected =
        true;

    public bool IsOverviewSelected
    {
        get => _isOverviewSelected;
        set => SetProperty(
            ref _isOverviewSelected,
            value);
    }


    private bool _isLearnersSelected;

    public bool IsLearnersSelected
    {
        get => _isLearnersSelected;
        set => SetProperty(
            ref _isLearnersSelected,
            value);
    }


    private bool _isResourcesSelected;

    public bool IsResourcesSelected
    {
        get => _isResourcesSelected;
        set => SetProperty(
            ref _isResourcesSelected,
            value);
    } // (Microsoft, 2026)


    private bool _isAttendanceSelected;

    public bool IsAttendanceSelected
    {
        get => _isAttendanceSelected;
        set => SetProperty(
            ref _isAttendanceSelected,
            value);
    }


    public ObservableCollection<TeacherScheduleItem>
        Timetable
    { get; } =
        new();


    public ObservableCollection<HomeworkItem>
        Homework
    { get; } =
        new();


    public ObservableCollection<ClassAnnouncementItem>
        Announcements
    { get; } =
        new();


    public ObservableCollection<LearnerItem>
        Learners
    { get; } =
        new();


    public ObservableCollection<ClassResourceItem>
        Resources
    { get; } =
        new(); // (Microsoft, 2026)


    public IRelayCommand ShowOverviewCommand { get; }

    public IRelayCommand ShowLearnersCommand { get; }

    public IRelayCommand ShowResourcesCommand { get; }

    public IRelayCommand ShowAttendanceCommand { get; }


    public ClassDetailsViewModel(
        TeacherApiService teacherApiService)
    {
        _teacherApiService =
            teacherApiService;


        ShowOverviewCommand =
            new RelayCommand(
                ShowOverview);

        ShowLearnersCommand =
            new RelayCommand(
                ShowLearners);

        ShowResourcesCommand =
            new RelayCommand(
                ShowResources);

        ShowAttendanceCommand =
            new RelayCommand(
                ShowAttendance);
    }


    public void LoadClass(
        TeacherClassItem selectedClass)
    {
        ClassId =
            selectedClass.Id;

        Grade =
            selectedClass.Grade;

        Subject =
            selectedClass.Subject;

        LearnerCount =
            selectedClass.LearnerCount;
    }


    public async Task<bool> RefreshLearnersAsync()
    {
        if (IsLoadingLearners)
        {
            return false;
        }


        if (string.IsNullOrWhiteSpace(
                ClassId))
        {
            LearnerErrorMessage =
                "The selected class does not have a valid class ID.";

            return false;
        }


        try
        {
            IsLoadingLearners =
                true;

            LearnerErrorMessage =
                string.Empty;


            var response =
                await _teacherApiService
                    .GetClassLearnersAsync(
                        ClassId);


            if (response == null ||
                !response.Success) // (Microsoft, 2026)
            {
                LearnerErrorMessage =
                    "Unable to load learners.";

                return false;
            }


            Learners.Clear();


            foreach (var apiLearner
                in response.Learners)
            {
                var fullName =
                    $"{apiLearner.FirstName} {apiLearner.LastName}"
                        .Trim();


                Learners.Add(
                    new LearnerItem
                    {
                        Id =
                            apiLearner.Id,

                        FullName =
                            fullName,

                        StudentNumber =
                            apiLearner.AdmissionNumber
                    });
            }


            LearnerCount =
                Learners.Count;


            UpdateAttendanceSummary(); // (Microsoft, 2026)


            return true;
        }
        catch (Exception ex)
        {
            LearnerErrorMessage =
                ex.Message;

            return false;
        }
        finally
        {
            IsLoadingLearners =
                false;
        }
    }


    public async Task<bool> RefreshHomeworkAsync()
    {
        try
        {
            var response =
                await _teacherApiService
                    .GetHomeworkAsync(
                        Grade,
                        Subject);


            if (response == null ||
                !response.Success)
            {
                return false;
            }


            Homework.Clear();


            foreach (var apiHomework
                in response.Homework)
            {
                var dueDate =
                    apiHomework.DueDate;


                if (DateTime.TryParse(
                        apiHomework.DueDate,
                        out var parsedDate))
                {
                    dueDate =
                        $"Due {parsedDate:dd MMM yyyy}";
                } // (Microsoft, 2026)


                var attachmentPath =
                    string.IsNullOrWhiteSpace(
                        apiHomework.Id)
                        ? string.Empty
                        : Preferences.Default.Get(
                            $"HomeworkAttachmentPath_{apiHomework.Id}",
                            string.Empty);


                Homework.Add(
                    new HomeworkItem
                    {
                        Id =
                            apiHomework.Id,

                        Title =
                            apiHomework.Title,

                        Description =
                            apiHomework.Description,

                        DueDate =
                            dueDate,

                        Status =
                            apiHomework.Status,

                        Grade =
                            apiHomework.Grade,

                        Subject =
                            apiHomework.Subject,

                        AttachmentName =
                            apiHomework.AttachmentName,

                        AttachmentPath =
                            attachmentPath
                    });
            }


            return true;
        }
        catch
        {
            return false;
        } // (Microsoft, 2026)
    }


    public async Task<bool> RefreshAnnouncementsAsync()
    {
        try
        {
            var response =
                await _teacherApiService
                    .GetAnnouncementsAsync();


            if (response == null ||
                !response.Success)
            {
                return false;
            }


            Announcements.Clear();


            foreach (var apiAnnouncement
                in response.Announcements)
            {
                var isForAllClasses =
                    apiAnnouncement.Audience ==
                        "All Classes";


                var isForCurrentClass =
                    string.Equals(
                        apiAnnouncement.Grade,
                        Grade,
                        StringComparison.OrdinalIgnoreCase)
                    &&
                    string.Equals(
                        apiAnnouncement.Subject,
                        Subject,
                        StringComparison.OrdinalIgnoreCase);


                if (!isForAllClasses &&
                    !isForCurrentClass)
                {
                    continue;
                } // (Microsoft, 2026)


                DateTime eventDate =
                    DateTime.Today;


                if (!string.IsNullOrWhiteSpace(
                        apiAnnouncement.EventDate) &&
                    DateTime.TryParse(
                        apiAnnouncement.EventDate,
                        out var parsedEventDate))
                {
                    eventDate =
                        parsedEventDate;
                }


                Announcements.Add(
                    new ClassAnnouncementItem
                    {
                        Id =
                            apiAnnouncement.Id,

                        Title =
                            apiAnnouncement.Title,

                        Message =
                            apiAnnouncement.Message,

                        Audience =
                            apiAnnouncement.Audience,

                        Grade =
                            apiAnnouncement.Grade,

                        Subject =
                            apiAnnouncement.Subject,

                        EventDate =
                            eventDate // (Microsoft, 2026)
                    });
            }


            return true;
        }
        catch
        {
            return false;
        }
    }


    public async Task<bool> RefreshTimetableAsync()
    {
        try
        {
            var response =
                await _teacherApiService
                    .GetScheduleAsync();


            if (response == null ||
                !response.Success)
            {
                return false;
            } // (Microsoft, 2026)


            Timetable.Clear();


            var items =
                response.Schedule
                    .Select(
                        TeacherScheduleItem.FromApi)
                    .Where(
                        item =>
                            item.EventType ==
                                "Class" &&
                            string.Equals(
                                item.Grade,
                                Grade,
                                StringComparison.OrdinalIgnoreCase) &&
                            string.Equals(
                                item.Subject,
                                Subject,
                                StringComparison.OrdinalIgnoreCase))
                    .OrderBy(
                        item =>
                            item.Date)
                    .ThenBy(
                        item =>
                            item.StartTime);


            foreach (var item in items)
            {
                Timetable.Add(
                    item);
            }


            return true;
        }
        catch
        {
            return false; // (Microsoft, 2026)
        }
    }


    public async Task<bool> RefreshAttendanceAsync()
    {
        if (string.IsNullOrWhiteSpace(
                ClassId))
        {
            return false;
        }


        try
        {
            AttendanceErrorMessage =
                string.Empty;


            foreach (var learner in Learners)
            {
                learner.AttendanceStatus =
                    string.Empty;
            }


            var response =
                await _teacherApiService
                    .GetAttendanceAsync(
                        ClassId,
                        DateTime.Today);


            if (response == null ||
                !response.Success)
            {
                AttendanceErrorMessage =
                    "Unable to load attendance.";

                return false; // (Microsoft, 2026)
            }


            foreach (var record
                in response.Records)
            {
                var learner =
                    Learners.FirstOrDefault(
                        item =>
                            item.Id ==
                            record.LearnerId);


                if (learner != null)
                {
                    learner.AttendanceStatus =
                        record.Status;
                }
            }


            UpdateAttendanceSummary();


            return true;
        }
        catch (Exception ex)
        {
            AttendanceErrorMessage =
                ex.Message;

            return false; // (Microsoft, 2026)
        }
    }


    public async Task<bool> SaveAttendanceAsync()
    {
        try
        {
            AttendanceErrorMessage =
                string.Empty;


            var response =
                await _teacherApiService
                    .SaveAttendanceAsync(
                        ClassId,
                        Grade,
                        Subject,
                        DateTime.Today,
                        Learners);


            if (response == null ||
                !response.Success)
            {
                AttendanceErrorMessage =
                    response?.Message ??
                    "Unable to save attendance."; // (Microsoft, 2026)

                return false;
            }


            UpdateAttendanceSummary();


            return true;
        }
        catch (Exception ex)
        {
            AttendanceErrorMessage =
                ex.Message;

            return false;
        }
    }


    public void UpdateAttendanceSummary()
    {
        PresentToday =
            Learners.Count(
                learner =>
                    learner.AttendanceStatus ==
                        "Present");


        AbsentToday =
            Learners.Count(
                learner =>
                    learner.AttendanceStatus ==
                        "Absent"); // (Microsoft, 2026)


        var marked =
            PresentToday +
            AbsentToday;


        AverageAttendance =
            marked == 0
                ? "0%"
                : $"{Math.Round(
                    PresentToday * 100.0 /
                    marked)}%";
    }


    private void ResetTabs()
    {
        IsOverviewSelected =
            false;

        IsLearnersSelected =
            false;

        IsResourcesSelected =
            false;

        IsAttendanceSelected = // (Microsoft, 2026)
            false;
    }


    private void ShowOverview()
    {
        ResetTabs();

        IsOverviewSelected =
            true;
    }


    private void ShowLearners()
    {
        ResetTabs();

        IsLearnersSelected =
            true; // (Microsoft, 2026)
    }


    private void ShowResources()
    {
        ResetTabs();

        IsResourcesSelected =
            true;
    }


    private void ShowAttendance()
    {
        ResetTabs();

        IsAttendanceSelected =
            true; // (Microsoft, 2026)
    }
}


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].

