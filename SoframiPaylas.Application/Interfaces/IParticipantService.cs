
using SoframiPaylas.Application.DTOs.Participant;

namespace SoframiPaylas.Application.Interfaces
{
    public interface IParticipantService
    {
        Task<bool> AddParticipantAsync(ParticipantDto participantDto);
        Task<bool> UpdateParticipantStatus(ParticipantDto participantDtype);
    }
}