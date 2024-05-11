using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoframiPaylas.Application.DTOs;

namespace SoframiPaylas.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterUserAsync(CreateUserDto userDto, string password);
        Task<bool> GetUserByUsernameAsync(string username);
    }
}