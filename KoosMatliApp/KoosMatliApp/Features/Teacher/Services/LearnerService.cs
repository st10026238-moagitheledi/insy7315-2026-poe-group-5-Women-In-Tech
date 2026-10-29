using System.Collections.ObjectModel;
using KoosMatliApp.Features.Teacher.Models;

namespace KoosMatliApp.Features.Teacher.Services;

public static class LearnerService
{
    private static readonly Dictionary<string, ObservableCollection<LearnerItem>>
        _classLearners = new();


    static LearnerService()
    {
        var grade5AKey =
            GetClassKey(
                "Grade 5A",
                "Mathematics");


        _classLearners[grade5AKey] =
            new ObservableCollection<LearnerItem>
            {
                new()
                {
                    FullName = "Lerato Mokoena",
                    StudentNumber = "STD001"
                },

                new()
                {
                    FullName = "Thabo Nkosi",
                    StudentNumber = "STD002"
                },

                new()
                {
                    FullName = "Amahle Dlamini",
                    StudentNumber = "STD003"
                },

                new()
                {
                    FullName = "Kagiso Molefe",
                    StudentNumber = "STD004"
                },

                new()
                {
                    FullName = "Naledi Khumalo",
                    StudentNumber = "STD005"
                },

                new()
                {
                    FullName = "Sipho Maseko",
                    StudentNumber = "STD006"
                }
            };
    }


    private static string GetClassKey(
        string grade,
        string subject)
    {
        return $"{grade}|{subject}";
    }


    public static IEnumerable<LearnerItem> GetLearners(
        string grade,
        string subject)
    {
        var key =
            GetClassKey(
                grade,
                subject);


        if (!_classLearners.ContainsKey(key))
        {
            _classLearners[key] =
                new ObservableCollection<LearnerItem>();
        }


        return _classLearners[key];
    }


    public static void AddLearner(
        string grade,
        string subject,
        LearnerItem learner)
    {
        var key =
            GetClassKey(
                grade,
                subject);


        if (!_classLearners.ContainsKey(key))
        {
            _classLearners[key] =
                new ObservableCollection<LearnerItem>();
        }


        _classLearners[key].Add(learner);
    }

    public static int GetLearnerCount(
       string grade,
       string subject)
    {
        var key =
            GetClassKey(
                grade,
                subject);

        if (_classLearners.TryGetValue(
                key,
                out var learners))
        {
            return learners.Count;
        }

        return 0;
    }

    public static void UpdateLearner(
        string grade,
        string subject,
        LearnerItem learner,
        string fullName,
        string studentNumber)
    {
        var key =
            GetClassKey(
                grade,
                subject);

        if (!_classLearners.ContainsKey(key))
        {
            return;
        }


        learner.FullName =
            fullName;

        learner.StudentNumber =
            studentNumber;
    }


    public static void DeleteLearner(
        string grade,
        string subject,
        LearnerItem learner)
    {
        var key =
            GetClassKey(
                grade,
                subject);

        if (!_classLearners.ContainsKey(key))
        {
            return;
        }


        _classLearners[key].Remove(
            learner);
    }
}
