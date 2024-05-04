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
        /// <summary>
        /// Tüm kullanıcıların listesini döner.
        /// </summary>
        /// <remarks>
        /// Bu işlem, sistemde kayıtlı olan tüm kullanıcıları getirir. Eğer kayıtlı hiç kullanıcı yoksa, kullanıcıya 404 hatası ile bilgi verilir.
        ///
        /// ### Örnek Yanıt
        /// 
        /// Başarılı yanıt aşağıdaki gibi olacaktır:
        ///
        ///     [
        ///         {
        ///          "email": "example@email.com",
        ///         "fullName": "Kullanıcı 1",
        ///         "isHost": true,
        ///         "profilePicture": "https://example.com/images/profile.jpg",
        ///         "about": "Kullanıcı hakkında kısa bilgi."
        ///         },
        ///         {
        ///            "email": "example@email.com",
        ///         "fullName": "Kullanıcı 2",
        ///         "isHost": true,
        ///         "profilePicture": "https://example.com/images/profile.jpg",
        ///         "about": "Kullanıcı hakkında kısa bilgi."
        ///         }
        ///     ]
        ///
        /// ### Hatalar
        /// 
        /// - **404 Not Found**: Eğer sistemde hiç kullanıcı bulunamazsa, bu hata dönülür.
        /// - **500 Internal Server Error**: Kullanıcılar getirilirken beklenmedik bir hata oluşursa, bu hata dönülür.
        /// </remarks>
        /// <returns>Bir HTTP yanıtı döner ki bu, başarılı olursa kullanıcı listesini, başarısız olursa hata mesajını içerir.</returns>
        /// <response code="200">Kullanıcılar başarıyla bulundu ve döndürüldü.</response>
        /// <response code="404">Hiç kullanıcı bulunamadı.</response>
        /// <response code="500">Kullanıcıları getirirken bir hata meydana geldi.</response>
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUserAsync()
        {
            try
            {
                var users = await _userService.GetAllUserAsync();
                if (users == null)
                {
                    return NotFound("Hiç kullanıcı bulunamadı.");
                }
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Kullanıcıları getirirken bir hata meydana geldi.");
            }
        }
        /// <summary>
        /// Belirtilen ID'ye sahip kullanıcıyı getirir.
        /// </summary>
        /// <remarks>
        /// Bu işlem, veritabanında belirtilen GUID formatındaki ID'ye sahip kullanıcıyı arar ve bulursa döndürür. Eğer ilgili kullanıcı bulunamazsa, kullanıcıya 404 hatası ile bilgi verilir.
        ///
        /// ### Örnek İstek
        /// 
        ///     GET /api/users/{userId}
        ///     {
        ///         "userId": "f2b3ec34-eb7a-46ef-8c2e-f10ba9b8e8b2"
        ///     }
        ///
        /// ### Örnek Yanıt
        /// 
        /// Başarılı yanıt aşağıdaki gibi olacaktır:
        ///
        ///     {
        ///         "email": "example@email.com",
        ///         "fullName": "Kullanıcı Adı Soyadı",
        ///         "isHost": true,
        ///         "profilePicture": "https://example.com/images/profile.jpg",
        ///         "about": "Kullanıcı hakkında kısa bilgi."
        ///     }
        ///
        /// ### Hatalar
        /// 
        /// - **400 Bad Request**: Kullanıcı ID'si sağlanmadıysa bu hata dönülür.
        /// - **404 Not Found**: Belirtilen ID'ye sahip kullanıcı bulunamazsa bu hata dönülür.
        /// - **500 Internal Server Error**: Kullanıcı bilgisi getirilirken beklenmedik bir hata oluşursa, bu hata dönülür.
        /// </remarks>
        /// <param name="userId">Getirilecek kullanıcının GUID formatındaki benzersiz tanımlayıcısı.</param>
        /// <returns>HTTP yanıtı olarak, bulunan kullanıcı nesnesini veya hata mesajını içerir.</returns>
        /// <response code="200">Kullanıcı başarıyla bulundu ve döndürüldü.</response>
        /// <response code="400">Kullanıcı ID'si sağlanmadı.</response>
        /// <response code="404">Belirtilen ID'ye sahip kullanıcı bulunamadı.</response>
        /// <response code="500">Kullanıcı bilgisi getirilirken bir hata meydana geldi.</response>
        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetUserByIdAsync(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("Kullanıcı ID'si sağlanmadı.");
                }
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return NotFound("Belirtilen ID'ye sahip kullanıcı bulunamadı.");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return StatusCode(500, "Kullanıcı bilgisi getirilirken bir hata meydana geldi.");
            }
        }
        /// <summary>
        /// Yeni bir kullanıcı oluşturur.
        /// </summary>
        /// <remarks>
        /// Bu işlem, gönderilen kullanıcı bilgileriyle yeni bir kullanıcı kaydı oluşturur. Eğer kullanıcı bilgileri doğrulanamazsa veya eksikse, 400 Bad Request hatası döner. Kullanıcı başarıyla oluşturulursa, kullanıcının benzersiz tanımlayıcısı döndürülür.
        ///
        /// ### Örnek İstek
        /// 
        ///     POST /api/users/user
        ///     {
        ///        "email": "newuser@example.com",
        ///        "fullName": "Yeni Kullanıcı",
        ///        "isHost": false,
        ///        "profilePicture": "https://example.com/images/default.jpg",
        ///        "about": "Kullanıcı hakkında bilgi."
        ///     }
        ///
        /// ### Örnek Yanıt
        /// 
        /// Başarılı bir oluşturma işlemi sonucunda dönen yanıt:
        ///
        ///     "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        ///
        /// ### Hatalar
        /// 
        /// - **400 Bad Request**: Gönderilen verilerde eksiklik veya geçersizlik varsa bu hata kodu döner.
        /// - **500 Internal Server Error**: Kullanıcı oluşturma işlemi sırasında beklenmedik bir hata oluşursa, bu hata dönülür.
        /// </remarks>
        /// <param name="userDto">Oluşturulacak kullanıcının bilgilerini içeren DTO nesnesi. DTO, 'email', 'fullName', 'isHost', 'profilePicture', ve 'about' alanlarını içermelidir.</param>
        /// <returns>HTTP yanıtı olarak, başarılı ise kullanıcının ID'si, başarısız ise hata mesajı içerir.</returns>
        /// <response code="200">Kullanıcı başarıyla oluşturuldu ve kullanıcının ID'si döndürüldü.</response>
        /// <response code="400">Gönderilen verilerde eksiklik veya geçersizlik varsa bu hata kodu döner.</response>
        /// <response code="500">Kullanıcı oluşturma işlemi sırasında beklenmedik bir hata oluştuğunda bu hata döner.</response>
        [HttpPost("user")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                // Detaylı ModelState hatalarını döndür
                return BadRequest(new { error = "Validation failed", details = ModelState });
            }
            try
            {
                var userId = await _userService.CreateUserAsync(userDto);
                if (string.IsNullOrEmpty(userId))
                {
                    // Kullanıcı oluşturulamadı için özelleştirilmiş mesaj
                    return BadRequest("Kullanıcı oluşturulamadı, detaylar eksik veya geçersiz.");
                }

                return Ok(userId);
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return StatusCode(500, "Kullanıcı oluşturulurken bir hata meydana geldi.");
            }
        }
        /// <summary>
        /// Belirtilen ID'ye sahip kullanıcıyı günceller.
        /// </summary>
        /// <remarks>
        /// Bu işlem, belirtilen kullanıcı ID'sine sahip kullanıcının bilgilerini günceller. Eğer veriler doğrulanamazsa 400 hata kodu ile birlikte hata detayları döner. Güncelleme işlemi başarılı olursa 200 durum kodu ile cevap döner.
        ///
        /// ### Örnek İstek
        /// 
        ///     PUT /api/users/user/{userId}
        ///     {
        ///        "email": "updateduser@example.com",
        ///        "fullName": "Güncellenmiş Kullanıcı Adı",
        ///        "isHost": true,
        ///        "profilePicture": "https://example.com/images/updated_profile.jpg",
        ///        "about": "Kullanıcı hakkında güncellenmiş bilgi."
        ///     }
        ///
        /// ### Hatalar
        /// 
        /// - **400 Bad Request**: Gönderilen verilerin doğrulaması başarısız olduğunda bu hata dönülür.
        /// - **500 Internal Server Error**: Kullanıcı güncelleme işlemi sırasında beklenmedik bir hata oluşursa, bu hata dönülür.
        /// </remarks>
        /// <param name="userId">Güncellenmek istenen kullanıcının GUID formatındaki benzersiz tanımlayıcısı.</param>
        /// <param name="userDto">Güncellenmek istenen kullanıcının yeni bilgilerini içeren DTO nesnesi.</param>
        /// <returns>HTTP yanıtı olarak, başarılı ise boş bir cevap, başarısız ise hata mesajı içerir.</returns>
        /// <response code="200">Kullanıcı başarıyla güncellendi.</response>
        /// <response code="400">Gönderilen verilerin doğrulaması başarısız olduğunda bu hata kodu dönülür.</response>
        /// <response code="500">Kullanıcı güncelleme işlemi sırasında beklenmedik bir hata oluştuğunda bu hata dönülür.</response>
        [HttpPut("user/{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { error = "Gönderilen verilerin doğrulaması başarısız olduğunda bu hata kodu dönülür.", details = ModelState });
            }
            try
            {
                await _userService.UpdateUserAsync(userDto, userId);
                return Ok("Kullanıcı başarıyla güncellendi.");
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return StatusCode(500, "Kullanıcı güncelleme işlemi sırasında beklenmedik bir hata oluştuğunda bu hata dönülür.");
            }
        }
        /// <summary>
        /// Belirtilen ID'ye sahip kullanıcıyı sistemden siler.
        /// </summary>
        /// <remarks>
        /// Bu işlem, veritabanında belirtilen ID'ye sahip kullanıcıyı arar ve siler. Kullanıcı başarıyla silinirse 204 No Content yanıtı döner. 
        /// Kullanıcı bulunamazsa veya silinemezse, hata detayları ile birlikte 500 Internal Server Error yanıtı döner.
        ///
        /// ### Örnek İstek
        /// 
        ///     DELETE /api/users/user/{userId}
        ///     {
        ///         "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        ///     }
        ///
        /// ### Hatalar
        /// 
        /// - **204 No Content**: Kullanıcı başarıyla silindi.
        /// - **500 Internal Server Error**: Kullanıcı silinirken beklenmedik bir hata oluşursa, bu hata dönülür.
        /// </remarks>
        /// <param name="userId">Silinmek istenen kullanıcının GUID formatındaki benzersiz tanımlayıcısı.</param>
        /// <returns>HTTP yanıtı olarak, başarılı ise içerik dönmeyen yanıt, başarısız ise hata mesajı içerir.</returns>
        /// <response code="204">Kullanıcı başarıyla silindi. İçerik dönmez.</response>
        /// <response code="500">Kullanıcı silinirken beklenmedik bir hata oluştuğunda bu hata dönülür.</response>
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
                return StatusCode(500, "Kullanıcı silinirken beklenmedik bir hata oluştuğunda bu hata dönülür.");
            }
        }

    }
}