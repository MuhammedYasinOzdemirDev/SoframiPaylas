using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Google.Cloud.Firestore;
using SoframiPaylas.Application.DTOs.Food;
using SoframiPaylas.Application.Interfaces;
using SoframiPaylas.Domain.Entities;
using SoframiPaylas.Infrastructure.Interfaces;

namespace SoframiPaylas.Application.Services
{
    public class FoodService : IFoodService
    {
        private readonly IFoodRepository _foodrepository;
        private readonly IMapper _mapper;

        public FoodService(IFoodRepository foodRepository, IMapper mapper)
        {
            _foodrepository = foodRepository;
            _mapper = mapper;
        }

        public async Task<string> CreateFoodAsync(CreateFoodDto foodDto)
        {
            var food = _mapper.Map<Food>(foodDto);
            return await _foodrepository.CreateFoodAsync(food);
        }

        public async Task DeleteFoodAsync(string foodId)
        {
            var food = await _foodrepository.GetFoodByIdAsync(foodId);
            if (food == null)
            {
                throw new Exception("Food not found.");
            }
            await _foodrepository.DeleteFoodAsync(foodId);
        }

        public async Task<IEnumerable<FoodDto>> GetAllFoodAsync()
        {
            var foods = await _foodrepository.GetAllFoodsAsync();
            return foods.Select(p => _mapper.Map<FoodDto>(p));
        }

        public async Task<FoodDto> GetFoodByIdAsync(string userId)
        {
            var food = await _foodrepository.GetFoodByIdAsync(userId);
            if (food == null)
            {
                throw new KeyNotFoundException($"No food found with ID {userId}");
            }
            return _mapper.Map<FoodDto>(food);
        }

        public async Task UpdateFoodAsync(UpdateFoodDto foodDto, string foodId)
        {
            var food = await _foodrepository.GetFoodByIdAsync(foodId);
            if (food == null)
            {
                throw new Exception("Food not found.");
            }
            _mapper.Map(foodDto, food);
            await _foodrepository.UpdateFoodAsync(food, foodId);
        }
    }
}