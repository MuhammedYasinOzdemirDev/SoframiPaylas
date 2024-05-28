using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SoframiPaylas.Application.DTOs;
using SoframiPaylas.Application.DTOs.Participant;
using SoframiPaylas.Application.Interfaces;
using SoframiPaylas.Domain.Entities;
using SoframiPaylas.Infrastructure.Interfaces;

namespace SoframiPaylas.Application.Services
{
    public class ParticipantService : IParticipantService
    {
        private IParticipantRepository _repository;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public ParticipantService(IParticipantRepository repository, IPostRepository postRepository, IMapper mapper)
        {
            _repository = repository;
            _postRepository = postRepository;
            _mapper = mapper;
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
            var result = await _repository.UpdateParticipantStatus(participantDto.PostID, participantDto.UserID, (int)ParticipationStatus.Confirmed);
            if (result != null)
            {
                var result2 = await _postRepository.UpdateParticipantStatus(participantDto.PostID, result);
                return result2;
            }
            return false;
        }
        public async Task<bool> DeclinedParticipantStatus(ParticipantDto participantDto)
        {
            var result = await _repository.UpdateParticipantStatus(participantDto.PostID, participantDto.UserID, (int)ParticipationStatus.Declined);
            return result != null;
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
            var result = await _postRepository.RemoveParticipant(id);
            if (result != null)
            {
                var result2 = await _repository.DeleteParticipantAsync(id);
                return result2;
            }
            return false;
        }
        public async Task<List<string>> GetUserIdPost(string userId)
        {
            return await _repository.GetUserIdPost(userId);
        }

        public async Task<IEnumerable<UserDto>> GetPostIdAsync(string postId)
        {
            var users = await _repository.GetPostIdAsync(postId);

            return users.Select(u =>
            {
                var userDto = _mapper.Map<UserDto>(u.user);
                userDto.UserID = u.id;
                return userDto;
            });
        }

        public async Task<bool> LeaveParticipantAsync(string postId, string userId)
        {
            if (postId == null || userId == null)
            {
                return false;
            }
            return await _repository.LeaveParticipantAsync(postId, userId);
        }

        public enum ParticipationStatus
        {
            Pending, // Beklemede
            Confirmed, // Onaylandı
            Declined // Reddedildi
        }

    }
}