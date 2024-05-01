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
                    Dictionary<string, object> postDict = new Dictionary<string, object>();
                    var post = new Post
                    {
                        PostID = postDict.ContainsKey("PostId") ? Convert.ToInt32(postDict["PostId"]) : 0,
                        UserID = postDict.ContainsKey("userID") ? Convert.ToInt32(postDict["userID"]) : 0,
                        Title = postDict.ContainsKey("Title") ? postDict["Title"].ToString() : null,
                        Description = postDict.ContainsKey("Description") ? postDict["Description"].ToString() : null,
                        Date = postDict.ContainsKey("Date") ? DateTime.Parse(postDict["Date"].ToString()) : DateTime.MinValue,
                        Participants = postDict.ContainsKey("Participants") ? Convert.ToInt32(postDict["Participants"]) : 0,
                        Images = postDict.ContainsKey("Images") ? postDict["Images"].ToString() : null,
                        Status = postDict.ContainsKey("Status") ? postDict["Status"].ToString() : null
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}