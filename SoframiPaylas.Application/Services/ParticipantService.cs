using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SoframiPaylas.Application.DTOs.Participant;
using SoframiPaylas.Application.Interfaces;
using SoframiPaylas.Domain.Entities;
using SoframiPaylas.Infrastructure.Interfaces;

namespace SoframiPaylas.Application.Services
{
    public class ParticipantService : IParticipantService
    {
        private IParticipantRepository _repository;
        public ParticipantService(IParticipantRepository repository)
        {
            _repository = repository;
        }
        public async Task AddParticipantAsync(string postId, JoinParticipantDto joinParticipantDto)
        {
            await _repository.AddParticipantAsync(new Participant
            {
                PostId = postId,
                UserID = joinParticipantDto.UserID,
                Status = joinParticipantDto.Status
            });
        }

        public async Task<bool> UpdateParticipantStatus(string postId, string userId)
        {
            return await _repository.UpdateParticipantStatus(postId, userId, (int)ParticipationStatus.Confirmed);
        }
        public enum ParticipationStatus
        {
            Pending, // Beklemede
            Confirmed, // Onaylandı
            Declined // Reddedildi
        }
    }
}