
using SoframiPaylas.Application.DTOs;

namespace SoframiPaylas.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterUserAsync(CreateUserDto userDto, string password);
        Task<bool> GetUserByUsernameAsync(string username);
        Task<string> AuthenticateAsync(string email, string password);
    }
}