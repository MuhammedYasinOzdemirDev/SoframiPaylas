
using SoframiPaylas.Application.DTOs.Food;


namespace SoframiPaylas.Application.Interfaces
{
    public interface IFoodService
    {
        Task<IEnumerable<FoodDto>> GetAllFoodAsync();
        Task<FoodDto> GetFoodByIdAsync(string foodId);
        Task<string> CreateFoodAsync(CreateFoodDto foodDto);
        Task<bool> UpdateFoodAsync(UpdateFoodDto foodDto, string foodId);
        Task<bool> DeleteFoodAsync(string foodId);
    }
}