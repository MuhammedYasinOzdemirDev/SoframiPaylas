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

        public async Task<string> CreateUserAsync(CreateUserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            return await _userRepository.CreateUserAsync(user);
        }

        public async Task DeleteUserAsync(string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }
            await _userRepository.DeleteUserAsync(userId);
        }

        public async Task<IEnumerable<UserDto>> GetAllUserAsync()
        {
            var users = await _userRepository.GetAllUserAsync();
            return users.Select(u => _mapper.Map<UserDto>(u));
        }
        public async Task<UserDto> GetUserByIdAsync(string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            return _mapper.Map<UserDto>(user);
        }

        public async Task UpdateUserAsync(UpdateUserDto userDto, string userId)
        {

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }
            // Bu, mevcut kullanıcı nesnesini kullanarak, sadece gerekli alanları günceller.
            _mapper.Map(userDto, user);
            await _userRepository.UpdateUserAsync(user, userId);
        }
    }
}