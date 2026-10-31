using CommunityToolkit.Mvvm.ComponentModel;

namespace KoosMatliApp.Features.Teacher.Models;

public class TeacherClassItem : ObservableObject
{
    // Firestore/API class document ID. Used when requesting learners for this class.
    public string Id { get; set; } =
        string.Empty;


    public string Grade { get; set; } =
        string.Empty; // (Microsoft, 2026)


    private int _learnerCount;

    public int LearnerCount
    {
        get => _learnerCount;

        set => SetProperty(
            ref _learnerCount,
            value);
    }


    public string Subject { get; set; } =
        string.Empty;


    public string Schedule { get; set; } =
        string.Empty; // (Microsoft, 2026)
}


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].

