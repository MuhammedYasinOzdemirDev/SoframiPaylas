namespace SoframiPaylas.Infrastructure.Repositories;
using Google.Cloud.Firestore;
using SoframiPaylas.Domain.Entities;
using SoframiPaylas.Infrastructure.Data.Service;
using SoframiPaylas.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AnnouncementRepository : IAnnouncementRepository
{
    private readonly FirestoreDb _db;
    private readonly FirebaseService _firebaseService;

    public AnnouncementRepository(FirebaseService service)
    {
        _db = service.GetDb();
        _firebaseService = service;
    }

    public async Task<(Announcement announcement, string announcementId)> AddAnnouncementAsync(Announcement announcement)
    {
        return await _firebaseService.ExecuteFirestoreOperationAsync(async () =>
        {
            if (announcement == null)
                throw new ArgumentNullException(nameof(announcement), "Announcement object must not be null.");

            DocumentReference docRef = await _db.Collection("Announcements").AddAsync(announcement);
            var id = docRef.Id;
            return (announcement, id);
        }, TimeSpan.FromSeconds(20));
    }

    public async Task<List<(Announcement announcement, string announcementId)>> GetAnnouncementsAsync()
    {
        return await _firebaseService.ExecuteFirestoreOperationAsync(async () =>
        {
            QuerySnapshot snapshot = await _db.Collection("Announcements").GetSnapshotAsync();
            List<(Announcement announcement, string announcementId)> announcements = new List<(Announcement announcement, string announcementId)>();

            foreach (DocumentSnapshot documentSnapshot in snapshot.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Announcement announcement = documentSnapshot.ConvertTo<Announcement>();
                    var id = documentSnapshot.Id;
                    announcements.Add((announcement, id));
                }
            }
            return announcements;
        }, TimeSpan.FromSeconds(20));
    }
    public async Task<List<(Announcement announcement, string announcementId)>> GetPostIdAnnouncementsAsync(string postId)
    {
        return await _firebaseService.ExecuteFirestoreOperationAsync(async () =>
        {
            QuerySnapshot snapshot = await _db.Collection("Announcements").WhereEqualTo("PostId", postId).GetSnapshotAsync();
            List<(Announcement announcement, string announcementId)> announcements = new List<(Announcement announcement, string announcementId)>();

            foreach (DocumentSnapshot documentSnapshot in snapshot.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Announcement announcement = documentSnapshot.ConvertTo<Announcement>();
                    var id = documentSnapshot.Id;
                    announcements.Add((announcement, id));
                }
            }
            return announcements;
        }, TimeSpan.FromSeconds(20));
    }
    public async Task<List<(Announcement announcement, string id)>> GetPostIds(List<string> postIds)
    {
        return await _firebaseService.ExecuteFirestoreOperationAsync(async () =>
        {
            if (_db == null)
                throw new InvalidOperationException("Database service is not initialized properly.");

            var announcements = new List<(Announcement announcement, string id)>();

            foreach (var postId in postIds)
            {
                Query docRef = _db.Collection("Announcements").WhereEqualTo("PostId", postId);
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
