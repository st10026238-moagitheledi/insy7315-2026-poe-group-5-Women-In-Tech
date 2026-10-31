using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using KoosMatliApp.Features.Teacher.Models;
using KoosMatliApp.Features.Teacher.Services;
using KoosMatliApp.Resources.Localization;

namespace KoosMatliApp.Features.Teacher.ViewModels;

public partial class TeacherHomeViewModel : ObservableObject
{
    private readonly TeacherApiService
        _teacherApiService;


    public ObservableCollection<HomeClassItem>
        TodayClasses
    { get; } =
        new(); // (Microsoft, 2026)


    public ObservableCollection<TeacherScheduleItem>
        UpcomingEvents
    { get; } =
        new();


    private string _announcementTitle =
        string.Empty;

    public string AnnouncementTitle
    {
        get => _announcementTitle;

        set => SetProperty(
            ref _announcementTitle,
            value);
    } // (Microsoft, 2026)


    private string _announcementMessage =
        string.Empty;

    public string AnnouncementMessage
    {
        get => _announcementMessage;

        set => SetProperty(
            ref _announcementMessage,
            value);
    }


    private string _announcementDate =
        string.Empty;

    public string AnnouncementDate
    {
        get => _announcementDate;

        set => SetProperty(
            ref _announcementDate,
            value);
    } // (Microsoft, 2026)


    public TeacherHomeViewModel(
        TeacherApiService teacherApiService)
    {
        _teacherApiService =
            teacherApiService;
    }


    public async Task<bool> RefreshScheduleAsync()
    {
        try
        {
            var response =
                await _teacherApiService
                    .GetScheduleAsync();


            if (response == null ||
                !response.Success)
            {
                TodayClasses.Clear();

                UpcomingEvents.Clear();

                return false;
            } // (Microsoft, 2026)


            var schedule =
                response.Schedule
                    .Select(
                        TeacherScheduleItem.FromApi)
                    .OrderBy(
                        item =>
                            item.Date)
                    .ThenBy(
                        item =>
                            item.StartTime)
                    .ToList();


            TodayClasses.Clear();

            UpcomingEvents.Clear();


            foreach (var item in schedule
                .Where(
                    item =>
                        item.Date.Date ==
                            DateTime.Today &&
                        item.EventType ==
                            "Class"))
            {
                TodayClasses.Add(
                    new HomeClassItem
                    {
                        Grade =
                            item.Grade,

                        Subject =
                            item.Subject,

                        Time =
                            item.TimeDisplay,

                        Status =
                            GetClassStatus(
                                item)
                    });
            } // (Microsoft, 2026)


            var now =
                DateTime.Now;


            var upcoming =
                schedule
                    .Where(
                        item =>
                            item.Date.Date >
                                DateTime.Today ||
                            (
                                item.Date.Date ==
                                    DateTime.Today &&
                                item.StartTime >
                                    now.TimeOfDay
                            ))
                    .Take(3);


            foreach (var item in upcoming)
            {
                UpcomingEvents.Add(
                    item);
            }


            return true;
        }
        catch
        {
            TodayClasses.Clear();

            UpcomingEvents.Clear();

            return false; // (Microsoft, 2026)
        }
    }


    public async Task<bool> RefreshAnnouncementAsync()
    {
        try
        {
            var response =
                await _teacherApiService
                    .GetAnnouncementsAsync();


            if (response == null ||
                !response.Success ||
                response.Announcements.Count == 0)
            {
                SetEmptyAnnouncement();

                return false;
            }


            var announcements =
                response.Announcements
                    .Select(
                        announcement =>
                            new
                            {
                                Announcement =
                                    announcement,

                                EventDate =
                                    GetAnnouncementDate(
                                        announcement)
                            })
                    .ToList(); // (Microsoft, 2026)


            var upcomingAnnouncement =
                announcements
                    .Where(
                        item =>
                            item.EventDate.Date >=
                            DateTime.Today)
                    .OrderBy(
                        item =>
                            item.EventDate)
                    .FirstOrDefault();


            var selectedAnnouncement =
                upcomingAnnouncement ??
                announcements
                    .OrderByDescending(
                        item =>
                            item.EventDate)
                    .FirstOrDefault();


            if (selectedAnnouncement == null)
            {
                SetEmptyAnnouncement();

                return false;
            }


            AnnouncementTitle =
                selectedAnnouncement
                    .Announcement
                    .Title;


            AnnouncementMessage =
                selectedAnnouncement
                    .Announcement
                    .Message; // (Microsoft, 2026)


            var dueEventText =
                LocalizationManager.Instance[
                    "DueEvent"];


            AnnouncementDate =
                $"{dueEventText}: " +
                $"{selectedAnnouncement.EventDate:dd MMM yyyy}";


            return true;
        }
        catch
        {
            SetEmptyAnnouncement();

            return false;
        } // (Microsoft, 2026)
    }


    private static string GetClassStatus(
        TeacherScheduleItem item)
    {
        var now =
            DateTime.Now.TimeOfDay;


        if (now < item.StartTime)
        {
            var minutes =
                Math.Ceiling(
                    (item.StartTime - now)
                        .TotalMinutes);


            if (minutes <= 60)
            {
                return $"In {(int)minutes} min";
            }


            return "Upcoming"; // (Microsoft, 2026)
        }


        if (now <= item.EndTime)
        {
            return "Now";
        }


        return "Completed";
    }


    private static DateTime GetAnnouncementDate(
        TeacherAnnouncementApiModel announcement)
    {
        if (!string.IsNullOrWhiteSpace(
                announcement.EventDate) &&
            DateTime.TryParse(
                announcement.EventDate,
                out var eventDate))
        {
            return eventDate; // (Microsoft, 2026)
        }


        if (!string.IsNullOrWhiteSpace(
                announcement.CreatedAt) &&
            DateTime.TryParse(
                announcement.CreatedAt,
                out var createdDate))
        {
            return createdDate;
        }


        return DateTime.Today;
    }


    private void SetEmptyAnnouncement()
    {
        AnnouncementTitle =
            LocalizationManager.Instance[
                "Announcements"];


        AnnouncementMessage =
            string.Empty;


        AnnouncementDate =
            string.Empty; // (Microsoft, 2026)
    }
}


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].

