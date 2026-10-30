using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using KoosMatliApp.Features.Teacher.Models;
using KoosMatliApp.Features.Teacher.Services;

namespace KoosMatliApp.Features.Teacher.ViewModels;

public class ScheduleViewModel : ObservableObject
{
    public ObservableCollection<TeacherScheduleItem>
        TodaySchedule
    { get; } =
        new();


    public ObservableCollection<TeacherScheduleItem>
        UpcomingSchedule
    { get; } =
        new();


    public string TodayDate =>
        DateTime.Today.ToString(
            "dddd, dd MMMM yyyy");


    public void RefreshSchedule()
    {
        TodaySchedule.Clear();

        UpcomingSchedule.Clear();


        var todayItems =
            ScheduleService.GetTodaySchedule();


        foreach (var item in todayItems)
        {
            TodaySchedule.Add(
                item);
        }


        var upcomingItems =
            ScheduleService.GetUpcomingSchedule();


        foreach (var item in upcomingItems)
        {
            UpcomingSchedule.Add(
                item);
        }
    }
}
