namespace SoframiPaylas.Domain.Entities;
using Google.Cloud.Firestore;

[FirestoreData]
public class MessageStore
{
    [FirestoreProperty]
    public string SenderId { get; set; }

    [FirestoreProperty]
    public string ReceiverId { get; set; }

    [FirestoreProperty]
    public string Content { get; set; }

    [FirestoreProperty]
    public Timestamp Timestamp { get; set; }
}
