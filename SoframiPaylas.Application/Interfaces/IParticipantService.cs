using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoframiPaylas.Application.DTOs.Participant;

namespace SoframiPaylas.Application.Interfaces
{
    public interface IParticipantService
    {
        Task AddParticipantAsync(string postId, JoinParticipantDto joinParticipantDto);
        Task<bool> UpdateParticipantStatus(string postId, string userId);
    }
}