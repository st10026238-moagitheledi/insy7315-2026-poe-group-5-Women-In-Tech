using System.Collections.ObjectModel;
using KoosMatliApp.Features.Teacher.Models;

namespace KoosMatliApp.Features.Teacher.Services;

public static class ScheduleService
{
    private static readonly ObservableCollection<TeacherScheduleItem>
        _schedule =
        new()
        {
            new TeacherScheduleItem
            {
                Title = "Mathematics",
                Description = "Grade 5A Mathematics lesson.",
                EventType = "Class",
                Date = DateTime.Today,
                StartTime = new TimeSpan(8, 0, 0),
                EndTime = new TimeSpan(8, 45, 0),
                Grade = "Grade 5A",
                Subject = "Mathematics",
                Location = "Room 12"
            },

            new TeacherScheduleItem
            {
                Title = "Life Skills",
                Description = "Grade 4B Life Skills lesson.",
                EventType = "Class",
                Date = DateTime.Today,
                StartTime = new TimeSpan(9, 0, 0),
                EndTime = new TimeSpan(9, 45, 0),
                Grade = "Grade 4B",
                Subject = "Life Skills",
                Location = "Room 8"
            },

            new TeacherScheduleItem
            {
                Title = "Natural Science",
                Description = "Grade 6A Natural Science lesson.",
                EventType = "Class",
                Date = DateTime.Today,
                StartTime = new TimeSpan(10, 0, 0),
                EndTime = new TimeSpan(10, 45, 0),
                Grade = "Grade 6A",
                Subject = "Natural Science",
                Location = "Science Lab"
            },

            new TeacherScheduleItem
            {
                Title = "Staff Meeting",
                Description = "Weekly staff meeting.",
                EventType = "Meeting",
                Date = DateTime.Today.AddDays(1),
                StartTime = new TimeSpan(14, 30, 0),
                EndTime = new TimeSpan(15, 30, 0),
                Location = "Staff Room"
            },

            new TeacherScheduleItem
            {
                Title = "Interhouse Sports",
                Description = "School interhouse sports programme.",
                EventType = "School Event",
                Date = DateTime.Today.AddDays(3),
                StartTime = new TimeSpan(9, 0, 0),
                EndTime = new TimeSpan(14, 0, 0),
                Location = "School Sports Field"
            }
        };


    public static IEnumerable<TeacherScheduleItem>
        GetAllScheduleItems()
    {
        return _schedule
            .OrderBy(item => item.Date)
            .ThenBy(item => item.StartTime);
    }


    public static IEnumerable<TeacherScheduleItem>
        GetTodaySchedule()
    {
        return _schedule
            .Where(
                item =>
                    item.Date.Date ==
                    DateTime.Today)
            .OrderBy(
                item =>
                    item.StartTime);
    }


    public static IEnumerable<TeacherScheduleItem>
        GetUpcomingSchedule()
    {
        return _schedule
            .Where(
                item =>
                    item.Date.Date >
                    DateTime.Today)
            .OrderBy(
                item =>
                    item.Date)
            .ThenBy(
                item =>
                    item.StartTime);
    }


    public static IEnumerable<TeacherScheduleItem>
        GetClassSchedule(
            string grade,
            string subject)
    {
        return _schedule
            .Where(
                item =>
                    item.EventType == "Class" &&
                    item.Grade == grade &&
                    item.Subject == subject)
            .OrderBy(
                item =>
                    item.Date)
            .ThenBy(
                item =>
                    item.StartTime);
    }


    public static void AddScheduleItem(
        TeacherScheduleItem item)
    {
        _schedule.Add(
            item);
    }


    public static void UpdateScheduleItem(
        TeacherScheduleItem item,
        string title,
        string description,
        string eventType,
        DateTime date,
        TimeSpan startTime,
        TimeSpan endTime,
        string grade,
        string subject,
        string location)
    {
        item.Title = title;

        item.Description = description;

        item.EventType = eventType;

        item.Date = date;

        item.StartTime = startTime;

        item.EndTime = endTime;

        item.Grade = grade;

        item.Subject = subject;

        item.Location = location;
    }


    public static void DeleteScheduleItem(
        TeacherScheduleItem item)
    {
        _schedule.Remove(
            item);
    }
}