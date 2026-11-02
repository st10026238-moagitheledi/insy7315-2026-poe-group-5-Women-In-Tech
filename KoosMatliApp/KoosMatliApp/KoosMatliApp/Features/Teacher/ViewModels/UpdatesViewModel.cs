using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using KoosMatliApp.Features.Teacher.Models;
using KoosMatliApp.Features.Teacher.Services;

namespace KoosMatliApp.Features.Teacher.ViewModels;

public class UpdatesViewModel : ObservableObject
{
    private readonly TeacherApiService _teacherApiService;


    public ObservableCollection<ClassAnnouncementItem>
        Announcements
    { get; } =
        new();


    private bool _isLoading;

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(
            ref _isLoading,
            value);
    } // (Microsoft, 2026)


    private string _errorMessage =
        string.Empty;

    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(
            ref _errorMessage,
            value);
    }


    public UpdatesViewModel(
        TeacherApiService teacherApiService)
    {
        _teacherApiService =
            teacherApiService;
    }


    public async Task<bool> RefreshAnnouncementsAsync()
    {
        if (IsLoading)
        {
            return false;
        } // (Microsoft, 2026)


        try
        {
            IsLoading = true;

            ErrorMessage =
                string.Empty;


            var response =
                await _teacherApiService
                    .GetAnnouncementsAsync();


            if (response == null ||
                !response.Success)
            {
                ErrorMessage =
                    "Unable to load announcements.";

                return false;
            }


            Announcements.Clear();


            foreach (var apiAnnouncement
                in response.Announcements)
            {
                DateTime eventDate =
                    DateTime.Today; // (Microsoft, 2026)


                if (!string.IsNullOrWhiteSpace(
                        apiAnnouncement.EventDate) &&
                    DateTime.TryParse(
                        apiAnnouncement.EventDate,
                        out var parsedEventDate))
                {
                    eventDate =
                        parsedEventDate;
                }
                else if (!string.IsNullOrWhiteSpace(
                             apiAnnouncement.CreatedAt) &&
                         DateTime.TryParse(
                             apiAnnouncement.CreatedAt,
                             out var parsedCreatedDate))
                {
                    eventDate =
                        parsedCreatedDate;
                }


                var announcement =
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
                    };


                Announcements.Add(
                    announcement);
            }


            return true;
        }
        catch (Exception ex)
        {
            ErrorMessage =
                ex.Message;

            return false;
        }
        finally
        {
            IsLoading =
                false; // (Microsoft, 2026)
        }
    }
}


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].

