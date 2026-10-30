using Google.Cloud.Firestore;

namespace KoosMatliAdminApi.Models;

[FirestoreData]
public class SchoolClass
{
    public SchoolClass() { }

    [FirestoreProperty]
    public int Id { get; set; }

    [FirestoreProperty]
    public string ClassName { get; set; } = string.Empty;
    [FirestoreProperty]
    public string Grade { get; set; } = string.Empty;
    [FirestoreProperty]
    public string TeacherName { get; set; } = string.Empty;
    [FirestoreProperty]
    public int LearnerCount { get; set; }

}
