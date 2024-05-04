using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoframiPaylas.Domain.Entities;

namespace SoframiPaylas.Infrastructure.Interfaces
{
    public interface IFoodRepository
    {
        Task<List<Food>> GetAllFoodsAsync();
        Task<Food> GetFoodByIdAsync(string id);
        Task<string> CreateFoodAsync(Food food);
        Task UpdateFoodAsync(Food food, string footId);
        Task DeleteFoodAsync(string foodId);
    }
}