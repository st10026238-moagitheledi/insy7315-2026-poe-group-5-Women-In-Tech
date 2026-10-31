using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using KoosMatliApp.Features.Teacher.Models;
using KoosMatliApp.Features.Teacher.Services;

namespace KoosMatliApp.Features.Teacher.ViewModels;

public class ClassesViewModel : ObservableObject
{
    private readonly TeacherApiService
        _teacherApiService;


    public ObservableCollection<TeacherClassItem>
        MyClasses
    { get; } = new(); // (Microsoft, 2026)


    public ObservableCollection<TeacherClassItem>
        AllClasses
    { get; } = new();


    private bool _isMyClassesSelected = true;

    public bool IsMyClassesSelected
    {
        get => _isMyClassesSelected;

        set => SetProperty(
            ref _isMyClassesSelected,
            value); // (Microsoft, 2026)
    }


    private bool _isAllClassesSelected;

    public bool IsAllClassesSelected
    {
        get => _isAllClassesSelected;

        set => SetProperty(
            ref _isAllClassesSelected,
            value);
    }


    private bool _isLoading;

    public bool IsLoading
    {
        get => _isLoading;

        set => SetProperty(
            ref _isLoading,
            value);
    } // (Microsoft, 2026)


    private string _errorMessage =
        string.Empty;

    public string ErrorMessage
    {
        get => _errorMessage;

        set => SetProperty(
            ref _errorMessage,
            value);
    }


    public IRelayCommand
        ShowMyClassesCommand
    { get; } // (Microsoft, 2026)


    public IRelayCommand
        ShowAllClassesCommand
    { get; }


    public ClassesViewModel(
        TeacherApiService teacherApiService)
    {
        _teacherApiService =
            teacherApiService;


        ShowMyClassesCommand =
            new RelayCommand(
                ShowMyClasses);


        ShowAllClassesCommand =
            new RelayCommand(
                ShowAllClasses);
    } // (Microsoft, 2026)


    private void ShowMyClasses()
    {
        IsMyClassesSelected =
            true;

        IsAllClassesSelected =
            false;
    }


    private void ShowAllClasses()
    {
        IsMyClassesSelected =
            false;

        IsAllClassesSelected =
            true;
    }


    public async Task<bool>
        RefreshClassesAsync()
    {
        if (IsLoading)
        {
            return false;
        } // (Microsoft, 2026)


        try
        {
            IsLoading =
                true;

            ErrorMessage =
                string.Empty;


            var classesResponse =
                await _teacherApiService
                    .GetClassesAsync();


            if (classesResponse == null ||
                !classesResponse.Success)
            {
                ErrorMessage =
                    "Unable to load classes.";

                return false;
            }

            var learnersResponse =
                await _teacherApiService
                    .GetLearnersAsync();


            var learners =
                learnersResponse?.Learners ??
                new List
                    <TeacherLearnerApiModel>(); // (Microsoft, 2026)


            MyClasses.Clear();

            AllClasses.Clear();


            foreach (
                var classItem
                in classesResponse.MyClasses)
            {
                MyClasses.Add(
                    ConvertClass(
                        classItem,
                        learners));
            }


            foreach (
                var classItem
                in classesResponse.AllClasses)
            {
                AllClasses.Add(
                    ConvertClass(
                        classItem,
                        learners));
            } // (Microsoft, 2026)


            return true;
        }
        catch (Exception ex)
        {
            ErrorMessage =
                ex.Message;

            return false;
        }
        finally
        {
            IsLoading =
                false;
        }
    }


    private static TeacherClassItem
        ConvertClass(
            TeacherClassApiModel apiClass,
            IEnumerable
                <TeacherLearnerApiModel>
                learners)
    {
        var className =
            FirstAvailable(
                apiClass.ClassName,
                apiClass.Grade,
                apiClass.Name,
                apiClass.Code,
                apiClass.Id,
                "Class"); // (Microsoft, 2026)


        var subject =
            FirstAvailable(
                apiClass.Subject,
                GetDifferentValue(
                    apiClass.Name,
                    className),
                GetDifferentValue(
                    apiClass.Code,
                    className),
                "Class");


        var schedule =
            FirstAvailable(
                apiClass.Schedule,
                string.Empty);


        var identifiers =
            new[]
            {
                apiClass.Id,
                apiClass.ClassId,
                apiClass.ClassName,
                apiClass.Grade,
                apiClass.Name,
                apiClass.Code
            }
            .Where(value =>
                !string.IsNullOrWhiteSpace(
                    value))
            .Select(value =>
                value.Trim()
                    .ToLowerInvariant())
            .Distinct()
            .ToList(); // (Microsoft, 2026)


        var actualLearnerCount =
            learners.Count(
                learner =>
                {
                    var learnerClass =
                        learner.ClassName?
                            .Trim()
                            .ToLowerInvariant();


                    return
                        !string.IsNullOrWhiteSpace(
                            learnerClass)
                        &&
                        identifiers.Contains(
                            learnerClass);
                });


        var learnerCount =
            actualLearnerCount > 0
                ? actualLearnerCount
                : apiClass.LearnerCount ?? 0;


        return new TeacherClassItem
        {
            Id =
                apiClass.Id,

            Grade =
                className,

            Subject =
                subject,

            Schedule =
                schedule,

            LearnerCount =
                learnerCount // (Microsoft, 2026)
        };
    }


    private static string
        GetDifferentValue(
            string value,
            string comparison)
    {
        if (string.IsNullOrWhiteSpace(
                value))
        {
            return string.Empty;
        }


        if (string.Equals(
                value.Trim(),
                comparison.Trim(),
                StringComparison
                    .OrdinalIgnoreCase))
        {
            return string.Empty;
        }


        return value.Trim();
    } // (Microsoft, 2026)


    private static string
        FirstAvailable(
            params string[] values)
    {
        foreach (var value in values)
        {
            if (!string.IsNullOrWhiteSpace(
                    value))
            {
                return value.Trim();
            }
        }


        return string.Empty;
    } // (Microsoft, 2026)
}


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].

