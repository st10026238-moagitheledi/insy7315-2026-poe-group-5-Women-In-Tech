using CommunityToolkit.Mvvm.ComponentModel;
using KoosMatliApp.Features.Teacher.Services;

namespace KoosMatliApp.Features.Teacher.ViewModels;

public class ProfileHubViewModel : ObservableObject
{
    private string _fullName = string.Empty;

    public string FullName
    {
        get => _fullName;
        set => SetProperty(
            ref _fullName,
            value);
    }


    private string _initials = string.Empty;

    public string Initials
    {
        get => _initials;
        set => SetProperty(
            ref _initials,
            value);
    }


    private string _position = string.Empty;

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
    }


    public void RefreshProfile()
    {
        var profile = TeacherProfileService.GetProfile();


        FullName =
            $"{profile.FirstName} {profile.LastName}".Trim();


        var firstInitial =
            string.IsNullOrWhiteSpace(profile.FirstName)
                ? string.Empty
                : profile.FirstName[..1].ToUpper();


        var lastInitial =
            string.IsNullOrWhiteSpace(profile.LastName)
                ? string.Empty
                : profile.LastName[..1].ToUpper();


        Initials = $"{firstInitial}{lastInitial}";


        Position = profile.Position;


        EmployeeNumber = profile.EmployeeNumber;
    }
}
