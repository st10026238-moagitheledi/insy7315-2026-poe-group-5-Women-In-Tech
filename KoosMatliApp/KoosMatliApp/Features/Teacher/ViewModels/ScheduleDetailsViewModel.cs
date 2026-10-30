using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using KoosMatliApp.Features.Teacher.Models;
using KoosMatliApp.Features.Teacher.Services;

namespace KoosMatliApp.Features.Teacher.ViewModels;

public class ScheduleDetailsViewModel : ObservableObject
{
    private TeacherScheduleItem? _scheduleItem;


    public ObservableCollection<string> EventTypeOptions { get; } =
        new()
        {
            "Class",
            "Meeting",
            "School Event",
            "Other"
        };


    public ObservableCollection<string> GradeOptions { get; } =
        new()
        {
            "Grade 2B",
            "Grade 3C",
            "Grade 4B",
            "Grade 5A",
            "Grade 6A",
            "Grade 7A"
        };


    public ObservableCollection<string> SubjectOptions { get; } =
        new()
        {
            "Mathematics",
            "Life Skills",
            "Natural Science",
            "English Home Language"
        };


    private string _title = string.Empty;

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }


    private string _description = string.Empty;

    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }


    private string _eventType = "Class";

    public string EventType
    {
        get => _eventType;

        set
        {
            if (SetProperty(ref _eventType, value))
            {
                OnPropertyChanged(nameof(IsClass));
            }
        }
    }


    public bool IsClass =>
        EventType == "Class";


    private DateTime _date =
        DateTime.Today;

    public DateTime Date
    {
        get => _date;
        set => SetProperty(ref _date, value);
    }


    private TimeSpan _startTime;

    public TimeSpan StartTime
    {
        get => _startTime;
        set => SetProperty(ref _startTime, value);
    }


    private TimeSpan _endTime;

    public TimeSpan EndTime
    {
        get => _endTime;
        set => SetProperty(ref _endTime, value);
    }


    private string _grade = "Grade 5A";

    public string Grade
    {
        get => _grade;
        set => SetProperty(ref _grade, value);
    }


    private string _subject = "Mathematics";

    public string Subject
    {
        get => _subject;
        set => SetProperty(ref _subject, value);
    }


    private string _location = string.Empty;

    public string Location
    {
        get => _location;
        set => SetProperty(ref _location, value);
    }


    public void LoadScheduleItem(
        TeacherScheduleItem item)
    {
        _scheduleItem =
            item;

        Title =
            item.Title;

        Description =
            item.Description;

        EventType =
            item.EventType;

        Date =
            item.Date;

        StartTime =
            item.StartTime;

        EndTime =
            item.EndTime;

        Grade =
            string.IsNullOrWhiteSpace(item.Grade)
                ? "Grade 5A"
                : item.Grade;

        Subject =
            string.IsNullOrWhiteSpace(item.Subject)
                ? "Mathematics"
                : item.Subject;

        Location =
            item.Location;
    }


    public void SaveChanges()
    {
        if (_scheduleItem == null)
        {
            return;
        }


        ScheduleService.UpdateScheduleItem(
            _scheduleItem,
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
    }


    public void DeleteScheduleItem()
    {
        if (_scheduleItem == null)
        {
            return;
        }


        ScheduleService.DeleteScheduleItem(
            _scheduleItem);
    }
}
