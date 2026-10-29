using CommunityToolkit.Mvvm.ComponentModel;

namespace KoosMatliApp.Features.Teacher.Models;

public class TeacherClassItem : ObservableObject
{
    public string Grade { get; set; } = string.Empty;

    private int _learnerCount;

    public int LearnerCount
    {
        get => _learnerCount;
        set => SetProperty(ref _learnerCount, value);
    }

    public string Subject { get; set; } = string.Empty;

    public string Schedule { get; set; } = string.Empty;
}
