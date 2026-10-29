using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KoosMatliApp.Features.Teacher.Models;
using KoosMatliApp.Features.Teacher.Services;

namespace KoosMatliApp.Features.Teacher.ViewModels;

public class ClassesViewModel : ObservableObject
{


    public ObservableCollection<TeacherClassItem> MyClasses { get; }

    public ObservableCollection<TeacherClassItem> AllClasses { get; }




    private bool _isMyClassesSelected = true;

    public bool IsMyClassesSelected
    {
        get => _isMyClassesSelected;

        set => SetProperty(ref _isMyClassesSelected, value);
    }


    private bool _isAllClassesSelected = false;

    public bool IsAllClassesSelected
    {
        get => _isAllClassesSelected;

        set => SetProperty(ref _isAllClassesSelected, value);
    }




    public IRelayCommand ShowMyClassesCommand { get; }

    public IRelayCommand ShowAllClassesCommand { get; }



    public ClassesViewModel()
    {

        MyClasses = new ObservableCollection<TeacherClassItem>
        {
            new TeacherClassItem
            {
                Grade = "Grade 5A",
                LearnerCount = 32,
                Subject = "Mathematics",
                Schedule = "Mon, Wed, Fri  |  08:00 - 08:45"
            },

            new TeacherClassItem
            {
                Grade = "Grade 4B",
                LearnerCount = 28,
                Subject = "Life Skills",
                Schedule = "Mon, Wed  |  09:00 - 09:45"
            },

            new TeacherClassItem
            {
                Grade = "Grade 6A",
                LearnerCount = 30,
                Subject = "Natural Science",
                Schedule = "Tue, Thu  |  10:00 - 10:45"
            },

            new TeacherClassItem
            {
                Grade = "Grade 3C",
                LearnerCount = 26,
                Subject = "English Home Language",
                Schedule = "Tue, Thu  |  12:00 - 12:45"
            }
        };


        AllClasses = new ObservableCollection<TeacherClassItem>
        {
            new TeacherClassItem
            {
                Grade = "Grade 5A",
                LearnerCount = 32,
                Subject = "Mathematics",
                Schedule = "Mon, Wed, Fri  |  08:00 - 08:45"
            },

            new TeacherClassItem
            {
                Grade = "Grade 4B",
                LearnerCount = 28,
                Subject = "Life Skills",
                Schedule = "Mon, Wed  |  09:00 - 09:45"
            },

            new TeacherClassItem
            {
                Grade = "Grade 6A",
                LearnerCount = 30,
                Subject = "Natural Science",
                Schedule = "Tue, Thu  |  10:00 - 10:45"
            },

            new TeacherClassItem
            {
                Grade = "Grade 3C",
                LearnerCount = 26,
                Subject = "English Home Language",
                Schedule = "Tue, Thu  |  12:00 - 12:45"
            },

            new TeacherClassItem
            {
                Grade = "Grade 7A",
                LearnerCount = 31,
                Subject = "Mathematics",
                Schedule = "Mon, Tue  |  13:00 - 13:45"
            },

            new TeacherClassItem
            {
                Grade = "Grade 2B",
                LearnerCount = 25,
                Subject = "Life Skills",
                Schedule = "Wed, Fri  |  11:00 - 11:45"
            }
        };


        ShowMyClassesCommand =
            new RelayCommand(ShowMyClasses);

        ShowAllClassesCommand =
            new RelayCommand(ShowAllClasses);
    }


    private void ShowMyClasses()
    {
        IsMyClassesSelected = true;
        IsAllClassesSelected = false;
    }

    private void ShowAllClasses()
    {
        IsMyClassesSelected = false;
        IsAllClassesSelected = true;
    }

    public void RefreshLearnerCounts()
    {
        foreach (var classItem in MyClasses)
        {
            classItem.LearnerCount =
                LearnerService.GetLearnerCount(
                    classItem.Grade,
                    classItem.Subject);
        }


        foreach (var classItem in AllClasses)
        {
            classItem.LearnerCount =
                LearnerService.GetLearnerCount(
                    classItem.Grade,
                    classItem.Subject);
        }
    }

}