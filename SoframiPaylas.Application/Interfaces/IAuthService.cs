
using SoframiPaylas.Application.DTOs;
using SoframiPaylas.Domain.Entities;

namespace SoframiPaylas.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterUserAsync(CreateUserDto userDto, string password);
        Task<bool> GetUserByUsernameAsync(string username);
        Task<string> AuthenticateAsync(string email, string password);
        Task<FirebaseUser> VerifyUser(string idToken);
    }
}