using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using KoosMatliApp.Features.Teacher.Services;

namespace KoosMatliApp.Features.Teacher.ViewModels;

public class AddAnnouncementViewModel : ObservableObject
{
    private readonly TeacherApiService _teacherApiService;


    public ObservableCollection<string> AudienceOptions { get; } =
        new()
        {
            "All Classes",
            "Specific Class" // (Microsoft, 2026)
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


    public AddAnnouncementViewModel(
        TeacherApiService teacherApiService)
    {
        _teacherApiService =
            teacherApiService; // (Microsoft, 2026)
    }


    private string _title = string.Empty;

    public string Title
    {
        get => _title;
        set => SetProperty(
            ref _title,
            value);
    }


    private string _message = string.Empty;

    public string Message
    {
        get => _message;
        set => SetProperty(
            ref _message,
            value);
    }


    private string _audience = "All Classes";

    public string Audience
    {
        get => _audience;

        set
        {
            if (SetProperty(
                    ref _audience,
                    value))
            {
                OnPropertyChanged(
                    nameof(IsSpecificClass));
            }
        }
    }


    public bool IsSpecificClass =>
        Audience == "Specific Class";


    private string _grade = "Grade 5A";

    public string Grade
    {
        get => _grade;
        set => SetProperty(
            ref _grade,
            value); // (Microsoft, 2026)
    }


    private string _subject = "Mathematics";

    public string Subject
    {
        get => _subject;
        set => SetProperty(
            ref _subject,
            value);
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


    public string ErrorMessage { get; private set; } =
        string.Empty;


    public async Task<bool>
        SaveAnnouncementAsync()
    {
        try
        {
            ErrorMessage =
                string.Empty;


            var audience =
                IsSpecificClass
                    ? Grade
                    : "All Classes";


            var grade =
                IsSpecificClass
                    ? Grade
                    : string.Empty;


            var subject =
                IsSpecificClass
                    ? Subject // (Microsoft, 2026)
                    : string.Empty;


            var response =
                await _teacherApiService
                    .CreateAnnouncementAsync(
                        Title.Trim(),
                        Message.Trim(),
                        audience,
                        grade,
                        subject,
                        EventDate);


            if (response == null ||
                !response.Success)
            {
                ErrorMessage =
                    response?.Message ??
                    "Unable to publish announcement.";

                return false;
            }


            return true;
        }
        catch (Exception ex)
        {
            ErrorMessage =
                ex.Message;

            return false; // (Microsoft, 2026)
        }
    }
}


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].

