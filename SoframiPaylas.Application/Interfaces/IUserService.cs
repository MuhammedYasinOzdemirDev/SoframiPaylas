using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoframiPaylas.Application.DTOs;
using SoframiPaylas.Domain.Entities;

namespace SoframiPaylas.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUserAsync();
        Task<UserDto> GetUserByIdAsync(string userId);
        Task<string> CreateUserAsync(CreateUserDto userDto);
        Task UpdateUserAsync(UpdateUserDto userDto, string userId);
        Task DeleteUserAsync(string userId);
    }
}