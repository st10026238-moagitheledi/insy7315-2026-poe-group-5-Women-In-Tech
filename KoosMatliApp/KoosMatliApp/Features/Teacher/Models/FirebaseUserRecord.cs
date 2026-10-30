#if ANDROID
using Plugin.Firebase.Firestore;
#endif

namespace KoosMatliApp.Features.Teacher.Models;

public class FirebaseUserRecord
#if ANDROID
    : IFirestoreObject
#endif
{
#if ANDROID
    [FirestoreProperty("createdAt")]
#endif
    public string CreatedAt { get; set; } =
        string.Empty;


#if ANDROID
    [FirestoreProperty("displayName")]
#endif
    public string DisplayName { get; set; } =
        string.Empty;


#if ANDROID
    [FirestoreProperty("email")]
#endif
    public string Email { get; set; } =
        string.Empty;


#if ANDROID
    [FirestoreProperty("isActive")]
#endif
    public bool IsActive { get; set; }


#if ANDROID
    [FirestoreProperty("role")]
#endif
    public string Role { get; set; } =
        string.Empty;


#if ANDROID
    [FirestoreProperty("uid")]
#endif
    public string Uid { get; set; } =
        string.Empty;
}
