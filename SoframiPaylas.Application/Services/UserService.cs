using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<string> CreateUserAsync(CreateUserDto userDto)
        {
            var user = new User
            {
                Email = userDto.Email,
                FullName = userDto.FullName,
                ProfilePicture = userDto.ProfilePicture,
                IsHost = userDto.IsHost,
                About = userDto.About,
                PasswordHash = userDto.PasswordHash
            };
            return await _userRepository.CreateUserAsync(user);
        }

        public async Task DeleteUserAsync(string userId)
        {
            await _userRepository.DeleteUserAsync(userId);
        }

        public async Task<IEnumerable<UserDto>> GetAllUserAsync()
        {
            var users = await _userRepository.GetAllUserAsync();
            return users.Select(u => new UserDto { FullName = u.FullName, Email = u.Email, IsHost = u.IsHost, About = u.About, ProfilePicture = u.ProfilePicture });
        }
        public async Task<UserDto> GetUserByIdAsync(string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            return new UserDto
            {
                Email = user.Email,
                FullName = user.FullName,
                ProfilePicture = user.ProfilePicture,
                IsHost = user.IsHost,
                About = user.About
            };
        }

        public async Task UpdateUserAsync(UpdateUserDto userDto, string userId)
        {
            var user = new User
            {
                Email = userDto.Email,
                FullName = userDto.FullName,
                ProfilePicture = userDto.ProfilePicture,
                PasswordHash = userDto.PasswordHash,
                IsHost = userDto.IsHost,
                About = userDto.About
            };
            await _userRepository.UpdateUserAsync(user, userId);
        }
    }
}