using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using KoosMatliApp.Features.Teacher.Models;
using KoosMatliApp.Features.Teacher.Services;
using KoosMatliApp.Resources.Localization;

namespace KoosMatliApp.Features.Teacher.ViewModels;

public partial class TeacherHomeViewModel : ObservableObject
{
    public ObservableCollection<HomeClassItem>
        TodayClasses
    { get; }


    public ObservableCollection<TeacherScheduleItem>
        UpcomingEvents
    { get; }


    private string _announcementTitle =
        string.Empty;

    public string AnnouncementTitle
    {
        get => _announcementTitle;

        set => SetProperty(
            ref _announcementTitle,
            value);
    }


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
    }


    public TeacherHomeViewModel()
    {
        TodayClasses =
            new ObservableCollection<HomeClassItem>
            {
                new HomeClassItem
                {
                    Grade = "Grade 5A",
                    Subject = "Mathematics",
                    Time = "08:00 - 08:45",
                    Status = "In 15 min"
                },

                new HomeClassItem
                {
                    Grade = "Grade 5B",
                    Subject = "Life Skills",
                    Time = "09:00 - 09:45",
                    Status = "Upcoming"
                }
            };


        UpcomingEvents =
            new ObservableCollection<TeacherScheduleItem>();


        RefreshUpcomingEvents();

        RefreshAnnouncement();
    }


    public void RefreshUpcomingEvents()
    {
        UpcomingEvents.Clear();


        var events =
            ScheduleService
                .GetUpcomingSchedule()
                .Take(3);


        foreach (var item in events)
        {
            UpcomingEvents.Add(
                item);
        }
    }


    public void RefreshAnnouncement()
    {
        var announcement =
            AnnouncementService
                .GetLatestAnnouncement();


        if (announcement == null)
        {
            AnnouncementTitle =
                LocalizationManager.Instance[
                    "Announcements"];

            AnnouncementMessage =
                string.Empty;

            AnnouncementDate =
                string.Empty;

            return;
        }


        AnnouncementTitle =
            announcement.Title;

        AnnouncementMessage =
            announcement.Message;


        var dueEventText =
            LocalizationManager.Instance[
                "DueEvent"];


        AnnouncementDate =
            $"{dueEventText}: " +
            $"{announcement.EventDate:dd MMM yyyy}";
    }
}