using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Google.Cloud.Firestore;
using SoframiPaylas.Application.DTOs;
using SoframiPaylas.Application.Interfaces;
using SoframiPaylas.Domain.Entities;
using SoframiPaylas.Infrastructure.Interfaces;

namespace SoframiPaylas.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task DeleteUserAsync(string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user.user == null)
            {
                throw new Exception("User not found.");
            }
            await _userRepository.DeleteUserAsync(userId);
        }

        public async Task<IEnumerable<UserDto>> GetAllUserAsync()
        {
            var users = await _userRepository.GetAllUserAsync();
            return users.Select(u =>
            {
                var dto = _mapper.Map<UserDto>(u.user);
                dto.UserID = u.id;
                return dto;
            });
        }
        public async Task<UserDto> GetUserByIdAsync(string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user.user == null)
            {
                throw new Exception("User not found.");
            }

            var dto = _mapper.Map<UserDto>(user.user);
            dto.UserID = user.id;
            return dto;
        }

        public async Task UpdateUserAsync(UpdateUserDto userDto, string userId)
        {

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user.user == null)
            {
                throw new Exception("User not found.");
            }
            // Bu, mevcut kullanıcı nesnesini kullanarak, sadece gerekli alanları günceller.
            _mapper.Map(userDto, user.user);
            await _userRepository.UpdateUserAsync(user.user, userId);
        }
    }
}