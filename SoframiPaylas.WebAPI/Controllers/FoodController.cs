using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SoframiPaylas.Application.DTOs.Food;
using SoframiPaylas.Application.Interfaces;

namespace SoframiPaylas.WebAPI
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoodController : Controller
    {

        private readonly IFoodService _service;
        public FoodController(IFoodService service)
        {
            _service = service;
        }
        [HttpGet("foods")]
        public async Task<IActionResult> GetAllFoods()
        {
            var foods = await _service.GetAllFoodAsync();
            if (foods == null)
            {
                return NotFound();
            }
            return Ok(foods);
        }
        [HttpGet("food/{foodId}")]
        public async Task<IActionResult> GetFoodById(string foodId)
        {
            var foodItem = await _service.GetFoodByIdAsync(foodId);
            if (foodItem == null)
                return NotFound("Food not found");
            return Ok(foodItem);
        }

        [HttpPost("food")]
        public async Task<IActionResult> CreateFood([FromBody] CreateFoodDto foodDto)
        {
            var foodId = await _service.CreateFoodAsync(foodDto);
            if (string.IsNullOrEmpty(foodId))
                return BadRequest();
            return Ok(foodId);
        }

        [HttpPut("food/{foodID}")]
        public async Task<IActionResult> UpdateFoodById([FromBody] UpdateFoodDto foodDto, string foodID)
        {
            await _service.UpdateFoodAsync(foodDto, foodID);
            return Ok();
        }
        [HttpDelete("food/{foodID}")]
        public async Task<IActionResult> DeleteFoodById(string foodID)
        {
            await _service.DeleteFoodAsync(foodID);
            return Ok();
        }
    }
}