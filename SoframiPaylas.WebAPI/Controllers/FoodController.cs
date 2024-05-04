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
            try
            {
                var foods = await _service.GetAllFoodAsync();
                if (foods == null || !foods.Any())
                {
                    return NotFound("Hiç yiyecek bulunamadı.");
                }
                return Ok(foods);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the food.");

            }
        }
        [HttpGet("food/{foodId}")]
        public async Task<IActionResult> GetFoodById(string foodId)
        {
            try
            {
                var foodItem = await _service.GetFoodByIdAsync(foodId);
                if (foodItem == null)
                    return NotFound("Yiyecek bulunamadı");
                return Ok(foodItem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the food.");

            }
        }

        [HttpPost("food")]
        public async Task<IActionResult> CreateFood([FromBody] CreateFoodDto foodDto)
        {
            try
            {
                var foodId = await _service.CreateFoodAsync(foodDto);
                if (string.IsNullOrEmpty(foodId))
                    return BadRequest("Yiyecek oluşturulamadı.");
                return Ok(foodId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the food.");

            }
        }

        [HttpPut("food/{foodID}")]
        public async Task<IActionResult> UpdateFoodById([FromBody] UpdateFoodDto foodDto, string foodID)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var updateResult = await _service.UpdateFoodAsync(foodDto, foodID);
                if (!updateResult)
                    return NotFound("Güncellenecek yiyecek bulunamadı.");
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the food.");

            }
        }
        [HttpDelete("food/{foodID}")]
        public async Task<IActionResult> DeleteFoodById(string foodID)
        {
            try
            {
                var deleteResult = await _service.DeleteFoodAsync(foodID);
                if (!deleteResult)
                    return NotFound("Silinecek yiyecek bulunamadı.");
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the food.");

            }
        }

    }
}