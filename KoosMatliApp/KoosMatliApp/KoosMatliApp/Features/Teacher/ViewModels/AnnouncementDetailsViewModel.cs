using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using KoosMatliApp.Features.Teacher.Models;
using KoosMatliApp.Features.Teacher.Services;

namespace KoosMatliApp.Features.Teacher.ViewModels;

public class AnnouncementDetailsViewModel : ObservableObject
{
    private readonly TeacherApiService _teacherApiService;

    private ClassAnnouncementItem? _announcement; // (Microsoft, 2026)


    public AnnouncementDetailsViewModel(
        TeacherApiService teacherApiService)
    {
        _teacherApiService =
            teacherApiService;
    }


    public ObservableCollection<string> AudienceOptions { get; } =
        new()
        {
            "All Classes",
            "Specific Class"
        }; // (Microsoft, 2026)


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
        }; // (Microsoft, 2026)


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
            value);
    } // (Microsoft, 2026)


    private string _subject = "Mathematics";

    public string Subject
    {
        get => _subject;
        set => SetProperty(
            ref _subject,
            value);
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


    public string ErrorMessage { get; private set; } =
        string.Empty;


    public void LoadAnnouncement(
        ClassAnnouncementItem announcement)
    {
        _announcement =
            announcement; // (Microsoft, 2026)


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


    public async Task<bool>
        SaveChangesAsync()
    {
        if (_announcement == null ||
            string.IsNullOrWhiteSpace(
                _announcement.Id))
        {
            ErrorMessage =
                "This announcement does not have a valid ID.";

            return false;
        }


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
                    ? Subject
                    : string.Empty;


            var response =
                await _teacherApiService
                    .UpdateAnnouncementAsync(
                        _announcement.Id,
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
                    "Unable to update announcement."; // (Microsoft, 2026)

                return false;
            }


            return true;
        }
        catch (Exception ex)
        {
            ErrorMessage =
                ex.Message;

            return false;
        }
    }


    public async Task<bool>
        DeleteAnnouncementAsync()
    {
        if (_announcement == null ||
            string.IsNullOrWhiteSpace(
                _announcement.Id))
        {
            ErrorMessage =
                "This announcement does not have a valid ID.";

            return false;
        }


        try
        {
            ErrorMessage =
                string.Empty;


            var response =
                await _teacherApiService
                    .DeleteAnnouncementAsync(
                        _announcement.Id);


            if (response == null ||
                !response.Success)
            {
                ErrorMessage =
                    response?.Message ??
                    "Unable to delete announcement.";

                return false;
            } // (Microsoft, 2026)


            return true;
        }
        catch (Exception ex)
        {
            ErrorMessage =
                ex.Message;

            return false;
        }
    }
}


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].

