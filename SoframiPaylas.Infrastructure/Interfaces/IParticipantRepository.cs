
using SoframiPaylas.Domain.Entities;

namespace SoframiPaylas.Infrastructure.Interfaces
{
    public interface IParticipantRepository
    {
        Task<bool> AddParticipantAsync(Participant participant);
        Task<bool> UpdateParticipantStatus(string postId, string userId, int status);
    }
}