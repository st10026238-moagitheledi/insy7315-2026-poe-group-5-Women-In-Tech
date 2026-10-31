using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using KoosMatliApp.Features.Teacher.Services;

namespace KoosMatliApp.Features.Teacher.ViewModels;

public class TeacherSettingsViewModel : ObservableObject
{
    public ObservableCollection<string>
        LanguageOptions
    { get; } =
        new()
        {
            "English",
            "Setswana",
            "Sepedi"
        }; // (Microsoft, 2026)


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
    } // (Microsoft, 2026)


    private string _language =
        "English";

    public string Language
    {
        get => _language;
        set => SetProperty(
            ref _language,
            value);
    }


    public TeacherSettingsViewModel()
    {
        LoadSettings();
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

        Language =
            settings.Language;
    } // (Microsoft, 2026)


    public void SaveSettings()
    {
        TeacherSettingsService.UpdateNotifications(
        NotificationsEnabled,
        HomeworkNotifications,
        AnnouncementNotifications);


        TeacherSettingsService.UpdateLanguage(
            Language);
    }
}// (Microsoft, 2026)


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].

