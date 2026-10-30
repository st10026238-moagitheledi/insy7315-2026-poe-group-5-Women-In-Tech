using Google.Cloud.Firestore;
namespace KoosMatliAdminApi.Models;
[FirestoreData]
public sealed class UserSettings
{
    [FirestoreProperty] public string DisplayName { get; set; } = "";
    [FirestoreProperty] public string PhoneNumber { get; set; } = "";
    [FirestoreProperty] public bool ConfirmSensitiveActions { get; set; } = true;
    [FirestoreProperty] public string Language { get; set; } = "en-ZA";
    [FirestoreProperty] public string Theme { get; set; } = "System";
    [FirestoreProperty] public bool NotificationsEnabled { get; set; } = true;
}
