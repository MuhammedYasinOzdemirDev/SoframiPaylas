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
        /// <summary>
        /// Yeni kullanıcı kaydı oluşturur.
        /// </summary>
        /// <remarks>
        /// Bu işlem, verilen kullanıcı bilgileri ve şifre ile yeni bir kullanıcı kaydı oluşturur.
        /// </remarks>
        /// <param name="user">Kullanıcı bilgilerini içeren DTO.</param>
        /// <param name="password">Kullanıcının şifresi.</param>
        /// <returns>Bir HTTP yanıtı döner ki bu, başarılı olursa yeni kullanıcının ID'sini ve başarılı mesajını, başarısız olursa hata mesajını içerir.</returns>
        /// <response code="200">Kayıt başarılı.</response>
        /// <response code="400">Email ve şifre gereklidir.</response>
        /// <response code="409">Kullanıcı adı veya email zaten var.</response>
        /// <response code="500">Kayıt sırasında bir hata meydana geldi.</response>
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
        /// <summary>
        /// Kullanıcı giriş yapar ve bir token döner.
        /// </summary>
        /// <remarks>
        /// Bu işlem, verilen email ve şifre ile kullanıcıyı kimlik doğrular ve bir JWT token döner.
        /// </remarks>
        /// <param name="request">Giriş bilgilerini içeren DTO.</param>
        /// <returns>Bir HTTP yanıtı döner ki bu, başarılı olursa JWT tokenını, başarısız olursa hata mesajını içerir.</returns>
        /// <response code="200">Giriş başarılı.</response>
        /// <response code="400">Giriş sırasında bir hata meydana geldi.</response>
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
        /// <summary>
        /// Kullanıcıyı doğrular.
        /// </summary>
        /// <remarks>
        /// Bu işlem, verilen ID token ile kullanıcıyı doğrular ve kullanıcı bilgilerini döner.
        /// </remarks>
        /// <param name="idToken">Doğrulanacak ID token.</param>
        /// <returns>Bir HTTP yanıtı döner ki bu, başarılı olursa kullanıcı bilgilerini, başarısız olursa hata mesajını içerir.</returns>
        /// <response code="200">Kullanıcı doğrulandı.</response>
        /// <response code="400">Doğrulama sırasında bir hata meydana geldi.</response>
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
        /// <summary>
        /// Kullanıcı şifresini değiştirir.
        /// </summary>
        /// <remarks>
        /// Bu işlem, verilen bilgilerle kullanıcının şifresini değiştirir.
        /// </remarks>
        /// <param name="dto">Şifre değişikliği bilgilerini içeren DTO.</param>
        /// <returns>Bir HTTP yanıtı döner ki bu, başarılı olursa başarı mesajını, başarısız olursa hata mesajını içerir.</returns>
        /// <response code="200">Şifre başarıyla değiştirildi.</response>
        /// <response code="400">Şifre değişikliği sırasında bir hata meydana geldi.</response>
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangeUserPassword([FromBody] ChangeUserPasswordDto dto)
        {

            try
            {
                var result = await _authService.ChangeUserPassword(dto);
                if (result)
                {
                    return Ok("Password changed successfully.");
                }
                else
                {
                    return BadRequest("Password change failed.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}