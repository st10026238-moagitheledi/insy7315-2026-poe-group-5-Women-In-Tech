using System.Collections.ObjectModel;
using KoosMatliApp.Features.Teacher.Models;

namespace KoosMatliApp.Features.Teacher.Services;

public static class AnnouncementService
{
    private static readonly ObservableCollection<ClassAnnouncementItem>
        _announcements =
        new()
        {
            new ClassAnnouncementItem
            {
                Title = "Reminder: Maths Test",
                Message = "Term 2 Mathematics test.",
                Audience = "Grade 5A",
                Grade = "Grade 5A",
                Subject = "Mathematics",
                EventDate = new DateTime(2026, 11, 5)
            },

            new ClassAnnouncementItem
            {
                Title = "Sports Day",
                Message = "Sports Day will take place at the school.",
                Audience = "All Classes",
                Grade = string.Empty,
                Subject = string.Empty,
                EventDate = new DateTime(2026, 11, 12)
            },

            new ClassAnnouncementItem
            {
                Title = "Homework Reminder",
                Message = "Please remind learners to complete their homework before Friday.",
                Audience = "Grade 5A",
                Grade = "Grade 5A",
                Subject = "Mathematics",
                EventDate = new DateTime(2026, 11, 6)
            }
        };


    public static IEnumerable<ClassAnnouncementItem>
        GetAllAnnouncements()
    {
        return _announcements
            .OrderBy(announcement => announcement.EventDate);
    }


    public static IEnumerable<ClassAnnouncementItem>
        GetAnnouncementsForClass(
            string grade,
            string subject)
    {
        return _announcements
            .Where(
                announcement =>
                    announcement.Audience == "All Classes" ||
                    (
                        announcement.Grade == grade &&
                        announcement.Subject == subject
                    ))
            .OrderBy(
                announcement =>
                    announcement.EventDate);
    }


    public static void AddAnnouncement(
        ClassAnnouncementItem announcement)
    {
        _announcements.Add(
            announcement);
    }


    public static void UpdateAnnouncement(
        ClassAnnouncementItem announcement,
        string title,
        string message,
        string audience,
        string grade,
        string subject,
        DateTime eventDate)
    {
        announcement.Title = title;

        announcement.Message = message;

        announcement.Audience = audience;

        announcement.Grade = grade;

        announcement.Subject = subject;

        announcement.EventDate = eventDate;
    }


    public static void DeleteAnnouncement(
        ClassAnnouncementItem announcement)
    {
        _announcements.Remove(
            announcement);
    }
}