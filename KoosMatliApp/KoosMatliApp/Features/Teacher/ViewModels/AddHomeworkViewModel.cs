using CommunityToolkit.Mvvm.ComponentModel;
using KoosMatliApp.Features.Teacher.Models;
using KoosMatliApp.Features.Teacher.Services;

namespace KoosMatliApp.Features.Teacher.ViewModels;

public class AddHomeworkViewModel : ObservableObject
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


    private string _title = string.Empty;

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }


    private string _description = string.Empty;

    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }


    private DateTime _dueDate =
        DateTime.Today.AddDays(7);

    public DateTime DueDate
    {
        get => _dueDate;
        set => SetProperty(ref _dueDate, value);
    }


    private string _status = "Assigned";

    public string Status
    {
        get => _status;
        set => SetProperty(ref _status, value);
    }


    private string _attachmentName =
        "No file selected";

    public string AttachmentName
    {
        get => _attachmentName;
        set => SetProperty(ref _attachmentName, value);
    }


    public void LoadClassInformation(
        string grade,
        string subject)
    {
        Grade = grade;
        Subject = subject;
    }


    public void SaveHomework()
    {
        var homework =
            new HomeworkItem
            {
                Title = Title.Trim(),

                Description =
                    Description.Trim(),

                DueDate =
                    $"Due {DueDate:dd MMM yyyy}",

                Status = Status,

                Grade = Grade,

                Subject = Subject,

                AttachmentName =
                    AttachmentName == "No file selected"
                        ? string.Empty
                        : AttachmentName
            };

        HomeworkService.AddHomework(homework);
    }
}