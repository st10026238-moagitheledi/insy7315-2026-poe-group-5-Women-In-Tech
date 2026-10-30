using Google.Cloud.Firestore;

namespace KoosMatliAdminApi.Models;

[FirestoreData]
public class User
{
    public User() { }

    [FirestoreProperty]
    public int Id { get; set; }

    [FirestoreProperty]
    public string FirstName { get; set; } = string.Empty;
    [FirestoreProperty]
    public string LastName { get; set; } = string.Empty;
    [FirestoreProperty]
    public string Email { get; set; } = string.Empty;
    [FirestoreProperty]
    public string PhoneNumber { get; set; } = string.Empty;
    [FirestoreProperty]
    public string Role { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
}
