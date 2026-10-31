using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using KoosMatliApp.Features.Teacher.Services;

namespace KoosMatliApp.Features.Teacher.ViewModels;

public class AddScheduleViewModel : ObservableObject
{
    private readonly TeacherApiService
        _teacherApiService;


    public AddScheduleViewModel(
        TeacherApiService teacherApiService)
    {
        _teacherApiService =
            teacherApiService;
    } // (Microsoft, 2026)


    public ObservableCollection<string>
        EventTypeOptions
    { get; } =
        new()
        {
            "Class",
            "Meeting",
            "School Event",
            "Other"
        };


    public ObservableCollection<string>
        GradeOptions
    { get; } =
        new()
        {
            "Grade 2B",
            "Grade 3C",
            "Grade 4B",
            "Grade 5A",
            "Grade 6A",
            "Grade 7A"
        };


    public ObservableCollection<string>
        SubjectOptions
    { get; } =
        new()
        {
            "Mathematics",
            "Life Skills",
            "Natural Science",
            "English Home Language"
        }; // (Microsoft, 2026)


    private string _title =
        string.Empty;

    public string Title
    {
        get => _title;
        set => SetProperty(
            ref _title,
            value);
    }


    private string _description =
        string.Empty;

    public string Description
    {
        get => _description;
        set => SetProperty(
            ref _description,
            value);
    }


    private string _eventType =
        "Class";

    public string EventType
    {
        get => _eventType;

        set
        {
            if (SetProperty(
                    ref _eventType,
                    value))
            {
                OnPropertyChanged(
                    nameof(IsClass));
            }
        } // (Microsoft, 2026)
    }


    public bool IsClass =>
        EventType == "Class";


    private DateTime _date =
        DateTime.Today;

    public DateTime Date
    {
        get => _date;
        set => SetProperty(
            ref _date,
            value);
    }


    private TimeSpan _startTime =
        new(8, 0, 0);

    public TimeSpan StartTime
    {
        get => _startTime;
        set => SetProperty(
            ref _startTime,
            value);
    }


    private TimeSpan _endTime =
        new(8, 45, 0);

    public TimeSpan EndTime
    {
        get => _endTime;
        set => SetProperty(
            ref _endTime,
            value);
    }


    private string _grade =
        "Grade 5A";

    public string Grade
    {
        get => _grade;
        set => SetProperty(
            ref _grade,
            value);
    }


    private string _subject =
        "Mathematics";

    public string Subject
    {
        get => _subject;
        set => SetProperty(
            ref _subject,
            value);
    } // (Microsoft, 2026)


    private string _location =
        string.Empty;

    public string Location
    {
        get => _location;
        set => SetProperty(
            ref _location,
            value);
    }


    public string ErrorMessage { get; private set; } =
        string.Empty;


    public async Task<bool> SaveScheduleItemAsync()
    {
        try
        {
            ErrorMessage =
                string.Empty;


            var response =
                await _teacherApiService
                    .CreateScheduleItemAsync(
                        Title.Trim(),
                        Description.Trim(),
                        EventType,
                        Date,
                        StartTime,
                        EndTime,
                        IsClass
                            ? Grade
                            : string.Empty,
                        IsClass
                            ? Subject
                            : string.Empty,
                        Location.Trim());


            if (response == null ||
                !response.Success)
            {
                ErrorMessage =
                    response?.Message ??
                    "Unable to add schedule item.";

                return false;
            } // (Microsoft, 2026)


            return true;
        }
        catch (Exception ex)
        {
            ErrorMessage =
                ex.Message;

            return false;
        } // (Microsoft, 2026)
    }
}


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].

