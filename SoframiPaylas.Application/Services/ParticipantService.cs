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
        public async Task<bool> AddParticipantAsync(ParticipantDto participantDto)
        {
            return await _repository.AddParticipantAsync(new Participant
            {
                PostId = participantDto.PostID,
                UserID = participantDto.UserID,
                Status = (int)ParticipationStatus.Pending
            });
        }

        public async Task<bool> UpdateParticipantStatus(ParticipantDto participantDto)
        {
            return await _repository.UpdateParticipantStatus(participantDto.PostID, participantDto.UserID, (int)ParticipationStatus.Confirmed);
        }
        public enum ParticipationStatus
        {
            Pending, // Beklemede
            Confirmed, // Onaylandı
            Declined // Reddedildi
        }
    }
}