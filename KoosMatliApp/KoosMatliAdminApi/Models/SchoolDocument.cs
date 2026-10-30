using Google.Cloud.Firestore;

namespace KoosMatliAdminApi.Models;

[FirestoreData]
public class SchoolDocument
{
    public SchoolDocument() { }

    [FirestoreProperty]
    public int Id { get; set; }

    [FirestoreProperty]
    public string Title { get; set; } = string.Empty;
    [FirestoreProperty]
    public string FileName { get; set; } = string.Empty;
    [FirestoreProperty]
    public string FileUrl { get; set; } = string.Empty;
    [FirestoreProperty(ConverterType = typeof(KoosMatliAdminApi.Services.UtcDateTimeConverter))]
    public DateTime UploadedDate { get; set; }

    [FirestoreProperty]
    public string Category { get; set; } = string.Empty;
}
