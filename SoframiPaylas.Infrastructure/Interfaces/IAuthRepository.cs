
using SoframiPaylas.Domain.Entities;

namespace SoframiPaylas.Infrastructure.Interfaces
{
    public interface IAuthRepository
    {
        Task<string> RegisterUserAsync(User user, string password);
        Task<string> GenerateEmailVerificationLink(string email);
        Task<bool> GetUserByUsernameAsync(string username);
    }
}