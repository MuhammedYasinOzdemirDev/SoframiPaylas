
using SoframiPaylas.Domain.Entities;

namespace SoframiPaylas.Infrastructure.Interfaces
{
    public interface IAuthRepository
    {
        Task<string> RegisterUserAsync(User user, string password);
        Task<string> GenerateEmailVerificationLink(string email);
        Task<bool> GetUserByUsernameAsync(string username);
        Task<string> SignInWithEmailAndPassword(string email, string password);
        Task<FirebaseUser> GetUserDetailsAsync(string idToken);
        Task<bool> ChangeUserPassword(string userId, string newPassword);
    }
}