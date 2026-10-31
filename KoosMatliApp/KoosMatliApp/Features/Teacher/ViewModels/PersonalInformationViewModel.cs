using CommunityToolkit.Mvvm.ComponentModel;
using KoosMatliApp.Features.Teacher.Services;

namespace KoosMatliApp.Features.Teacher.ViewModels;

public class PersonalInformationViewModel : ObservableObject
{
    private readonly TeacherApiService
        _teacherApiService;


    public PersonalInformationViewModel(
        TeacherApiService teacherApiService)
    {
        _teacherApiService =
            teacherApiService;
    } // (Microsoft, 2026)


    private string _firstName =
        string.Empty;

    public string FirstName
    {
        get => _firstName;
        set => SetProperty(
            ref _firstName,
            value);
    }


    private string _lastName =
        string.Empty;

    public string LastName
    {
        get => _lastName;
        set => SetProperty(
            ref _lastName,
            value);
    }


    private string _email =
        string.Empty;

    public string Email
    {
        get => _email;
        set => SetProperty(
            ref _email,
            value);
    } // (Microsoft, 2026)


    private string _phoneNumber =
        string.Empty;

    public string PhoneNumber
    {
        get => _phoneNumber;
        set => SetProperty(
            ref _phoneNumber,
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
    }


    private string _position =
        string.Empty;

    public string Position
    {
        get => _position;
        set => SetProperty(
            ref _position,
            value);
    } // (Microsoft, 2026)


    private string _department =
        string.Empty;

    public string Department
    {
        get => _department;
        set => SetProperty(
            ref _department,
            value);
    }


    public string ErrorMessage { get; private set; } =
        string.Empty;


    public async Task<bool> LoadProfileAsync()
    {
        try
        {
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
                    "Unable to load your profile.";

                return false;
            } // (Microsoft, 2026)


            var profile =
                response.Profile;


            var nameParts =
                profile.DisplayName.Split(
                    ' ',
                    2,
                    StringSplitOptions
                        .RemoveEmptyEntries);


            FirstName =
                nameParts.Length > 0
                    ? nameParts[0]
                    : string.Empty;


            LastName =
                nameParts.Length > 1
                    ? nameParts[1]
                    : string.Empty;


            Email =
                profile.Email;


            PhoneNumber =
                profile.PhoneNumber;


            EmployeeNumber =
                profile.StaffId;


            Position =
                string.Equals(
                    profile.Role,
                    "teacher",
                    StringComparison.OrdinalIgnoreCase)
                    ? "Teacher"
                    : profile.Role;


            Department =
                string.Empty;


            return true;
        }
        catch (Exception ex)
        {
            ErrorMessage =
                ex.Message;

            return false;
        } // (Microsoft, 2026)
    }


    public async Task<bool> SaveProfileAsync()
    {
        try
        {
            ErrorMessage =
                string.Empty;


            var displayName =
                $"{FirstName.Trim()} {LastName.Trim()}"
                    .Trim();


            var response =
                await _teacherApiService
                    .UpdateProfileAsync(
                        displayName,
                        PhoneNumber.Trim());


            if (response == null ||
                !response.Success)
            {
                ErrorMessage =
                    response?.Message ??
                    "Unable to update your profile.";

                return false;
            } // (Microsoft, 2026)


            return true;
        }
        catch (Exception ex)
        {
            ErrorMessage =
                ex.Message;

            return false;
        }
    }
}



//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].

