using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SoframiPaylas.Application.DTOs;
using SoframiPaylas.Application.Interfaces;

namespace SoframiPaylas.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUserAsync()
        {
            try
            {
                var users = await _userService.GetAllUserAsync();
                if (users == null)
                {
                    return NotFound("Users not found.");
                }
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving users.");
            }
        }
        [HttpGet("users/{userId}", Name = "GetUserById")]
        public async Task<IActionResult> GetUserByIdAsync(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User ID must be provided.");
                }
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return NotFound($"User with ID {userId} not found.");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return StatusCode(500, "An error occurred while retrieving the user.");
            }
        }
        [HttpPost("user")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var userId = await _userService.CreateUserAsync(userDto);
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("Failed to create user.");
                }

                return Ok(userId);
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return StatusCode(500, "An error occurred while creating the user.");
            }
        }
        [HttpPut("user/{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _userService.UpdateUserAsync(userDto, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return StatusCode(500, "An error occurred while updating the user.");
            }
        }
        [HttpDelete("user/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                await _userService.DeleteUserAsync(userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return StatusCode(500, "An error occurred while deleting the user.");
            }
        }

    }
}