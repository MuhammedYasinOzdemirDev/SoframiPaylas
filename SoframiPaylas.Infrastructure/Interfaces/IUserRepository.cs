using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoframiPaylas.Domain.Entities;

namespace SoframiPaylas.Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUserAsync();
        Task<User> GetUserByIdAsync(string userId);
        Task<string> CreateUserAsync(User user);
        Task UpdateUserAsync(User user, string userId);
    }
}