using CommunityToolkit.Mvvm.ComponentModel;
using KoosMatliApp.Features.Teacher.Services;

namespace KoosMatliApp.Features.Teacher.ViewModels;

public class NotificationSettingsViewModel : ObservableObject
{
    private bool _notificationsEnabled;

    public bool NotificationsEnabled
    {
        get => _notificationsEnabled;
        set => SetProperty(
            ref _notificationsEnabled,
            value);
    }


    private bool _homeworkNotifications;

    public bool HomeworkNotifications
    {
        get => _homeworkNotifications;
        set => SetProperty(
            ref _homeworkNotifications,
            value);
    }


    private bool _announcementNotifications;

    public bool AnnouncementNotifications
    {
        get => _announcementNotifications;
        set => SetProperty(
            ref _announcementNotifications,
            value);
    }


    public void LoadSettings()
    {
        var settings =
            TeacherSettingsService.GetSettings();


        NotificationsEnabled =
            settings.NotificationsEnabled;

        HomeworkNotifications =
            settings.HomeworkNotifications;

        AnnouncementNotifications =
            settings.AnnouncementNotifications;
    }


    public void SaveSettings()
    {
        TeacherSettingsService.UpdateNotifications(
            NotificationsEnabled,
            HomeworkNotifications,
            AnnouncementNotifications);
    }
}