
using SoframiPaylas.Application.DTOs.Participant;

namespace SoframiPaylas.Application.Interfaces
{
    public interface IParticipantService
    {
        Task<bool> AddParticipantAsync(string postId, JoinParticipantDto joinParticipantDto);
        Task<bool> UpdateParticipantStatus(string postId, string userId);
    }
}