using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            var userId = await _authService.RegisterUserAsync(user, password);
            return Ok(new { UserId = userId, Message = "Registration successful" });
        }
        /*  catch (Exception ex)
          {

              return StatusCode(500, "Kullanıcı kaydı oluşturulamadı...");
          }*/


    }
}