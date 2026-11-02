using CommunityToolkit.Mvvm.ComponentModel;
using KoosMatliApp.Features.Teacher.Models;
using KoosMatliApp.Features.Teacher.Services;

namespace KoosMatliApp.Features.Teacher.ViewModels;

public class LearnerDetailsViewModel : ObservableObject
{
    private readonly TeacherApiService
        _teacherApiService;


    private LearnerItem?
        _selectedLearner; // (Microsoft, 2026)


    public LearnerDetailsViewModel(
        TeacherApiService teacherApiService)
    {
        _teacherApiService =
            teacherApiService;
    }


    public string ClassId { get; private set; } =
        string.Empty;


    public string Grade { get; private set; } =
        string.Empty;


    public string Subject { get; private set; } =
        string.Empty;


    private string _firstName =
        string.Empty;

    public string FirstName
    {
        get => _firstName;
        set => SetProperty(
            ref _firstName,
            value); // (Microsoft, 2026)
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


    public string ErrorMessage { get; private set; } =
        string.Empty;


    public void LoadLearner(
        string grade,
        string subject,
        string classId,
        LearnerItem learner)
    {
        Grade = grade;
        Subject = subject;
        ClassId = classId;

        _selectedLearner =
            learner; // (Microsoft, 2026)


        var nameParts =
            learner.FullName.Split(
                ' ',
                2,
                StringSplitOptions
                    .RemoveEmptyEntries);


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


    public async Task<bool>
        SaveChangesAsync()
    {
        if (_selectedLearner == null ||
            string.IsNullOrWhiteSpace(
                _selectedLearner.Id))
        {
            ErrorMessage =
                "This learner does not have a valid ID.";

            return false; // (Microsoft, 2026)
        }


        try
        {
            var response =
                await _teacherApiService
                    .UpdateLearnerAsync(
                        _selectedLearner.Id,
                        FirstName.Trim(),
                        LastName.Trim(),
                        Grade,
                        Grade,
                        StudentNumber.Trim());


            if (response == null ||
                !response.Success)
            {
                ErrorMessage =
                    response?.Message ??
                    "Unable to update learner.";

                return false;
            }


            return true;
        }
        catch (Exception ex)
        {
            ErrorMessage =
                ex.Message;

            return false; // (Microsoft, 2026)
        }
    }


    public async Task<bool>
        DeleteLearnerAsync()
    {
        if (_selectedLearner == null ||
            string.IsNullOrWhiteSpace(
                _selectedLearner.Id))
        {
            ErrorMessage =
                "This learner does not have a valid ID.";

            return false;
        }


        try
        {
            var response =
                await _teacherApiService
                    .DeleteLearnerAsync(
                        _selectedLearner.Id);


            if (response == null ||
                !response.Success)
            {
                ErrorMessage =
                    response?.Message ??
                    "Unable to remove learner.";

                return false; // (Microsoft, 2026)
            }


            return true;
        }
        catch (Exception ex)
        {
            ErrorMessage =
                ex.Message;

            return false; // (Microsoft, 2026)
        }
    }
}


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].

