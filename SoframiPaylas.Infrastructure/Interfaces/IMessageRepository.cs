using FirebaseAdmin.Messaging;
using SoframiPaylas.Domain.Entities;

namespace SoframiPaylas.Infrastructure.Interfaces;
public interface IMessageRepository
{
    Task<(MessageStore message, string messageId)> AddMessageAsync(MessageStore message);
    Task<List<(MessageStore message, string messageId)>> GetMessagesByReceiverIdAsync(string receiverId);
    Task<int> MessageCount(string userId);
}