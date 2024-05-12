using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Newtonsoft.Json;

using SoframiPaylas.Domain.Entities;

using SoframiPaylas.Infrastructure.Data.Service;
using SoframiPaylas.Infrastructure.Interfaces;

namespace SoframiPaylas.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly FirestoreDb db;
        private readonly FirebaseService firebaseService;
        public PostRepository(FirebaseService service)
        {
            db = firebaseService.GetDb();
            firebaseService = service;
        }

        public async Task<string> CreatePostAsync(Post post)
        {
            return await firebaseService.ExecuteFirestoreOperationAsync(async () =>
            {
                string postId = Guid.NewGuid().ToString();
                await db.Collection("Posts").Document(postId).SetAsync(post);
                return postId;
            }, TimeSpan.FromSeconds(20));
        }

        public async Task<bool> DeletePostAsync(string id)
        {
            return await firebaseService.ExecuteFirestoreOperationAsync(async () =>
                       {
                           if (string.IsNullOrEmpty(id))
                               throw new ArgumentException("Post ID cannot be null or empty.", nameof(id));

                           DocumentReference postRef = db.Collection("Posts").Document(id);

                           // Firestore'dan belirtilen kullanıcıyı silme
                           try
                           {
                               await postRef.DeleteAsync();
                               return true;  // Silme işlemi başarılı
                           }
                           catch (Exception ex)
                           {
                               // Loglama işlemi burada yapılabilir
                               return false;  // Silme işlemi başarısız
                           }
                       }, TimeSpan.FromSeconds(20));
        }

        public async Task<List<Post>> GetPostAllAsync()
        {
            return await firebaseService.ExecuteFirestoreOperationAsync(async () =>
            {
                if (db == null)
                    throw new InvalidOperationException("Database service is not initialized properly.");
                CollectionReference postRef = db.Collection("Posts");
                QuerySnapshot snapshots = await postRef.GetSnapshotAsync();

                List<Post> posts = new List<Post>();
                foreach (DocumentSnapshot document in snapshots.Documents)
                {
                    if (document.Exists)
                    {
                        Dictionary<string, object> postDict = document.ToDictionary();
                        var postItem = new Post
                        {
                            HostID = postDict.ContainsKey("hostID") ? postDict["hostID"].ToString() : null,
                            Title = postDict.ContainsKey("title") ? postDict["title"].ToString() : null,
                            Description = postDict.ContainsKey("description") ? postDict["description"].ToString() : null,
                            Location = postDict.ContainsKey("location") ? (GeoPoint)postDict["location"] : new GeoPoint(0, 0),
                            Date = postDict.ContainsKey("date") ? (Timestamp)postDict["date"] : new Timestamp(),
                            Time = postDict.ContainsKey("time") ? postDict["time"].ToString() : null,
                            MaxParticipants = postDict.ContainsKey("maxParticipants") ? Convert.ToInt32(postDict["maxParticipants"]) : 0,
                            Images = postDict.ContainsKey("images") ? ((List<object>)postDict["images"]).ConvertAll(obj => obj.ToString()) : new List<string>(),
                            PostStatus = postDict.ContainsKey("eventStatus") ? Convert.ToBoolean(postDict["eventStatus"]) : false,
                            RelatedFoods = postDict.ContainsKey("relatedFoods") && postDict["relatedFoods"] is List<string> ? (List<string>)postDict["relatedFoods"] : null,
                            Participants = postDict.ContainsKey("participants") && postDict["participants"] is List<string> ? (List<string>)postDict["participants"] : null
                        };

                        posts.Add(postItem);
                    }
                }

                return posts;
            }, TimeSpan.FromSeconds(20));
        }

        public async Task<Post> GetPostByIdAsync(string id)
        {
            return await firebaseService.ExecuteFirestoreOperationAsync(async () =>
            {
                if (db == null)
                {
                    throw new InvalidOperationException("Database service is not initialized.");
                }
                DocumentReference postRef = db.Collection("Posts").Document(id);
                DocumentSnapshot snapshot = await postRef.GetSnapshotAsync();


                if (!snapshot.Exists)
                {
                    return null;
                }

                Dictionary<string, object> postDict = snapshot.ToDictionary();
                return new Post
                {
                    HostID = postDict.ContainsKey("hostID") ? postDict["hostID"].ToString() : null,
                    Title = postDict.ContainsKey("title") ? postDict["title"].ToString() : null,
                    Description = postDict.ContainsKey("description") ? postDict["description"].ToString() : null,
                    Location = postDict.ContainsKey("location") ? (GeoPoint)postDict["location"] : new GeoPoint(0, 0),
                    Date = postDict.ContainsKey("date") ? (Timestamp)postDict["date"] : new Timestamp(),
                    Time = postDict.ContainsKey("time") ? postDict["time"].ToString() : null,
                    MaxParticipants = postDict.ContainsKey("maxParticipants") ? Convert.ToInt32(postDict["maxParticipants"]) : 0,
                    Images = postDict.ContainsKey("images") ? ((List<object>)postDict["images"]).ConvertAll(obj => obj.ToString()) : new List<string>(),
                    PostStatus = postDict.ContainsKey("eventStatus") ? Convert.ToBoolean(postDict["eventStatus"]) : false,
                    RelatedFoods = postDict.ContainsKey("relatedFoods") && postDict["relatedFoods"] is List<string> ? (List<string>)postDict["relatedFoods"] : null,
                    Participants = postDict.ContainsKey("participants") && postDict["participants"] is List<string> ? (List<string>)postDict["participants"] : null
                };
            }, TimeSpan.FromSeconds(20));
        }

        public async Task<bool> UpdatePostAsync(string id, Post postItem)
        {
            return await firebaseService.ExecuteFirestoreOperationAsync(async () =>
            {
                if (postItem == null)
                    throw new ArgumentNullException(nameof(postItem), "Post object must not be null.");
                if (string.IsNullOrEmpty(id))
                    throw new ArgumentException("Post ID must not be null or empty.", nameof(id));

                DocumentReference postReference = db.Collection("Posts").Document(id);
                await postReference.SetAsync(postItem, SetOptions.MergeAll);
                try
                {
                    await postReference.SetAsync(postItem, SetOptions.MergeAll);
                    return true;  // Güncelleme işlemi başarılı
                }
                catch (Exception ex)
                {
                    // Loglama işlemi burada yapılabilir
                    return false;  // Güncelleme işlemi başarısız
                }
            }, TimeSpan.FromSeconds(20));
        }

    }
}