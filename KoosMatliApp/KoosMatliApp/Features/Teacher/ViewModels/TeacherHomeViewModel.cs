using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using KoosMatliApp.Features.Teacher.Models;

namespace KoosMatliApp.Features.Teacher.ViewModels;

public partial class TeacherHomeViewModel : ObservableObject
{
    public ObservableCollection<HomeClassItem> TodayClasses { get; }

    public string AnnouncementTitle { get; } = "Important Announcement";

    public string AnnouncementMessage { get; } =
        "Sports Day is on 24 May 2026. Please see the attached programme.";

    public string EventDay { get; } = "24";

    public string EventMonth { get; } = "MAY";

    public string EventTitle { get; } = "Sports Day";

    public string EventSubtitle { get; } = "All Day Event";

    public TeacherHomeViewModel()
    {
        TodayClasses = new ObservableCollection<HomeClassItem>
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
    }
}