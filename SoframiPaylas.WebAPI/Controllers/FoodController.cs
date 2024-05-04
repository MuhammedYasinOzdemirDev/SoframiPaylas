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
        /// <summary>
        /// Tüm yiyecek öğelerinin listesini döner.
        /// </summary>
        /// <remarks>
        /// Bu işlem, veritabanındaki tüm yiyecek öğelerini getirir. Eğer kayıtlı hiç yiyecek yoksa, kullanıcıya 404 hatası ile bilgi verilir.
        ///
        /// ### Örnek Yanıt
        /// 
        /// Başarılı yanıt aşağıdaki gibi olacaktır:
        ///
        ///     [
        ///         {
        ///             "postID": "123",
        ///             "title": "Peynirli Pizza",
        ///             "description": "Zengin mozzarella peyniri ile hazırlanmış, kıtır hamurlu pizza",
        ///             "images": "url_to_pizza_image"
        ///         },
        ///         {
        ///             "postID": "124",
        ///             "title": "Sushi",
        ///             "description": "Taze hazırlanmış sushi çeşitleri",
        ///             "images": "url_to_sushi_image"
        ///         }
        ///     ]
        ///
        /// ### Hatalar
        /// 
        /// - **404 Not Found**: Eğer sistemde hiç yiyecek bulunamazsa, bu hata dönülür.
        /// - **500 Internal Server Error**: Veritabanı sorgusu sırasında beklenmedik bir hata oluşursa, bu hata dönülür.
        /// </remarks>
        /// <returns>Bir HTTP yanıtı döner ki bu, başarılı olursa yiyecek listesini, başarısız olursa hata mesajını içerir.</returns>
        /// <response code="200">Yiyecekler başarıyla bulundu ve döndürüldü.</response>
        /// <response code="404">Hiç yiyecek bulunamadı.</response>
        /// <response code="500">Yiyecekleri getirirken bir hata meydana geldi.</response>
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
                return StatusCode(500, "Yiyecekleri getirirken bir hata meydana geldi.");

            }
        }
        /// <summary>
        /// Belirtilen ID'ye sahip yiyeceği getirir.
        /// </summary>
        /// <remarks>
        /// Bu işlem, veritabanında belirtilen ID'ye sahip yiyeceği arar ve bulursa döndürür. Eğer ilgili yiyecek bulunamazsa, kullanıcıya 404 hatası ile bilgi verilir.
        ///
        /// ### Örnek İstek
        /// 
        ///     GET /api/Food/food/{foodId}
        ///     {
        ///         "foodId": "123"
        ///     }
        ///
        /// ### Örnek Yanıt
        /// 
        /// Başarılı yanıt aşağıdaki gibi olacaktır:
        ///
        ///     {
        ///         "postID": "123",
        ///         "title": "Peynirli Pizza",
        ///         "description": "Zengin mozzarella peyniri ile hazırlanmış, kıtır hamurlu pizza",
        ///         "images": "url_to_pizza_image"
        ///     }
        ///
        /// ### Hatalar
        /// 
        /// - **404 Not Found**: Eğer sistemde belirtilen ID'ye sahip bir yiyecek bulunamazsa, bu hata dönülür.
        /// - **500 Internal Server Error**: Yiyecek bilgisini getirme sırasında beklenmedik bir hata oluşursa, bu hata dönülür.
        /// </remarks>
        /// <param name="foodId">Getirilecek yiyeceğin benzersiz tanımlayıcısı (ID).</param>
        /// <returns>HTTP yanıtı olarak, bulunan yiyecek nesnesini veya hata mesajını içerir.</returns>
        /// <response code="200">Yiyecek başarıyla bulundu ve döndürüldü.</response>
        /// <response code="404">Belirtilen ID'ye sahip yiyecek bulunamadı.</response>
        /// <response code="500">Yiyecek bilgisini getirirken bir hata meydana geldi.</response>
        [HttpGet("food/{foodId}")]
        public async Task<IActionResult> GetFoodById(string foodId)
        {
            try
            {
                var foodItem = await _service.GetFoodByIdAsync(foodId);
                if (foodItem == null)
                    return NotFound("Food not found");
                return Ok(foodItem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the food.");

            }
        }
        /// <summary>
        /// Sistemde yeni bir yiyecek öğesi oluşturur.
        /// </summary>
        /// <remarks>
        /// Bu işlem, belirtilen detaylarda yeni bir yiyecek öğesi ekler. Yiyecek bilgileri olarak PostID, Başlık, Açıklama ve Resim URL'leri gereklidir. 
        /// Başarılı bir oluşturma işlemi sonucunda, yiyeceğin benzersiz tanımlayıcısı (ID) döndürülür.
        ///
        /// ### Örnek İstek
        /// 
        ///     POST /api/Food/food
        ///     {
        ///        "postID": "VNRPesXcUL01AOaqFouk",
        ///        "title": "Peynirli Pizza",
        ///        "description": "Zengin mozzarella peyniri ile hazırlanmış, kıtır hamurlu pizza",
        ///        "images": "url_to_pizza_image"
        ///     }
        ///
        /// ### Örnek Yanıt
        /// 
        /// Başarılı bir oluşturma işlemi sonucunda dönen yanıt:
        ///
        ///     "VNRPesXcUL01AOaqFouk"
        ///
        /// ### Hatalar
        /// 
        /// - **400 Bad Request**: Gönderilen verilerde eksiklik veya geçersizlik varsa bu hata kodu döner.
        /// - **500 Internal Server Error**: Yiyecek oluşturma işlemi sırasında beklenmedik bir hata oluşursa, bu hata dönülür.
        /// </remarks>
        /// <param name="foodDto">Eklenecek yiyeceğin detaylarını içeren DTO nesnesi. DTO, 'postID', 'title', 'description' ve 'images' alanlarını içermelidir.</param>
        /// <returns>Yeni oluşturulan yiyeceğin ID'si. Hata durumunda uygun hata mesajı içeren HTTP yanıtı döner.</returns>
        /// <response code="200">Yiyecek başarıyla oluşturuldu ve yiyeceğin ID'si döndürüldü.</response>
        /// <response code="400">Gönderilen verilerde eksiklik veya geçersizlik varsa bu hata kodu döner.</response>
        /// <response code="500">Yiyecek oluşturma işlemi sırasında beklenmedik bir hata oluştuğunda bu hata döner.</response>
        [HttpPost("food")]
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
                return StatusCode(500, "Yiyecek oluşturma işlemi sırasında bir hata meydana geldi.");

            }
        }
        /// <summary>
        /// Belirtilen ID'ye sahip olan yiyecek öğesini günceller.
        /// </summary>
        /// <remarks>
        /// Bu işlem, veritabanında belirtilen ID'ye sahip yiyecek öğesinin detaylarını günceller. Güncelleme için PostID, Başlık, Açıklama ve Resim URL'leri gereklidir.
        /// Yiyecek bulunamazsa 404 hatası dönülür, güncelleme başarılı olursa herhangi bir içerik dönmez (204 No Content).
        ///
        /// ### Örnek İstek
        /// 
        ///     PUT /api/Food/food/{foodID}
        ///     {
        ///        "postID": "1234",
        ///        "title": "Güncellenmiş Pizza",
        ///        "description": "Daha fazla peynir içeren, kıtır hamurlu pizza",
        ///        "images": "url_to_updated_pizza_image"
        ///     }
        ///
        /// ### Hatalar
        /// 
        /// - **400 Bad Request**: Gönderilen verilerde eksiklik veya geçersizlik varsa bu hata kodu döner.
        /// - **404 Not Found**: Belirtilen ID'ye sahip yiyecek bulunamazsa bu hata dönülür.
        /// - **500 Internal Server Error**: Güncelleme işlemi sırasında beklenmedik bir hata oluşursa, bu hata dönülür.
        /// </remarks>
        /// <param name="foodDto">Güncellenecek yiyeceğin yeni detaylarını içeren DTO nesnesi. DTO, 'postID', 'title', 'description', ve 'images' alanlarını içermelidir.</param>
        /// <param name="foodID">Güncellenecek yiyeceğin benzersiz tanımlayıcısı (ID).</param>
        /// <returns>HTTP yanıtı olarak, başarılı ise içerik dönmeyen yanıt, başarısız ise hata mesajı içerir.</returns>
        /// <response code="204">Yiyecek başarıyla güncellendi. İçerik dönmez.</response>
        /// <response code="400">Gönderilen verilerde eksiklik veya geçersizlik varsa bu hata kodu döner.</response>
        /// <response code="404">Belirtilen ID'ye sahip yiyecek bulunamadı.</response>
        /// <response code="500">Güncelleme işlemi sırasında beklenmedik bir hata oluştuğunda bu hata döner.</response>
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
                return StatusCode(500, "Yiyecek güncellenirken bir hata meydana geldi.");

            }
        }
        /// <summary>
        /// Belirtilen ID'ye sahip olan yiyecek öğesini sistemden siler.
        /// </summary>
        /// <remarks>
        /// Bu işlem, veritabanında belirtilen ID'ye sahip yiyeceği bulur ve siler. Yiyecek bulunamazsa 404 hatası dönülür, silme başarılı olursa herhangi bir içerik dönmez (204 No Content).
        ///
        /// ### Örnek İstek
        /// 
        ///     DELETE /api/Food/food/{foodID}
        ///     {
        ///         "foodID": "1234"
        ///     }
        ///
        /// ### Hatalar
        /// 
        /// - **404 Not Found**: Belirtilen ID'ye sahip yiyecek bulunamazsa bu hata dönülür.
        /// - **500 Internal Server Error**: Silme işlemi sırasında beklenmedik bir hata oluşursa, bu hata dönülür.
        /// </remarks>
        /// <param name="foodID">Silinmek istenen yiyeceğin benzersiz tanımlayıcısı (ID).</param>
        /// <returns>HTTP yanıtı olarak, başarılı ise içerik dönmeyen yanıt, başarısız ise hata mesajı içerir.</returns>
        /// <response code="204">Yiyecek başarıyla silindi. İçerik dönmez.</response>
        /// <response code="404">Belirtilen ID'ye sahip yiyecek bulunamadı.</response>
        /// <response code="500">Silme işlemi sırasında beklenmedik bir hata oluştuğunda bu hata döner.</response>
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
                return StatusCode(500, "Yiyecek silinirken bir hata meydana geldi.");
            }
        }

    }
}