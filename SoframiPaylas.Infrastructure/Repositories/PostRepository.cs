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
            db = service.GetDb();
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

        public async Task<List<(Post post, string Id)>> GetPostAllAsync()
        {
            return await firebaseService.ExecuteFirestoreOperationAsync(async () =>
            {
                if (db == null)
                    throw new InvalidOperationException("Database service is not initialized properly.");
                CollectionReference postRef = db.Collection("Posts");
                QuerySnapshot snapshots = await postRef.GetSnapshotAsync();

                List<(Post post, string Id)> posts = new List<(Post post, string Id)>();
                foreach (DocumentSnapshot document in snapshots.Documents)
                {
                    if (document.Exists)
                    {
                        Dictionary<string, object> postDict = document.ToDictionary();
                        var id = document.Id;
                        var postItem = new Post
                        {
                            HostID = postDict.ContainsKey("hostID") ? postDict["hostID"].ToString() : null,
                            Title = postDict.ContainsKey("title") ? postDict["title"].ToString() : null,
                            Description = postDict.ContainsKey("description") ? postDict["description"].ToString() : null,
                            Longitude = postDict.ContainsKey("longitude") ? Convert.ToDouble(postDict["longitude"]) : 0,
                            Latitude = postDict.ContainsKey("latitude") ? Convert.ToDouble(postDict["latitude"]) : 0,
                            Date = postDict.ContainsKey("date") ? (Timestamp)postDict["date"] : new Timestamp(),
                            Time = postDict.ContainsKey("time") ? postDict["time"].ToString() : null,
                            MaxParticipants = postDict.ContainsKey("maxParticipants") ? Convert.ToInt32(postDict["maxParticipants"]) : 0,
                            Image = postDict.ContainsKey("image") ? postDict["image"].ToString() : null,
                            PostStatus = postDict.ContainsKey("eventStatus") ? Convert.ToBoolean(postDict["eventStatus"]) : false,
                            RelatedFoods = postDict.ContainsKey("relatedFoods") && postDict["relatedFoods"] is IEnumerable<object> relatedFoods
                        ? relatedFoods.Select(f => f.ToString()).ToList()
                        : new List<string>(),
                            Participants = postDict.ContainsKey("participants") && postDict["participants"] is IEnumerable<object> participants
                        ? participants.Select(f => f.ToString()).ToList()
                        : new List<string>()
                        };

                        posts.Add((postItem, id));
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
                    Longitude = postDict.ContainsKey("longitude") ? Convert.ToDouble(postDict["longitude"]) : 0,
                    Latitude = postDict.ContainsKey("latitude") ? Convert.ToDouble(postDict["latitude"]) : 0,
                    Date = postDict.ContainsKey("date") ? (Timestamp)postDict["date"] : new Timestamp(),
                    Time = postDict.ContainsKey("time") ? postDict["time"].ToString() : null,
                    MaxParticipants = postDict.ContainsKey("maxParticipants") ? Convert.ToInt32(postDict["maxParticipants"]) : 0,
                    Image = postDict.ContainsKey("image") ? postDict["image"].ToString() : null,
                    PostStatus = postDict.ContainsKey("eventStatus") ? Convert.ToBoolean(postDict["eventStatus"]) : false,
                    RelatedFoods = postDict.ContainsKey("relatedFoods") && postDict["relatedFoods"] is IEnumerable<object> relatedFoods
                        ? relatedFoods.Select(f => f.ToString()).ToList()
                        : new List<string>(),
                    Participants = postDict.ContainsKey("participants") && postDict["participants"] is IEnumerable<object> participants
                        ? participants.Select(f => f.ToString()).ToList()
                        : new List<string>()
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
        public async Task<List<(Post post, string postId)>> GetByUserIdPostAllAsync(string userId)
        {
            return await firebaseService.ExecuteFirestoreOperationAsync(async () =>
                {
                    if (db == null)
                        throw new InvalidOperationException("Database service is not initialized properly.");
                    Query query = db.Collection("Posts")
                                 .WhereEqualTo("hostID", userId);
                    QuerySnapshot snapshots = await query.GetSnapshotAsync();

                    List<(Post post, string Id)> posts = new List<(Post post, string Id)>();
                    foreach (DocumentSnapshot document in snapshots.Documents)
                    {
                        if (document.Exists)
                        {
                            Dictionary<string, object> postDict = document.ToDictionary();
                            var id = document.Id;
                            var postItem = new Post
                            {
                                HostID = postDict.ContainsKey("hostID") ? postDict["hostID"].ToString() : null,
                                Title = postDict.ContainsKey("title") ? postDict["title"].ToString() : null,
                                Description = postDict.ContainsKey("description") ? postDict["description"].ToString() : null,
                                Longitude = postDict.ContainsKey("longitude") ? Convert.ToDouble(postDict["longitude"]) : 0,
                                Latitude = postDict.ContainsKey("latitude") ? Convert.ToDouble(postDict["latitude"]) : 0,
                                Date = postDict.ContainsKey("date") ? (Timestamp)postDict["date"] : new Timestamp(),
                                Time = postDict.ContainsKey("time") ? postDict["time"].ToString() : null,
                                MaxParticipants = postDict.ContainsKey("maxParticipants") ? Convert.ToInt32(postDict["maxParticipants"]) : 0,
                                Image = postDict.ContainsKey("image") ? postDict["image"].ToString() : null,
                                PostStatus = postDict.ContainsKey("eventStatus") ? Convert.ToBoolean(postDict["eventStatus"]) : false,
                                RelatedFoods = postDict.ContainsKey("relatedFoods") && postDict["relatedFoods"] is IEnumerable<object> relatedFoods
                            ? relatedFoods.Select(f => f.ToString()).ToList()
                            : new List<string>(),
                                Participants = postDict.ContainsKey("participants") && postDict["participants"] is IEnumerable<object> participants
                            ? participants.Select(f => f.ToString()).ToList()
                            : new List<string>()
                            };

                            posts.Add((postItem, id));
                        }
                    }

                    return posts;
                }, TimeSpan.FromSeconds(20));
        }

        public async Task<List<(Post post, string postId)>> GetPostsByIdsAsync(List<string> postIds)
        {
            return await firebaseService.ExecuteFirestoreOperationAsync(async () =>
            {
                if (db == null)
                    throw new InvalidOperationException("Database service is not initialized properly.");

                var posts = new List<(Post post, string postId)>();

                foreach (var postId in postIds)
                {
                    DocumentReference docRef = db.Collection("Posts").Document(postId);
                    DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

                    if (snapshot.Exists)
                    {
                        Dictionary<string, object> postDict = snapshot.ToDictionary();
                        var id = snapshot.Id;
                        var postItem = new Post
                        {
                            HostID = postDict.ContainsKey("hostID") ? postDict["hostID"].ToString() : null,
                            Title = postDict.ContainsKey("title") ? postDict["title"].ToString() : null,
                            Description = postDict.ContainsKey("description") ? postDict["description"].ToString() : null,
                            Longitude = postDict.ContainsKey("longitude") ? Convert.ToDouble(postDict["longitude"]) : 0,
                            Latitude = postDict.ContainsKey("latitude") ? Convert.ToDouble(postDict["latitude"]) : 0,
                            Date = postDict.ContainsKey("date") ? (Timestamp)postDict["date"] : new Timestamp(),
                            Time = postDict.ContainsKey("time") ? postDict["time"].ToString() : null,
                            MaxParticipants = postDict.ContainsKey("maxParticipants") ? Convert.ToInt32(postDict["maxParticipants"]) : 0,
                            Image = postDict.ContainsKey("image") ? postDict["image"].ToString() : null,
                            PostStatus = postDict.ContainsKey("eventStatus") ? Convert.ToBoolean(postDict["eventStatus"]) : false,
                            RelatedFoods = postDict.ContainsKey("relatedFoods") && postDict["relatedFoods"] is IEnumerable<object> relatedFoods
                        ? relatedFoods.Select(f => f.ToString()).ToList()
                        : new List<string>(),
                            Participants = postDict.ContainsKey("participants") && postDict["participants"] is IEnumerable<object> participants
                        ? participants.Select(f => f.ToString()).ToList()
                        : new List<string>()
                        };

                        posts.Add((postItem, id));
                    }
                }

                return posts;
            }, TimeSpan.FromSeconds(20));
        }
        public async Task<bool> UpdateParticipantStatus(string postId, string participantId)
        {
            return await firebaseService.ExecuteFirestoreOperationAsync(async () =>
            {
                DocumentReference postRef = db.Collection("Posts").Document(postId);
                DocumentSnapshot snapshot = await postRef.GetSnapshotAsync();

                if (!snapshot.Exists)
                {
                    return false;
                }

                // participants listesine participantId'yi ekle
                Dictionary<string, object> updates = new Dictionary<string, object>
            {
            { "participants", FieldValue.ArrayUnion(participantId) }
            };

                await postRef.UpdateAsync(updates);
                return true;
            }, TimeSpan.FromSeconds(20));

        }
        public async Task<bool> RemoveParticipant(string participantId)
        {
            return await firebaseService.ExecuteFirestoreOperationAsync(async () =>
            {
                DocumentReference participantRef = db.Collection("Participants").Document(participantId);
                DocumentSnapshot snapshot = await participantRef.GetSnapshotAsync();

                if (!snapshot.Exists)
                {
                    return false;
                }
                var participant = snapshot.ConvertTo<Participant>();
                DocumentReference postRef = db.Collection("Posts").Document(participant.PostId);
                DocumentSnapshot snapshot2 = await postRef.GetSnapshotAsync();
                // participants listesinden participantId'yi çıkar
                Dictionary<string, object> updates = new Dictionary<string, object>
                {
            { "participants", FieldValue.ArrayRemove(participantId) }
                };

                await postRef.UpdateAsync(updates);
                return true;
            }, TimeSpan.FromSeconds(20));
        }
        public async Task<bool> EndPost(string postId)
        {
            return await firebaseService.ExecuteFirestoreOperationAsync(async () =>
            {
                DocumentReference postRef = db.Collection("Posts").Document(postId);
                DocumentSnapshot snapshot = await postRef.GetSnapshotAsync();

                if (!snapshot.Exists)
                {
                    return false;
                }

                Dictionary<string, object> updates = new Dictionary<string, object>
            {
            { "eventStatus", false }
            };

                await postRef.UpdateAsync(updates);
                return true;
            }, TimeSpan.FromSeconds(20));
        }
        public async Task<List<(Announcement announcement, string id)>> GetPostIds(List<string> postIds)
        {
            return await firebaseService.ExecuteFirestoreOperationAsync(async () =>
            {
                if (db == null)
                    throw new InvalidOperationException("Database service is not initialized properly.");

                var announcements = new List<(Announcement announcement, string id)>();

                foreach (var postId in postIds)
                {
                    Query docRef = db.Collection("Announcements").WhereEqualTo("PostId", postId);
                    QuerySnapshot snapshot = await docRef.GetSnapshotAsync();

                    if (snapshot.Count > 0)
                    {
                        foreach (var doc in snapshot.Documents)
                        {
                            Dictionary<string, object> postDict = doc.ToDictionary();
                            var id = doc.Id;
                            var announcementItem = new Announcement
                            {
                                Content = postDict.ContainsKey("Content") ? postDict["Content"].ToString() : null,
                                Timestamp = postDict.ContainsKey("Timestamp") ? (Timestamp)postDict["Timestamp"] : new Timestamp(),
                                PostId = postDict.ContainsKey("PostId") ? postDict["PostId"].ToString() : null
                            };

                            announcements.Add((announcementItem, id));
                        }
                    }
                }

                return announcements;
            }, TimeSpan.FromSeconds(20));
        }
    }
}