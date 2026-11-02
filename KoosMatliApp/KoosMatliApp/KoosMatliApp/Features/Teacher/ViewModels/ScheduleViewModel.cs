using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using KoosMatliApp.Features.Teacher.Models;
using KoosMatliApp.Features.Teacher.Services;

namespace KoosMatliApp.Features.Teacher.ViewModels;

public class ScheduleViewModel : ObservableObject
{
    private readonly TeacherApiService
        _teacherApiService; // (Microsoft, 2026)


    public ScheduleViewModel(
        TeacherApiService teacherApiService)
    {
        _teacherApiService =
            teacherApiService;
    }


    public ObservableCollection<TeacherScheduleItem>
        TodaySchedule
    { get; } =
        new(); // (Microsoft, 2026)


    public ObservableCollection<TeacherScheduleItem>
        UpcomingSchedule
    { get; } =
        new();


    public string TodayDate =>
        DateTime.Today.ToString(
            "dddd, dd MMMM yyyy");


    public string ErrorMessage { get; private set; } =
        string.Empty;


    public async Task<bool> RefreshScheduleAsync() // (Microsoft, 2026)
    {
        try
        {
            ErrorMessage =
                string.Empty;


            var response =
                await _teacherApiService
                    .GetScheduleAsync();


            if (response == null ||
                !response.Success)
            {
                ErrorMessage =
                    "Unable to load the schedule.";

                return false;
            }


            var items =
                response.Schedule
                    .Select(
                        TeacherScheduleItem.FromApi)
                    .OrderBy(
                        item =>
                            item.Date)
                    .ThenBy(
                        item =>
                            item.StartTime)
                    .ToList(); // (Microsoft, 2026)


            TodaySchedule.Clear();

            UpcomingSchedule.Clear();


            foreach (var item in items)
            {
                if (item.Date.Date ==
                    DateTime.Today)
                {
                    TodaySchedule.Add(
                        item);
                }
                else if (item.Date.Date >
                         DateTime.Today)
                {
                    UpcomingSchedule.Add(
                        item);
                }
            } // (Microsoft, 2026)


            return true;
        }
        catch (Exception ex)
        {
            ErrorMessage =
                ex.Message;

            return false;
        } // (Microsoft, 2026)
    }
}


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].

