using Google.Cloud.Firestore;
using SoframiPaylas.Domain.Entities;
using SoframiPaylas.Infrastructure.Data.Service;
using SoframiPaylas.Infrastructure.Interfaces;

namespace SoframiPaylas.Infrastructure.Repositories;
public class CommentRepository : ICommentRepository
{
    private readonly FirestoreDb db;
    private readonly FirebaseService firebaseService;
    public CommentRepository(FirebaseService service)
    {
        db = service.GetDb();
        firebaseService = service;
    }
    public async Task<List<(Comment comment, string commentId)>> GetCommentsByPostIdAsync(string postId)
    {
        return await firebaseService.ExecuteFirestoreOperationAsync(async () =>
        {
            if (string.IsNullOrEmpty(postId))
                throw new ArgumentException("Post ID cannot be null or empty.", nameof(postId));

            Query commentQuery = db.Collection("Comments").WhereEqualTo("PostId", postId);
            QuerySnapshot commentQuerySnapshot = await commentQuery.GetSnapshotAsync();
            List<(Comment comment, string commentId)> comments = new List<(Comment comment, string commentId)>();

            foreach (DocumentSnapshot documentSnapshot in commentQuerySnapshot.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Comment comment = documentSnapshot.ConvertTo<Comment>();
                    var id = documentSnapshot.Id;
                    comments.Add((comment, id));
                }
            }
            return comments;
        }, TimeSpan.FromSeconds(20));
    }

    public async Task<(Comment comment, string commentId)> AddCommentAsync(Comment comment)
    {
        return await firebaseService.ExecuteFirestoreOperationAsync(async () =>
        {
            if (comment == null)
                throw new ArgumentNullException(nameof(comment), "Comment object must not be null.");

            DocumentReference docRef = await db.Collection("Comments").AddAsync(comment);
            var id = docRef.Id;
            return (comment, id);
        }, TimeSpan.FromSeconds(20));
    }

    public async Task<bool> DeleteCommentAsync(string commentId)
    {
        return await firebaseService.ExecuteFirestoreOperationAsync(async () =>
        {
            if (string.IsNullOrEmpty(commentId))
                throw new ArgumentException("Comment ID cannot be null or empty.", nameof(commentId));

            DocumentReference commentRef = db.Collection("Comments").Document(commentId);
            try
            {
                await commentRef.DeleteAsync();
                return true;
            }
            catch (Exception ex)
            {

                return false;  // Silme işlemi başarısız
            }
        }, TimeSpan.FromSeconds(20));
    }
}