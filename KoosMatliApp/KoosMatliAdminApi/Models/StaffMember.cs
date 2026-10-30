using Google.Cloud.Firestore;

namespace KoosMatliAdminApi.Models;

[FirestoreData]
public class StaffMember
{
    public StaffMember() { }

    [FirestoreProperty]
    public int Id { get; set; }

    [FirestoreProperty]
    public string FirstName { get; set; } = string.Empty;
    [FirestoreProperty]
    public string LastName { get; set; } = string.Empty;
    [FirestoreProperty]
    public string Email { get; set; } = string.Empty;
    [FirestoreProperty]
    public string Position { get; set; } = string.Empty;
    [FirestoreProperty]
    public List<string> Subjects { get; set; } = new();
    [FirestoreProperty]
    public List<string> Classes { get; set; } = new();
    public string FullName => $"{FirstName} {LastName}";
}
