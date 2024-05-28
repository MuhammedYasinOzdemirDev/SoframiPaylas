namespace SoframiPaylas.Infrastructure.Repositories;

using FirebaseAdmin.Messaging;
using Google.Cloud.Firestore;
using SoframiPaylas.Domain.Entities;
using SoframiPaylas.Infrastructure.Data.Service;
using SoframiPaylas.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MessageRepository : IMessageRepository
{
    private readonly FirestoreDb _db;
    private readonly FirebaseService _firebaseService;

    public MessageRepository(FirebaseService service)
    {
        _db = service.GetDb();
        _firebaseService = service;
    }

    public async Task<(MessageStore message, string messageId)> AddMessageAsync(MessageStore message)
    {
        return await _firebaseService.ExecuteFirestoreOperationAsync(async () =>
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message), "Message object must not be null.");

            DocumentReference docRef = await _db.Collection("Messages").AddAsync(message);
            var id = docRef.Id;
            return (message, id);
        }, TimeSpan.FromSeconds(20));
    }

    public async Task<List<(MessageStore message, string messageId)>> GetMessagesByReceiverIdAsync(string receiverId)
    {
        return await _firebaseService.ExecuteFirestoreOperationAsync(async () =>
        {
            if (string.IsNullOrEmpty(receiverId))
                throw new ArgumentException("Receiver ID cannot be null or empty.", nameof(receiverId));

            Query messageQuery = _db.Collection("Messages").WhereEqualTo("ReceiverId", receiverId);
            QuerySnapshot messageQuerySnapshot = await messageQuery.GetSnapshotAsync();
            List<(MessageStore message, string messageId)> messages = new List<(MessageStore message, string messageId)>();

            foreach (DocumentSnapshot documentSnapshot in messageQuerySnapshot.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    MessageStore message = documentSnapshot.ConvertTo<MessageStore>();
                    var id = documentSnapshot.Id;
                    messages.Add((message, id));
                }
            }
            return messages;
        }, TimeSpan.FromSeconds(20));
    }
    public async Task<int> MessageCount(string userId)
    {
        return await _firebaseService.ExecuteFirestoreOperationAsync(async () =>
        {
            QuerySnapshot snapshot = await _db.Collection("Messages").WhereEqualTo("ReceiverId", userId).GetSnapshotAsync();
            return snapshot.Count;
        }, TimeSpan.FromSeconds(20));

    }
}
