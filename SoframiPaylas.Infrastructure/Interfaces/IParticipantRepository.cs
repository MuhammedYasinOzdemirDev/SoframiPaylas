
using SoframiPaylas.Domain.Entities;

namespace SoframiPaylas.Infrastructure.Interfaces
{
    public interface IParticipantRepository
    {
        Task<bool> AddParticipantAsync(Participant participant);
        Task<bool> UpdateParticipantStatus(string postId, string userId, int status);

        Task<List<(Participant participant, string id, string userName)>> GetParticipantPostIdAsync(string postId);
        Task<bool> CheckIfRequestExistsAsync(string postId, string userId);
        Task<int> CheckRequestStatusAsync(string postId, string userId);
    }
}