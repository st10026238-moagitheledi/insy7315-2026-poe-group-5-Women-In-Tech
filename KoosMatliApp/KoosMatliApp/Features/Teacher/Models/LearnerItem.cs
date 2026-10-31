using CommunityToolkit.Mvvm.ComponentModel;

namespace KoosMatliApp.Features.Teacher.Models;

public class LearnerItem : ObservableObject
{
    public string Id { get; set; } = string.Empty;


    private string _fullName = string.Empty;

    public string FullName
    {
        get => _fullName;

        set => SetProperty(
            ref _fullName,
            value);
    } // (Microsoft, 2026)


    private string _studentNumber =
        string.Empty;

    public string StudentNumber
    {
        get => _studentNumber;

        set => SetProperty(
            ref _studentNumber,
            value);
    }


    private string _attendanceStatus =
        string.Empty;

    public string AttendanceStatus
    {
        get => _attendanceStatus;

        set => SetProperty(
            ref _attendanceStatus,
            value);
    } // (Microsoft, 2026)
}


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].
