using Google.Cloud.Firestore;

namespace KoosMatliAdminApi.Models;

[FirestoreData]
public class AdmissionApplication
{
    public AdmissionApplication() { }

    [FirestoreProperty]
    public int Id { get; set; }

    [FirestoreProperty]
    public string ApplicationNumber { get; set; } = string.Empty;
    [FirestoreProperty]
    public string LearnerFirstName { get; set; } = string.Empty;
    [FirestoreProperty]
    public string LearnerLastName { get; set; } = string.Empty;
    [FirestoreProperty(ConverterType = typeof(KoosMatliAdminApi.Services.UtcDateTimeConverter))]
    public DateTime DateOfBirth { get; set; }

    [FirestoreProperty]
    public string GradeApplyingFor { get; set; } = string.Empty;
    [FirestoreProperty]
    public string GuardianName { get; set; } = string.Empty;
    [FirestoreProperty]
    public string GuardianEmail { get; set; } = string.Empty;
    [FirestoreProperty]
    public string GuardianPhone { get; set; } = string.Empty;
    [FirestoreProperty(ConverterType = typeof(KoosMatliAdminApi.Services.UtcDateTimeConverter))]
    public DateTime DateSubmitted { get; set; }

    [FirestoreProperty]
    public string Status { get; set; } = "New";
    public string LearnerFullName => $"{LearnerFirstName} {LearnerLastName}";
}
