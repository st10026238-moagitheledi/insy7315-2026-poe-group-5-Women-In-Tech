using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using KoosMatliApp.Features.Teacher.Models;
using KoosMatliApp.Features.Teacher.Services;

namespace KoosMatliApp.Features.Teacher.ViewModels;

public class UpdatesViewModel : ObservableObject
{
    public ObservableCollection<ClassAnnouncementItem>
        Announcements
    { get; } =
        new();


    public UpdatesViewModel()
    {
        RefreshAnnouncements();
    }


    public void RefreshAnnouncements()
    {
        Announcements.Clear();


        var announcements =
            AnnouncementService
                .GetAllAnnouncements();


        foreach (var announcement in announcements)
        {
            Announcements.Add(
                announcement);
        }
    }
}
