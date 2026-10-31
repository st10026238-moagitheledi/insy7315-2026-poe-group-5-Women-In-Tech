using CommunityToolkit.Mvvm.ComponentModel;
using KoosMatliApp.Features.Teacher.Services;

namespace KoosMatliApp.Features.Teacher.ViewModels;

public class ProfileHubViewModel : ObservableObject
{
    private readonly TeacherApiService
        _teacherApiService;


    public ProfileHubViewModel(
        TeacherApiService teacherApiService)
    {
        _teacherApiService =
            teacherApiService; // (Microsoft, 2026)
    }


    private string _fullName =
        string.Empty;

    public string FullName
    {
        get => _fullName;

        set => SetProperty(
            ref _fullName,
            value);
    }


    private string _initials =
        string.Empty;

    public string Initials
    {
        get => _initials;

        set => SetProperty(
            ref _initials,
            value); // (Microsoft, 2026)
    }


    private string _position =
        string.Empty;

    public string Position
    {
        get => _position;

        set => SetProperty(
            ref _position,
            value);
    }


    private string _employeeNumber =
        string.Empty;

    public string EmployeeNumber
    {
        get => _employeeNumber;

        set => SetProperty(
            ref _employeeNumber,
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


    private bool _isLoading;

    public bool IsLoading
    {
        get => _isLoading;

        set => SetProperty(
            ref _isLoading,
            value);
    }


    public async Task<bool> RefreshProfileAsync()
    {
        if (IsLoading)
        {
            return false;
        } // (Microsoft, 2026)


        try
        {
            IsLoading = true;

            ErrorMessage =
                string.Empty;


            var response =
                await _teacherApiService
                    .GetProfileAsync();


            if (response == null ||
                !response.Success ||
                response.Profile == null)
            {
                ErrorMessage =
                    "Unable to load the teacher profile.";

                return false;
            }


            var profile =
                response.Profile;


            FullName =
                string.IsNullOrWhiteSpace(
                    profile.DisplayName)
                    ? "Teacher"
                    : profile.DisplayName.Trim();


            Initials =
                CreateInitials(
                    FullName); // (Microsoft, 2026)


            Position =
                string.Equals(
                    profile.Role,
                    "teacher",
                    StringComparison.OrdinalIgnoreCase)
                    ? "Teacher"
                    : profile.Role;


            if (string.IsNullOrWhiteSpace(
                    Position))
            {
                Position =
                    "Teacher";
            }


            EmployeeNumber =
                string.IsNullOrWhiteSpace(
                    profile.StaffId)
                    ? "Not assigned"
                    : profile.StaffId;


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
            IsLoading = // (Microsoft, 2026)
                false;
        }
    }


    private static string CreateInitials(
        string fullName)
    {
        if (string.IsNullOrWhiteSpace(
                fullName))
        {
            return "T";
        }


        var names =
            fullName.Split(
                ' ',
                StringSplitOptions
                    .RemoveEmptyEntries);


        if (names.Length == 0)
        {
            return "T";
        }


        if (names.Length == 1)
        {
            return names[0][..1]
                .ToUpperInvariant(); // (Microsoft, 2026)
        }


        var firstInitial =
            names[0][..1]
                .ToUpperInvariant();


        var lastInitial =
            names[^1][..1]
                .ToUpperInvariant();


        return
            $"{firstInitial}{lastInitial}"; // (Microsoft, 2026)
    }
}



//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].

