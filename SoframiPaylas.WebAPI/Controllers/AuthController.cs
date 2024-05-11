using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
using SoframiPaylas.Application.DTOs;
using SoframiPaylas.Application.Interfaces;
using SoframiPaylas.Domain.Entities;

namespace SoframiPaylas.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDto user, string password)
        {
            if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(password))
            {
                return BadRequest("Email and password are required.");
            }
            var existingUser = await _authService.GetUserByUsernameAsync(user.UserName);
            if (existingUser)
            {
                return Conflict("Username already exists.");
            }
            try
            {
                var userId = await _authService.RegisterUserAsync(user, password);
                return Ok(new { UserId = userId, Message = "Registration successful" });
            }
            catch (FirebaseAuthException ex)
            {
                if (ex.AuthErrorCode == AuthErrorCode.EmailAlreadyExists)
                {
                    return Conflict("Email already exists.");
                }
                else
                {
                    return StatusCode(500, "Internal server error during registration.");
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Kullanıcı kaydı oluşturulamadı...");
            }
        }

    }
}