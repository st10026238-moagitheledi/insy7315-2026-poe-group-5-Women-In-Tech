using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using KoosMatliApp.Features.Teacher.Models;
using KoosMatliApp.Features.Teacher.Services;

namespace KoosMatliApp.Features.Teacher.ViewModels;

public class HomeworkViewModel : ObservableObject
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


    public ObservableCollection<HomeworkItem> Homework { get; } =
        new();


    public void LoadClassInformation(
        string grade,
        string subject)
    {
        Grade = grade;
        Subject = subject;

        RefreshHomework();
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