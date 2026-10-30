using CommunityToolkit.Mvvm.ComponentModel;
using KoosMatliApp.Features.Teacher.Services;

namespace KoosMatliApp.Features.Teacher.ViewModels;

public class TeacherProfileViewModel : ObservableObject
{
    private string _firstName = string.Empty;

    public string FirstName
    {
        get => _firstName;
        set
        {
            if (SetProperty(ref _firstName, value))
            {
                RefreshDisplayProperties();
            }
        }
    }


    private string _lastName = string.Empty;

    public string LastName
    {
        get => _lastName;
        set
        {
            if (SetProperty(ref _lastName, value))
            {
                RefreshDisplayProperties();
            }
        }
    }


    private string _email = string.Empty;

    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }


    private string _phoneNumber = string.Empty;

    public string PhoneNumber
    {
        get => _phoneNumber;
        set => SetProperty(ref _phoneNumber, value);
    }


    private string _employeeNumber = string.Empty;

    public string EmployeeNumber
    {
        get => _employeeNumber;
        set => SetProperty(ref _employeeNumber, value);
    }


    private string _position = string.Empty;

    public string Position
    {
        get => _position;
        set => SetProperty(ref _position, value);
    }


    private string _department = string.Empty;

    public string Department
    {
        get => _department;
        set => SetProperty(ref _department, value);
    }


    public string FullName =>
        $"{FirstName} {LastName}".Trim();


    public string Initials
    {
        get
        {
            var firstInitial =
                string.IsNullOrWhiteSpace(FirstName)
                    ? string.Empty
                    : FirstName[..1].ToUpper();

            var lastInitial =
                string.IsNullOrWhiteSpace(LastName)
                    ? string.Empty
                    : LastName[..1].ToUpper();

            return $"{firstInitial}{lastInitial}";
        }
    }


    public TeacherProfileViewModel()
    {
        LoadProfile();
    }


    public void LoadProfile()
    {
        var profile =
            TeacherProfileService.GetProfile();


        FirstName = profile.FirstName;

        LastName = profile.LastName;

        Email = profile.Email;

        PhoneNumber = profile.PhoneNumber;

        EmployeeNumber = profile.EmployeeNumber;

        Position = profile.Position;

        Department = profile.Department;


        RefreshDisplayProperties();
    }


    public void SaveProfile()
    {
        TeacherProfileService.UpdateProfile(
        FirstName.Trim(),
        LastName.Trim(),
        Email.Trim(),
        PhoneNumber.Trim());


        RefreshDisplayProperties();
    }


    private void RefreshDisplayProperties()
    {
        OnPropertyChanged(
            nameof(FullName));

        OnPropertyChanged(
            nameof(Initials));
    }
}
