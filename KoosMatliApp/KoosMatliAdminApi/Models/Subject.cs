using Google.Cloud.Firestore;

namespace KoosMatliAdminApi.Models;

[FirestoreData]
public class Subject
{
    public Subject() { }

    [FirestoreProperty]
    public int Id { get; set; }

    [FirestoreProperty]
    public string Name { get; set; } = string.Empty;
    [FirestoreProperty]
    public string Grade { get; set; } = string.Empty;
    [FirestoreProperty]
    public List<string> TeacherNames { get; set; } = new();
}
