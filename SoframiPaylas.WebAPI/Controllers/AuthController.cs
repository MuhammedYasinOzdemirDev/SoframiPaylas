using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
using SoframiPaylas.Application.DTOs;
using SoframiPaylas.Application.DTOs.User;
using SoframiPaylas.Application.Interfaces;


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
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            try
            {
                var token = await _authService.AuthenticateAsync(request.Email, request.Password);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("verify-user")]
        public async Task<IActionResult> VerifyUser(string idToken)
        {
            try
            {
                var user = await _authService.VerifyUser(idToken);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}