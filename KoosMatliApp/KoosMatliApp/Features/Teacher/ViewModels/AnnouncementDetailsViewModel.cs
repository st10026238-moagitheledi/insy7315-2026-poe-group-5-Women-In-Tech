using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using KoosMatliApp.Features.Teacher.Models;
using KoosMatliApp.Features.Teacher.Services;

namespace KoosMatliApp.Features.Teacher.ViewModels;

public class AnnouncementDetailsViewModel : ObservableObject
{
    private ClassAnnouncementItem? _announcement;


    public ObservableCollection<string> AudienceOptions { get; } =
        new()
        {
            "All Classes",
            "Specific Class"
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


    private string _message = string.Empty;

    public string Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }


    private string _audience = "All Classes";

    public string Audience
    {
        get => _audience;

        set
        {
            if (SetProperty(ref _audience, value))
            {
                OnPropertyChanged(nameof(IsSpecificClass));
            }
        }
    }


    public bool IsSpecificClass =>
        Audience == "Specific Class";


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


    public void LoadAnnouncement(
        ClassAnnouncementItem announcement)
    {
        _announcement =
            announcement;


        Title =
            announcement.Title;

        Message =
            announcement.Message;

        Grade =
            string.IsNullOrWhiteSpace(
                announcement.Grade)
            ? "Grade 5A"
            : announcement.Grade;

        Subject =
            string.IsNullOrWhiteSpace(
                announcement.Subject)
            ? "Mathematics"
            : announcement.Subject;


        Audience =
            announcement.Audience == "All Classes"
                ? "All Classes"
                : "Specific Class";

        EventDate =
            announcement.EventDate;
    }


    public void SaveChanges()
    {
        if (_announcement == null)
        {
            return;
        }


        AnnouncementService.UpdateAnnouncement(
            _announcement,
            Title.Trim(),
            Message.Trim(),
            IsSpecificClass
                ? Grade
                : "All Classes",
            IsSpecificClass
                ? Grade
                : string.Empty,
            IsSpecificClass
                ? Subject
                : string.Empty,
            EventDate);
    }


    public void DeleteAnnouncement()
    {
        if (_announcement == null)
        {
            return;
        }


        AnnouncementService.DeleteAnnouncement(
            _announcement);
    }

    private DateTime _eventDate =
    DateTime.Today;

    public DateTime EventDate
    {
        get => _eventDate;
        set => SetProperty(
            ref _eventDate,
            value);
    }
}
