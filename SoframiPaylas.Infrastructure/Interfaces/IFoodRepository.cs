
using SoframiPaylas.Domain.Entities;

namespace SoframiPaylas.Infrastructure.Interfaces
{
    public interface IFoodRepository
    {
        Task<List<Food>> GetAllFoodsAsync();
        Task<Food> GetFoodByIdAsync(string id);
        Task<string> CreateFoodAsync(Food food);
        Task<bool> UpdateFoodAsync(Food food, string footId);
        Task<bool> DeleteFoodAsync(string foodId);
    }
}