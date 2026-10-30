using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KoosMatliApp.Features.Teacher.Models;
using KoosMatliApp.Features.Teacher.Services;

namespace KoosMatliApp.Features.Teacher.ViewModels;

public class ClassDetailsViewModel : ObservableObject
{
    private string _grade = "Grade 5A";

    public string Grade
    {
        get => _grade;
        set => SetProperty(
            ref _grade,
            value);
    }


    private string _subject = "Mathematics";

    public string Subject
    {
        get => _subject;
        set => SetProperty(
            ref _subject,
            value);
    }


    private int _learnerCount;

    public int LearnerCount
    {
        get => _learnerCount;
        set => SetProperty(
            ref _learnerCount,
            value);
    }


    public int PresentToday { get; } =
        28;

    public int AbsentToday { get; } =
        4;

    public string AverageAttendance { get; } =
        "95%";


    private bool _isOverviewSelected =
        true;

    public bool IsOverviewSelected
    {
        get => _isOverviewSelected;
        set => SetProperty(
            ref _isOverviewSelected,
            value);
    }


    private bool _isLearnersSelected;

    public bool IsLearnersSelected
    {
        get => _isLearnersSelected;
        set => SetProperty(
            ref _isLearnersSelected,
            value);
    }


    private bool _isResourcesSelected;

    public bool IsResourcesSelected
    {
        get => _isResourcesSelected;
        set => SetProperty(
            ref _isResourcesSelected,
            value);
    }


    private bool _isAttendanceSelected;

    public bool IsAttendanceSelected
    {
        get => _isAttendanceSelected;
        set => SetProperty(
            ref _isAttendanceSelected,
            value);
    }


    public ObservableCollection<TeacherScheduleItem>
        Timetable
    { get; }


    public ObservableCollection<HomeworkItem>
        Homework
    { get; }


    public ObservableCollection<ClassAnnouncementItem>
        Announcements
    { get; }


    public ObservableCollection<LearnerItem>
        Learners
    { get; }


    public ObservableCollection<ClassResourceItem>
        Resources
    { get; }


    public IRelayCommand ShowOverviewCommand { get; }

    public IRelayCommand ShowLearnersCommand { get; }

    public IRelayCommand ShowResourcesCommand { get; }

    public IRelayCommand ShowAttendanceCommand { get; }


    public ClassDetailsViewModel()
    {
        Timetable =
            new ObservableCollection<TeacherScheduleItem>();


        Homework =
            new ObservableCollection<HomeworkItem>();


        Announcements =
            new ObservableCollection<ClassAnnouncementItem>();


        Learners =
            new ObservableCollection<LearnerItem>();


        Resources =
            new ObservableCollection<ClassResourceItem>
            {
                new()
                {
                    Title = "Fractions Revision Notes",
                    ResourceType = "PDF",
                    Description =
                        "Revision notes covering fractions and equivalent fractions."
                },

                new()
                {
                    Title = "Multiplication Worksheet",
                    ResourceType = "Worksheet",
                    Description =
                        "Practice questions for multiplication and number patterns."
                },

                new()
                {
                    Title = "Term 2 Study Guide",
                    ResourceType = "Study Guide",
                    Description =
                        "Study material covering the main Term 2 topics."
                },

                new()
                {
                    Title = "Maths Formula Sheet",
                    ResourceType = "Reference",
                    Description =
                        "Quick reference sheet for important mathematics formulas."
                }
            };


        ShowOverviewCommand = new RelayCommand(ShowOverview);

        ShowLearnersCommand = new RelayCommand(ShowLearners);

        ShowResourcesCommand = new RelayCommand(ShowResources);

        ShowAttendanceCommand = new RelayCommand(ShowAttendance);
    }


    public void LoadClass(
        TeacherClassItem selectedClass)
    {
        Grade =
            selectedClass.Grade;

        Subject =
            selectedClass.Subject;


        RefreshLearners();

        RefreshHomework();

        RefreshAnnouncements();

        RefreshTimetable();
    }


    private void ResetTabs()
    {
        IsOverviewSelected = false;

        IsLearnersSelected = false;

        IsResourcesSelected = false;

        IsAttendanceSelected = false;
    }


    private void ShowOverview()
    {
        ResetTabs();

        IsOverviewSelected =
            true;
    }


    private void ShowLearners()
    {
        ResetTabs();

        IsLearnersSelected =
            true;
    }


    private void ShowResources()
    {
        ResetTabs();

        IsResourcesSelected =
            true;
    }


    private void ShowAttendance()
    {
        ResetTabs();

        IsAttendanceSelected =
            true;
    }


    public void RefreshHomework()
    {
        Homework.Clear();


        var homeworkItems =
            HomeworkService.GetHomework(
                Grade,
                Subject);


        foreach (var homework in homeworkItems)
        {
            Homework.Add(
                homework);
        }
    }


    public void RefreshLearners()
    {
        Learners.Clear();


        var learners =
            LearnerService.GetLearners(
                Grade,
                Subject);


        foreach (var learner in learners)
        {
            Learners.Add(
                learner);
        }


        LearnerCount =
            Learners.Count;
    }


    public void RefreshAnnouncements()
    {
        Announcements.Clear();


        var announcements =
            AnnouncementService
                .GetAnnouncementsForClass(
                    Grade,
                    Subject);


        foreach (var announcement in announcements)
        {
            Announcements.Add(
                announcement);
        }
    }


    public void RefreshTimetable()
    {
        Timetable.Clear();


        var scheduleItems =
            ScheduleService.GetClassSchedule(
                Grade,
                Subject);


        foreach (var item in scheduleItems)
        {
            Timetable.Add(
                item);
        }
    }
}