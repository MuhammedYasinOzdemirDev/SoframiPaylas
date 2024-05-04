using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoframiPaylas.Domain.Entities;

namespace SoframiPaylas.Infrastructure.Interfaces
{
    public interface IParticipantRepository
    {
        Task AddParticipantAsync(Participant participant);
        Task<bool> UpdateParticipantStatus(string postId, string userId, int status);
    }
}