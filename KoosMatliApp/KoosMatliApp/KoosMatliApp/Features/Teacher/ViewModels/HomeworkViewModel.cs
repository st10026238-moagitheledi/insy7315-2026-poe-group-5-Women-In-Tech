using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Storage;
using KoosMatliApp.Features.Teacher.Models;
using KoosMatliApp.Features.Teacher.Services;

namespace KoosMatliApp.Features.Teacher.ViewModels;

public class HomeworkViewModel : ObservableObject
{
    private readonly TeacherApiService
        _teacherApiService;


    public HomeworkViewModel(
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


    public ObservableCollection<HomeworkItem>
        Homework
    { get; } =
        new(); // (Microsoft, 2026)


    public string ErrorMessage { get; private set; } =
        string.Empty;


    public void LoadClassInformation(
        string grade,
        string subject)
    {
        Grade =
            grade;

        Subject =
            subject;
    }


    public async Task<bool> RefreshHomeworkAsync()
    {
        try
        {
            ErrorMessage =
                string.Empty;


            var response =
                await _teacherApiService
                    .GetHomeworkAsync(
                        Grade,
                        Subject);


            if (response == null ||
                !response.Success)
            {
                ErrorMessage =
                    "Unable to load homework.";

                return false;
            } // (Microsoft, 2026)


            Homework.Clear();


            foreach (var apiHomework
                in response.Homework)
            {
                var dueDate =
                    apiHomework.DueDate;


                if (DateTime.TryParse(
                        apiHomework.DueDate,
                        out var parsedDate))
                {
                    dueDate =
                        $"Due {parsedDate:dd MMM yyyy}";
                }


                var attachmentPath =
                    string.IsNullOrWhiteSpace(
                        apiHomework.Id)
                        ? string.Empty
                        : Preferences.Default.Get(
                            $"HomeworkAttachmentPath_{apiHomework.Id}",
                            string.Empty); // (Microsoft, 2026)


                Homework.Add(
                    new HomeworkItem
                    {
                        Id =
                            apiHomework.Id,

                        Title =
                            apiHomework.Title,

                        Description =
                            apiHomework.Description,

                        DueDate =
                            dueDate,

                        Status =
                            apiHomework.Status,

                        Grade =
                            apiHomework.Grade,

                        Subject =
                            apiHomework.Subject,

                        AttachmentName =
                            apiHomework.AttachmentName,

                        AttachmentPath =
                            attachmentPath
                    });
            } // (Microsoft, 2026)


            return true;
        }
        catch (Exception ex)
        {
            ErrorMessage =
                ex.Message;

            return false;
        }
    } // (Microsoft, 2026)
}


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].

