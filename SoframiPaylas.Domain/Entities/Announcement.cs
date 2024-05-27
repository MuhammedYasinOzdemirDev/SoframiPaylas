namespace SoframiPaylas.Domain.Entities;

using Google.Cloud.Firestore;

[FirestoreData]
public class Announcement
{
    [FirestoreProperty]
    public string Content { get; set; }

    [FirestoreProperty]
    public Timestamp Timestamp { get; set; }
}
