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

        public async Task<bool> ConfirmedParticipantStatus(ParticipantDto participantDto)
        {
            return await _repository.UpdateParticipantStatus(participantDto.PostID, participantDto.UserID, (int)ParticipationStatus.Confirmed);
        }
        public async Task<bool> DeclinedParticipantStatus(ParticipantDto participantDto)
        {
            return await _repository.UpdateParticipantStatus(participantDto.PostID, participantDto.UserID, (int)ParticipationStatus.Declined);
        }
        public async Task<IEnumerable<ParticipantViewDto>> GetPendingPostIdByAsync(string postId)
        {
            var participants = await _repository.GetParticipantPostIdAsync(postId);
            return participants
                .Where(p => (ParticipationStatus)p.participant.Status == ParticipationStatus.Pending)
                .Select(p => new ParticipantViewDto
                {
                    ParticipantId = p.id,
                    Status = p.participant.Status,
                    UserID = p.participant.UserID,
                    PostID = p.participant.PostId,
                    UserName = p.userName,

                });
        }
        public async Task<IEnumerable<ParticipantViewDto>> GetConfirmedPostIdByAsync(string postId)
        {
            var participants = await _repository.GetParticipantPostIdAsync(postId);
            return participants
                .Where(p => (ParticipationStatus)p.participant.Status == ParticipationStatus.Confirmed)
                .Select(p => new ParticipantViewDto
                {
                    ParticipantId = p.id,
                    Status = p.participant.Status,
                    UserID = p.participant.UserID,
                    PostID = p.participant.PostId,
                    UserName = p.userName,

                });
        }
        public async Task<int> CheckIfRequestExistsAsync(string postId, string userId)
        {
            bool requestExists = await _repository.CheckIfRequestExistsAsync(postId, userId);
            if (requestExists)
            {
                return await _repository.CheckRequestStatusAsync(postId, userId);
            }
            return -1;
        }
        public async Task<bool> DeleteParticipantAsync(string id)
        {
            return await _repository.DeleteParticipantAsync(id);
        }
        public enum ParticipationStatus
        {
            Pending, // Beklemede
            Confirmed, // Onaylandı
            Declined // Reddedildi
        }

    }
}