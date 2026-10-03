namespace KoosMatliApp.Features.Teacher.Models;

public class TeacherClassItem
{
    public string Grade { get; set; } = string.Empty;

    public int LearnerCount { get; set; }

    public string Subject { get; set; } = string.Empty;

    public string Schedule { get; set; } = string.Empty;
}
