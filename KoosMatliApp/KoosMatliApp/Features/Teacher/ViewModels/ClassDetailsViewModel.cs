using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using KoosMatliApp.Features.Teacher.Models;

namespace KoosMatliApp.Features.Teacher.ViewModels;

public class ClassDetailsViewModel : ObservableObject
{
    private string _grade = "Grade 5A";

    public string Grade
    {
        get => _grade;
        set => SetProperty(ref _grade, value);
    }


    private string _subject = "Mathematics";

    public string Subject
    {
        get => _subject;
        set => SetProperty(ref _subject, value);
    }


    private int _learnerCount = 32;

    public int LearnerCount
    {
        get => _learnerCount;
        set => SetProperty(ref _learnerCount, value);
    }

    public int PresentToday { get; } = 28;

    public int AbsentToday { get; } = 4;

    public string AverageAttendance { get; } = "95%";


    public ObservableCollection<ClassTimetableItem> Timetable { get; }

    public ObservableCollection<HomeworkItem> Homework { get; }

    public ObservableCollection<ClassAnnouncementItem> Announcements { get; }


    public ClassDetailsViewModel()
    {
        Timetable = new ObservableCollection<ClassTimetableItem>
        {
            new ClassTimetableItem
            {
                Day = "Monday",
                Time = "08:00 - 08:45"
            },

            new ClassTimetableItem
            {
                Day = "Wednesday",
                Time = "08:00 - 08:45"
            },

            new ClassTimetableItem
            {
                Day = "Friday",
                Time = "08:00 - 08:45"
            }
        };


        Homework = new ObservableCollection<HomeworkItem>
        {
            new HomeworkItem
            {
                Title = "Fractions Worksheet",
                DueDate = "Due 24 May 2026",
                Status = "Active"
            },

            new HomeworkItem
            {
                Title = "Multiplication Exercise",
                DueDate = "Due 27 May 2026",
                Status = "Active"
            }
        };


        Announcements = new ObservableCollection<ClassAnnouncementItem>
        {
            new ClassAnnouncementItem
            {
                Title = "Mathematics Test",
                Message = "Learners will write a short mathematics test on Friday."
            }
        };
    }


    public void LoadClass(TeacherClassItem selectedClass)
    {
        Grade = selectedClass.Grade;
        Subject = selectedClass.Subject;
        LearnerCount = selectedClass.LearnerCount;
    }
}
