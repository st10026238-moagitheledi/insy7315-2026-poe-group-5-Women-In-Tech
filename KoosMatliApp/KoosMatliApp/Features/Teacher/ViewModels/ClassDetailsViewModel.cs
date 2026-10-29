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


    private bool _isOverviewSelected = true;

    public bool IsOverviewSelected
    {
        get => _isOverviewSelected;
        set => SetProperty(ref _isOverviewSelected, value);
    }

    private bool _isLearnersSelected;

    public bool IsLearnersSelected
    {
        get => _isLearnersSelected;
        set => SetProperty(ref _isLearnersSelected, value);
    }

    private bool _isResourcesSelected;

    public bool IsResourcesSelected
    {
        get => _isResourcesSelected;
        set => SetProperty(ref _isResourcesSelected, value);
    }

    private bool _isAttendanceSelected;

    public bool IsAttendanceSelected
    {
        get => _isAttendanceSelected;
        set => SetProperty(ref _isAttendanceSelected, value);
    }


    public ObservableCollection<ClassTimetableItem> Timetable { get; }

    public ObservableCollection<HomeworkItem> Homework { get; }

    public ObservableCollection<ClassAnnouncementItem> Announcements { get; }

    public ObservableCollection<LearnerItem> Learners { get; }

    public ObservableCollection<ClassResourceItem> Resources { get; }


    public IRelayCommand ShowOverviewCommand { get; }

    public IRelayCommand ShowLearnersCommand { get; }

    public IRelayCommand ShowResourcesCommand { get; }

    public IRelayCommand ShowAttendanceCommand { get; }


    public ClassDetailsViewModel()
    {
        Timetable = new ObservableCollection<ClassTimetableItem>
        {
            new()
            {
                Day = "Mon",
                Time = "08:00 - 08:45"
            },

            new()
            {
                Day = "Wed",
                Time = "08:00 - 08:45"
            },

            new()
            {
                Day = "Fri",
                Time = "08:00 - 08:45"
            }
        };


        Homework = new ObservableCollection<HomeworkItem>();


        Announcements = new ObservableCollection<ClassAnnouncementItem>
        {
            new()
            {
                Title = "Reminder: Maths Test",
                Message = "Term 2 Test on 22 May 2024."
            }
        };

        Resources = new ObservableCollection<ClassResourceItem>
        {
            new()
            {
                Title = "Fractions Revision Notes",
                ResourceType = "PDF",
                Description = "Revision notes covering fractions and equivalent fractions."
        },

            new()
            {
               Title = "Multiplication Worksheet",
               ResourceType = "Worksheet",
               Description = "Practice questions for multiplication and number patterns."
        },

            new()
            {
              Title = "Term 2 Study Guide",
              ResourceType = "Study Guide",
              Description = "Study material covering the main Term 2 topics."
        },

           new()
           {
             Title = "Maths Formula Sheet",
             ResourceType = "Reference",
             Description = "Quick reference sheet for important mathematics formulas."
           }
        };


        Learners = new ObservableCollection<LearnerItem>
        {
            new()
            {
                FullName = "Lerato Mokoena",
                StudentNumber = "STD001"
            },

            new()
            {
                FullName = "Thabo Nkosi",
                StudentNumber = "STD002"
            },

            new()
            {
                FullName = "Amahle Dlamini",
                StudentNumber = "STD003"
            },

            new()
            {
                FullName = "Kagiso Molefe",
                StudentNumber = "STD004"
            },

            new()
            {
                FullName = "Naledi Khumalo",
                StudentNumber = "STD005"
            },

            new()
            {
                FullName = "Sipho Maseko",
                StudentNumber = "STD006"
            }
        };


        ShowOverviewCommand = new RelayCommand(ShowOverview);

        ShowLearnersCommand = new RelayCommand(ShowLearners);

        ShowResourcesCommand = new RelayCommand(ShowResources);

        ShowAttendanceCommand = new RelayCommand(ShowAttendance);
    }


    public void LoadClass(TeacherClassItem selectedClass)
    {
        Grade = selectedClass.Grade;
        Subject = selectedClass.Subject;
        LearnerCount = selectedClass.LearnerCount;

        RefreshHomework();
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
        IsOverviewSelected = true;
    }


    private void ShowLearners()
    {
        ResetTabs();
        IsLearnersSelected = true;
    }


    private void ShowResources()
    {
        ResetTabs();
        IsResourcesSelected = true;
    }


    private void ShowAttendance()
    {
        ResetTabs();
        IsAttendanceSelected = true;
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
            Homework.Add(homework);
        }
    }
}
