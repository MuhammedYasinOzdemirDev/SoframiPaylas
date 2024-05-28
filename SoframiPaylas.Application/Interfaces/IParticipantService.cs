
using SoframiPaylas.Application.DTOs;
using SoframiPaylas.Application.DTOs.Participant;

namespace SoframiPaylas.Application.Interfaces
{
    public interface IParticipantService
    {
        Task<bool> AddParticipantAsync(ParticipantDto participantDto);
        Task<bool> ConfirmedParticipantStatus(ParticipantDto participantDto);
        Task<bool> DeclinedParticipantStatus(ParticipantDto participantDto);
        Task<IEnumerable<ParticipantViewDto>> GetPendingPostIdByAsync(string postId);
        Task<int> CheckIfRequestExistsAsync(string postId, string userId);
        Task<IEnumerable<ParticipantViewDto>> GetConfirmedPostIdByAsync(string postId);
        Task<bool> DeleteParticipantAsync(string id);
        Task<List<string>> GetUserIdPost(string userId);
        Task<IEnumerable<UserDto>> GetPostIdAsync(string postId);
        Task<bool> LeaveParticipantAsync(string postId, string userId);
    }
}