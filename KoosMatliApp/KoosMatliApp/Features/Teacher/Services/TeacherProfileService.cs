using KoosMatliApp.Features.Teacher.Models;

namespace KoosMatliApp.Features.Teacher.Services;

public static class TeacherProfileService
{
    private static readonly TeacherProfile _profile =
        new()
        {
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane.doe@koosmatli.co.za",
            PhoneNumber = "071 234 5678",
            EmployeeNumber = "TCH001",
            Position = "Teacher",
            Department = "Intermediate Phase"
        };


    public static TeacherProfile GetProfile()
    {
        return _profile;
    }


    public static void UpdateProfile(
        string firstName,
        string lastName,
        string email,
        string phoneNumber)
    {
        _profile.FirstName =
            firstName;

        _profile.LastName =
            lastName;

        _profile.Email =
            email;

        _profile.PhoneNumber =
            phoneNumber;
    }
}