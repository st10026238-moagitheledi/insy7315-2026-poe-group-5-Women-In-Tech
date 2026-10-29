using CommunityToolkit.Mvvm.ComponentModel;
using KoosMatliApp.Features.Teacher.Models;
using KoosMatliApp.Features.Teacher.Services;

namespace KoosMatliApp.Features.Teacher.ViewModels;

public class LearnerDetailsViewModel : ObservableObject
{
    private LearnerItem? _selectedLearner;


    private string _grade =
        string.Empty;

    public string Grade
    {
        get => _grade;
        set => SetProperty(
            ref _grade,
            value);
    }


    private string _subject =
        string.Empty;

    public string Subject
    {
        get => _subject;
        set => SetProperty(
            ref _subject,
            value);
    }


    private string _firstName =
        string.Empty;

    public string FirstName
    {
        get => _firstName;
        set => SetProperty(
            ref _firstName,
            value);
    }


    private string _lastName =
        string.Empty;

    public string LastName
    {
        get => _lastName;
        set => SetProperty(
            ref _lastName,
            value);
    }


    private string _studentNumber =
        string.Empty;

    public string StudentNumber
    {
        get => _studentNumber;
        set => SetProperty(
            ref _studentNumber,
            value);
    }


    public void LoadLearner(
        string grade,
        string subject,
        LearnerItem learner)
    {
        Grade =
            grade;

        Subject =
            subject;

        _selectedLearner =
            learner;


        var nameParts =
            learner.FullName
                .Split(
                    ' ',
                    2,
                    StringSplitOptions.RemoveEmptyEntries);


        FirstName =
            nameParts.Length > 0
                ? nameParts[0]
                : string.Empty;


        LastName =
            nameParts.Length > 1
                ? nameParts[1]
                : string.Empty;


        StudentNumber =
            learner.StudentNumber;
    }


    public void SaveChanges()
    {
        if (_selectedLearner == null)
        {
            return;
        }


        LearnerService.UpdateLearner(
            Grade,
            Subject,
            _selectedLearner,
            $"{FirstName.Trim()} {LastName.Trim()}",
            StudentNumber.Trim());
    }


    public void DeleteLearner()
    {
        if (_selectedLearner == null)
        {
            return;
        }


        LearnerService.DeleteLearner(
            Grade,
            Subject,
            _selectedLearner);
    }
}