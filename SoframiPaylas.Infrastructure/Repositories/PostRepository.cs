using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using SoframiPaylas.Domain.Entities;
using SoframiPaylas.Infrastructure.Data.Service;
using SoframiPaylas.Infrastructure.Interfaces;

namespace SoframiPaylas.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly FirebaseService _service;
        public PostRepository(FirebaseService service)
        {
            _service = service;
        }

        public async Task<string> CreatePostAsync(Post post)
        {
            string postId = Guid.NewGuid().ToString();

            await _service.GetDb().Collection("Posts").Document(postId).SetAsync(post);

            return postId;
        }

        public async Task DeletePostAsync(string postId)
        {
            if (string.IsNullOrEmpty(postId))
                throw new ArgumentException("Post ID cannot be null or empty.", nameof(postId));

            DocumentReference postRef = _service.GetDb().Collection("Posts").Document(postId);

            // Firestore'dan belirtilen kullanıcıyı silme
            await postRef.DeleteAsync();
        }

        public async Task<List<Post>> GetAllPostsAsync()
        {
            if (_service == null || _service.GetDb() == null)
                throw new InvalidOperationException("Database service is not initialized properly.");
            CollectionReference postRef = _service.GetDb().Collection("Posts");
            QuerySnapshot snapshots = await postRef.GetSnapshotAsync();

            List<Post> posts = new List<Post>();
            foreach (DocumentSnapshot document in snapshots.Documents)
            {

                if (document.Exists)
                {
                    Dictionary<string, object> postDict = document.ToDictionary();
                    var post = new Post
                    {
                        UserID = postDict.ContainsKey("userID") ? postDict["userID"].ToString() : null,
                        Title = postDict.ContainsKey("Title") ? postDict["Title"].ToString() : null,
                        Description = postDict.ContainsKey("Description") ? postDict["Description"].ToString() : null,
                        Date = postDict.ContainsKey("Date") && postDict["Date"] is Timestamp ? (Timestamp)postDict["Date"] : Timestamp.FromDateTime(DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc)),
                        Participants = postDict.ContainsKey("Participants") ? Convert.ToInt32(postDict["Participants"]) : 0,
                        Images = postDict.ContainsKey("Images") ? postDict["Images"].ToString() : null,
                        Status = postDict.ContainsKey("Status") ? postDict["Status"].ToString() : null
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }

        public async Task<Post> GetPostByIdAsync(string id)
        {
            if (_service == null || _service.GetDb() == null)
            {
                throw new InvalidOperationException("Database service is not initialized.");
            }

            DocumentReference eventRef = _service.GetDb().Collection("Posts").Document(id);
            DocumentSnapshot snapshot = await eventRef.GetSnapshotAsync();




            if (!snapshot.Exists)
            {
                return null;  // Eğer belge yoksa null döner
            }

            Dictionary<string, object> postDict = snapshot.ToDictionary();
            return new Post
            {
                UserID = postDict.ContainsKey("userID") ? postDict["userID"].ToString() : null,
                Title = postDict.ContainsKey("Title") ? postDict["Title"].ToString() : null,
                Description = postDict.ContainsKey("Description") ? postDict["Description"].ToString() : null,
                Date = postDict.ContainsKey("Date") && postDict["Date"] is Timestamp ? (Timestamp)postDict["Date"] : Timestamp.FromDateTime(DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc)),
                Participants = postDict.ContainsKey("Participants") ? Convert.ToInt32(postDict["Participants"]) : 0,
                Images = postDict.ContainsKey("Images") ? postDict["Images"].ToString() : null,
                Status = postDict.ContainsKey("Status") ? postDict["Status"].ToString() : null
            };
        }

        public async Task UpdatePostAsync(Post post, string postId)
        {
            if (post == null)
                throw new ArgumentNullException(nameof(post), "Post object must not be null.");
            if (string.IsNullOrEmpty(postId))
                throw new ArgumentException("Post ID must not be null or empty.", nameof(postId));

            DocumentReference postReference = _service.GetDb().Collection("Posts").Document(postId);
            await postReference.SetAsync(post, SetOptions.MergeAll);
        }
    }
}