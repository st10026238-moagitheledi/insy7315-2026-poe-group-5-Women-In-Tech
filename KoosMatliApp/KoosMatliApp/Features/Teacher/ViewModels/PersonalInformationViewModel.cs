using CommunityToolkit.Mvvm.ComponentModel;
using KoosMatliApp.Features.Teacher.Services;

namespace KoosMatliApp.Features.Teacher.ViewModels;

public class PersonalInformationViewModel : ObservableObject
{
    private string _firstName =
        string.Empty;

    public string FirstName
    {
        get => _firstName;
        set => SetProperty(ref _firstName, value);
    }


    private string _lastName =
        string.Empty;

    public string LastName
    {
        get => _lastName;
        set => SetProperty(ref _lastName, value);
    }


    private string _email =
        string.Empty;

    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }


    private string _phoneNumber =
        string.Empty;

    public string PhoneNumber
    {
        get => _phoneNumber;
        set => SetProperty(ref _phoneNumber, value);
    }


    private string _employeeNumber =
        string.Empty;

    public string EmployeeNumber
    {
        get => _employeeNumber;
        set => SetProperty(ref _employeeNumber, value);
    }


    private string _position =
        string.Empty;

    public string Position
    {
        get => _position;
        set => SetProperty(ref _position, value);
    }


    private string _department =
        string.Empty;

    public string Department
    {
        get => _department;
        set => SetProperty(ref _department, value);
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
    }


    public void SaveProfile()
    {
        TeacherProfileService.UpdateProfile(
            FirstName.Trim(),
            LastName.Trim(),
            Email.Trim(),
            PhoneNumber.Trim());
    }
}
