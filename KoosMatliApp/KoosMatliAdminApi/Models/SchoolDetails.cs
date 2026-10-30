using Google.Cloud.Firestore;

namespace KoosMatliAdminApi.Models;

[FirestoreData]
public class SchoolDetails
{
    public SchoolDetails() { }

    [FirestoreProperty]
    public int Id { get; set; }

    [FirestoreProperty]
    public string SchoolName { get; set; } = string.Empty;
    [FirestoreProperty]
    public string Address { get; set; } = string.Empty;
    [FirestoreProperty]
    public string PhoneNumber { get; set; } = string.Empty;
    [FirestoreProperty]
    public string Email { get; set; } = string.Empty;
    [FirestoreProperty]
    public string PrincipalName { get; set; } = string.Empty;
}
