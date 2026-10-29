using CommunityToolkit.Mvvm.ComponentModel;
using KoosMatliApp.Features.Teacher.Models;
using KoosMatliApp.Features.Teacher.Services;

namespace KoosMatliApp.Features.Teacher.ViewModels;

public class AddLearnerViewModel : ObservableObject
{
    private string _grade = string.Empty;

    public string Grade
    {
        get => _grade;
        set => SetProperty(ref _grade, value);
    }


    private string _subject = string.Empty;

    public string Subject
    {
        get => _subject;
        set => SetProperty(ref _subject, value);
    }


    private string _firstName = string.Empty;

    public string FirstName
    {
        get => _firstName;
        set => SetProperty(ref _firstName, value);
    }


    private string _lastName = string.Empty;

    public string LastName
    {
        get => _lastName;
        set => SetProperty(ref _lastName, value);
    }


    private string _studentNumber = string.Empty;

    public string StudentNumber
    {
        get => _studentNumber;
        set => SetProperty(ref _studentNumber, value);
    }


    public void LoadClassInformation(
        string grade,
        string subject)
    {
        Grade = grade;
        Subject = subject;
    }


    public void SaveLearner()
    {
        var learner =
            new LearnerItem
            {
                FullName =
                    $"{FirstName.Trim()} {LastName.Trim()}",

                StudentNumber =
                    StudentNumber.Trim()
            };


        LearnerService.AddLearner(
            Grade,
            Subject,
            learner);
    }
}
