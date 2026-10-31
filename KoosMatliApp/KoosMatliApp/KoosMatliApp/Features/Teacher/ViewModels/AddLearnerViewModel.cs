using CommunityToolkit.Mvvm.ComponentModel;
using KoosMatliApp.Features.Teacher.Services;

namespace KoosMatliApp.Features.Teacher.ViewModels;

public class AddLearnerViewModel : ObservableObject
{
    private readonly TeacherApiService
        _teacherApiService; // (Microsoft, 2026)


    public AddLearnerViewModel(
        TeacherApiService teacherApiService)
    {
        _teacherApiService =
            teacherApiService;
    }


    public string ClassId { get; private set; } =
        string.Empty;


    private string _grade =
        string.Empty;

    public string Grade
    {
        get => _grade;
        set => SetProperty(
            ref _grade,
            value); // (Microsoft, 2026)
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
            value); // (Microsoft, 2026)
    }


    public string ErrorMessage { get; private set; } =
        string.Empty;


    public void LoadClassInformation(
        string grade,
        string subject,
        string classId)
    {
        Grade = grade;
        Subject = subject;
        ClassId = classId;
    }


    public async Task<bool> SaveLearnerAsync()
    {
        try
        {
            ErrorMessage =
                string.Empty; // (Microsoft, 2026)


            var response =
                await _teacherApiService
                    .CreateLearnerAsync(
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
                    "Unable to add learner.";

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
}


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].

