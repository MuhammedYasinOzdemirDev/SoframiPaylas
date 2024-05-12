
using SoframiPaylas.Domain.Entities;

namespace SoframiPaylas.Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUserAsync();
        Task<User> GetUserByIdAsync(string userId);

        Task<bool> UpdateUserAsync(User user, string userId);
        Task<bool> DeleteUserAsync(string userId);
    }
}