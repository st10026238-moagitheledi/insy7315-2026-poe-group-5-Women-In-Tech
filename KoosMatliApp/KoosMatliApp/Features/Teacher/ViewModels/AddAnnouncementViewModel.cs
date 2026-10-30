using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using KoosMatliApp.Features.Teacher.Models;
using KoosMatliApp.Features.Teacher.Services;

namespace KoosMatliApp.Features.Teacher.ViewModels;

public class AddAnnouncementViewModel : ObservableObject
{
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


    public void SaveAnnouncement()
    {
        var announcement =
            new ClassAnnouncementItem
            {
                Title = Title.Trim(),

                Message = Message.Trim(),

                Audience =
                    IsSpecificClass
                        ? Grade
                        : "All Classes",

                Grade =
                    IsSpecificClass
                        ? Grade
                        : string.Empty,

                Subject =
                    IsSpecificClass
                        ? Subject
                        : string.Empty,

                EventDate = EventDate
            };


        AnnouncementService.AddAnnouncement(
            announcement);
    }

    private DateTime _eventDate =
        DateTime.Today.AddDays(1);
    
    public DateTime EventDate
    {
        get => _eventDate;
        set => SetProperty(
            ref _eventDate,
            value);
    }
}
