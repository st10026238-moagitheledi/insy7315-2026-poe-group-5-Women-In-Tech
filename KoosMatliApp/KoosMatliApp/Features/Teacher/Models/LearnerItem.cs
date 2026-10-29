using CommunityToolkit.Mvvm.ComponentModel;

namespace KoosMatliApp.Features.Teacher.Models;

public class LearnerItem : ObservableObject
{
    private string _fullName =
        string.Empty;

    public string FullName
    {
        get => _fullName;
        set => SetProperty(
            ref _fullName,
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


    private string _attendanceStatus =
        "Not Marked";

    public string AttendanceStatus
    {
        get => _attendanceStatus;

        set => SetProperty(
            ref _attendanceStatus,
            value);
    }
}