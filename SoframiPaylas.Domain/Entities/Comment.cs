using Google.Cloud.Firestore;

namespace SoframiPaylas.Domain.Entities;
[FirestoreData]
public class Comment
{


    [FirestoreProperty]
    public string PostId { get; set; }

    [FirestoreProperty]
    public string UserId { get; set; }

    [FirestoreProperty]
    public string UserName { get; set; }

    [FirestoreProperty]
    public string Content { get; set; }

    [FirestoreProperty]
    public Timestamp CreatedAt { get; set; }
}