using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Storage;
using KoosMatliApp.Features.Teacher.Services;

namespace KoosMatliApp.Features.Teacher.ViewModels;

public class AddHomeworkViewModel : ObservableObject
{
    private readonly TeacherApiService
        _teacherApiService;


    public AddHomeworkViewModel(
        TeacherApiService teacherApiService)
    {
        _teacherApiService =
            teacherApiService;
    } // (Microsoft, 2026)


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


    private string _title =
        string.Empty;

    public string Title
    {
        get => _title;
        set => SetProperty(
            ref _title,
            value);
    } // (Microsoft, 2026)


    private string _description =
        string.Empty;

    public string Description
    {
        get => _description;
        set => SetProperty(
            ref _description,
            value);
    }


    private DateTime _dueDate =
        DateTime.Today.AddDays(7);

    public DateTime DueDate
    {
        get => _dueDate;
        set => SetProperty(
            ref _dueDate,
            value); // (Microsoft, 2026)
    }


    private string _status =
        "Assigned";

    public string Status
    {
        get => _status;
        set => SetProperty(
            ref _status,
            value);
    }


    private string _attachmentName =
        "No file selected";

    public string AttachmentName
    {
        get => _attachmentName;
        set => SetProperty(
            ref _attachmentName,
            value);
    }


    private string _attachmentPath =
        string.Empty;

    public string AttachmentPath
    {
        get => _attachmentPath;
        set => SetProperty(
            ref _attachmentPath,
            value);
    }


    public string ErrorMessage { get; private set; } =
        string.Empty;


    public void LoadClassInformation(
        string grade,
        string subject)
    {
        Grade =
            grade;

        Subject =
            subject; // (Microsoft, 2026)
    }


    public async Task<bool> SaveHomeworkAsync()
    {
        try
        {
            ErrorMessage =
                string.Empty;


            var attachmentName =
                AttachmentName ==
                    "No file selected"
                    ? string.Empty
                    : AttachmentName;


            var response =
                await _teacherApiService
                    .CreateHomeworkAsync(
                        Title.Trim(),
                        Description.Trim(),
                        DueDate,
                        Status,
                        Grade,
                        Subject,
                        attachmentName); // (Microsoft, 2026)


            if (response == null ||
                !response.Success ||
                response.Homework == null)
            {
                ErrorMessage =
                    response?.Message ??
                    "Unable to save homework.";

                return false;
            }


            if (!string.IsNullOrWhiteSpace(
                    AttachmentPath) &&
                !string.IsNullOrWhiteSpace(
                    response.Homework.Id))
            {
                Preferences.Default.Set(
                    $"HomeworkAttachmentPath_{response.Homework.Id}",
                    AttachmentPath);
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

