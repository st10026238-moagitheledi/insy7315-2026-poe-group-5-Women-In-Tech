using System.Collections.ObjectModel;
using KoosMatliApp.Features.Teacher.Models;

namespace KoosMatliApp.Features.Teacher.Services;

public static class HomeworkService
{
    private static readonly ObservableCollection<HomeworkItem> _homework =
        new()
        {
            new HomeworkItem
            {
                Title = "Fractions Worksheet",
                Description = "Complete the fractions worksheet.",
                DueDate = "Due 17 May 2024",
                Status = "Assigned",
                Grade = "Grade 5A",
                Subject = "Mathematics",
                AttachmentName = "fractions-worksheet.pdf"
            },

            new HomeworkItem
            {
                Title = "Word Problems",
                Description = "Complete the word problem activity.",
                DueDate = "Due 20 May 2024",
                Status = "Draft",
                Grade = "Grade 5A",
                Subject = "Mathematics"
            }
        };


    public static IEnumerable<HomeworkItem> GetHomework(
        string grade,
        string subject)
    {
        return _homework.Where(
            homework =>
                homework.Grade == grade &&
                homework.Subject == subject);
    }


    public static void AddHomework(
        HomeworkItem homework)
    {
        _homework.Add(homework);
    }
}