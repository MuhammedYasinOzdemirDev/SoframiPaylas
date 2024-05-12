
using SoframiPaylas.Domain.Entities;

namespace SoframiPaylas.Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUserAsync();
        Task<User> GetUserByIdAsync(string userId);

        Task UpdateUserAsync(User user, string userId);
        Task DeleteUserAsync(string userId);
    }
}