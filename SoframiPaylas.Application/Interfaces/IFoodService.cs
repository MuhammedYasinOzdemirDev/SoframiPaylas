using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoframiPaylas.Application.DTOs.Food;


namespace SoframiPaylas.Application.Interfaces
{
    public interface IFoodService
    {
        Task<IEnumerable<FoodDto>> GetAllFoodAsync();
        Task<FoodDto> GetFoodByIdAsync(string foodId);
        Task<string> CreateFoodAsync(CreateFoodDto foodDto);
        Task UpdateFoodAsync(UpdateFoodDto foodDto, string foodId);
        Task DeleteFoodAsync(string foodId);
    }
}