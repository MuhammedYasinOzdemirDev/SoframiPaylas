using System.Net;
using Microsoft.AspNetCore.Mvc;
using SoframiPaylas.Application.DTOs.Participant;
using SoframiPaylas.Application.DTOs.Post;
using SoframiPaylas.Application.Interfaces;

namespace SoframiPaylas.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postservice;
        private readonly IParticipantService _participantService;

        public PostController(IPostService postService, IParticipantService participantService)
        {
            _postservice = postService;
            _participantService = participantService;
        }
        /// <summary>
        /// Sistemdeki tüm gönderileri listeler.
        /// </summary>
        /// <remarks>
        /// Bu işlem, sistemde kaydedilmiş tüm aktif gönderileri getirir. Eğer gönderi bulunamazsa, 404 Not Found yanıtı döner.
        ///
        /// ### Örnek Yanıt
        /// 
        /// Başarılı yanıt aşağıdaki gibi olacaktır:
        ///
        ///     [
        ///         {
        ///             "hostID": "8ymayJzHVVX393V9ZdO8",
        ///             "title": "Yemek Paylaşım Etkinliği",
        ///             "description": "Bu etkinlikte ev yapımı yemeklerimizi paylaşıyoruz!",
        ///             "location": { "latitude": 41.0082, "longitude": 28.9784 },
        ///             "formattedDate": "23/04/2024",
        ///             "time": "18:30",
        ///             "maxParticipants": 20,
        ///             "images": ["https://example.com/images/event1.jpg"],
        ///             "postStatus": true,
        ///             "relatedFoods": ["Lahmacun", "Baklava"],
        ///             "participants": ["user1", "user2"]
        ///         }
        ///     ]
        ///
        /// ### Hatalar
        /// 
        /// - **404 Not Found**: Kayıtlı gönderi bulunamadığında bu hata dönülür.
        /// - **500 Internal Server Error**: Gönderiler getirilirken beklenmedik bir hata oluşursa, bu hata dönülür.
        /// </remarks>
        /// <returns>Bir HTTP yanıtı döner ki bu, başarılı olursa gönderi listesini, başarısız olursa hata mesajını içerir.</returns>
        /// <response code="200">Gönderiler başarıyla bulundu ve döndürüldü.</response>
        /// <response code="404">Hiç gönderi bulunamadı.</response>
        /// <response code="500">Gönderileri getirirken bir hata meydana geldi.</response>
        [HttpGet("posts")]
        public async Task<IActionResult> GetAllPostAsync()
        {
            try
            {
                var posts = await _postservice.GetAllPostsAsync();
                if (posts == null || !posts.Any())
                    return NotFound("Hiç gönderi bulunamadı.");
                return Ok(posts);
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return StatusCode(500, "Gönderileri getirirken bir hata meydana geldi.");
            }
        }
        /// <summary>
        /// Belirtilen ID'ye sahip gönderiyi getirir.
        /// </summary>
        /// <remarks>
        /// Bu işlem, sistemde belirtilen ID'ye sahip gönderiyi arar ve bulursa döndürür. Eğer ilgili gönderi bulunamazsa, kullanıcıya 404 Not Found hatası ile bilgi verilir.
        ///
        /// ### Örnek İstek
        /// 
        ///     GET /api/Post/post/{postId}
        ///     {
        ///         "postId": "8ymayJzHVVX393V9ZdO8"
        ///     }
        ///
        /// ### Örnek Yanıt
        /// 
        /// Başarılı yanıt aşağıdaki gibi olacaktır:
        ///
        ///     {
        ///         "hostID": "8ymayJzHVVX393V9ZdO8",
        ///         "title": "Kültürlerarası Yemek Festivali",
        ///         "description": "Dünya mutfaklarından lezzetler bu etkinlikte sizleri bekliyor!",
        ///         "location": { "latitude": 41.0082, "longitude": 28.9784 },
        ///         "formattedDate": "30/05/2024",
        ///         "time": "14:00",
        ///         "maxParticipants": 50,
        ///         "images": ["https://example.com/images/festival.jpg"],
        ///         "postStatus": true,
        ///         "relatedFoods": ["Sushi", "Taco"],
        ///         "participants": ["user1", "user3", "user5"]
        ///     }
        ///
        /// ### Hatalar
        /// 
        /// - **404 Not Found**: Belirtilen ID'ye sahip gönderi bulunamazsa bu hata dönülür.
        /// - **500 Internal Server Error**: Gönderi bilgisi getirilirken beklenmedik bir hata oluşursa, bu hata dönülür.
        /// </remarks>
        /// <param name="postId">Getirilecek gönderinin benzersiz tanımlayıcısı.</param>
        /// <returns>HTTP yanıtı olarak, bulunan gönderi nesnesini veya hata mesajını içerir.</returns>
        /// <response code="200">Gönderi başarıyla bulundu ve döndürüldü.</response>
        /// <response code="404">Belirtilen ID'ye sahip gönderi bulunamadı.</response>
        /// <response code="500">Gönderi bilgisi getirilirken bir hata meydana geldi.</response>
        [HttpGet("post")]
        public async Task<IActionResult> GetPostByIdAsync([FromQuery] string postId)
        {
            try
            {
                var post = await _postservice.GetPostByIdAsync(postId);
                if (post == null)
                    return NotFound("Belirtilen ID'ye sahip gönderi bulunamadı.");
                return Ok(post);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Gönderi bilgisi getirilirken bir hata meydana geldi.");
            }
        }
        /// <summary>
        /// Yeni bir gönderi oluşturur.
        /// </summary>
        /// <remarks>
        /// Bu işlem, gönderilen gönderi bilgileriyle yeni bir gönderi kaydı oluşturur. Eğer gönderi bilgileri doğrulanamazsa veya eksikse, 400 Bad Request hatası döner. Gönderi başarıyla oluşturulursa, gönderinin benzersiz tanımlayıcısı döndürülür.
        ///
        /// ### Örnek İstek
        /// 
        ///     POST /api/Post/post
        ///     {
        ///        "hostID": "8ymayJzHVVX393V9ZdO8",
        ///        "title": "Uluslararası Mutfak Festivali",
        ///        "description": "Dünya mutfaklarının en seçkin lezzetlerini deneyimleyin!",
        ///        "location": { "latitude": 40.7128, "longitude": -74.0060 },
        ///        "formattedDate": "01/10/2024",
        ///        "time": "12:00",
        ///        "maxParticipants": 100,
        ///        "images": "https://example.com/images/festival1.jpg",
        ///        "postStatus": true,
        ///        "relatedFoods": ["Sushi", "Taco", "Pizza"],
        ///        "participants": ["user1", "user2"]
        ///     }
        ///
        /// ### Hatalar
        /// 
        /// - **400 Bad Request**: Gönderilen verilerin doğrulaması başarısız olduğunda veya gerekli alanlar eksikse bu hata kodu döner.
        /// - **500 Internal Server Error**: Gönderi oluşturma işlemi sırasında beklenmedik bir hata oluşursa, bu hata dönülür.
        /// </remarks>
        /// <param name="postDto">Oluşturulacak gönderinin bilgilerini içeren DTO nesnesi. DTO, 'hostID', 'title', 'description', 'location', 'formattedDate', 'time', 'maxParticipants', 'images', 'postStatus', 'relatedFoods', ve 'participants' alanlarını içermelidir.</param>
        /// <returns>HTTP yanıtı olarak, başarılı ise gönderinin ID'si, başarısız ise hata mesajı içerir.</returns>
        /// <response code="200">Gönderi başarıyla oluşturuldu ve gönderinin ID'si döndürüldü.</response>
        /// <response code="400">Gönderilen verilerin doğrulaması başarısız olduğunda veya gerekli alanlar eksikse bu hata kodu döner.</response>
        /// <response code="500">Gönderi oluşturma işlemi sırasında beklenmedik bir hata oluştuğunda bu hata dönülür.</response>
        [HttpPost("post")]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDto postDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var postId = await _postservice.CreatePostAsync(postDto);
                if (postId == null)
                    return BadRequest("Gönderilen verilerin doğrulaması başarısız olduğunda veya gerekli alanlar eksikse bu hata kodu döner.");
                return Ok(postId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Gönderi oluşturma işlemi sırasında beklenmedik bir hata oluştuğunda bu hata dönülür.");
            }
        }
        /// <summary>
        /// Belirtilen ID'ye sahip gönderiyi günceller.
        /// </summary>
        /// <remarks>
        /// Bu işlem, belirtilen ID'ye sahip gönderinin bilgilerini günceller. Gönderi bilgileri doğrulanamazsa 400 Bad Request hatası döner. Eğer belirtilen ID'ye sahip gönderi bulunamazsa 404 Not Found hatası döner. Güncelleme başarılı olursa, 200 OK yanıtı ile cevap döner.
        ///
        /// ### Örnek İstek
        /// 
        ///     PUT /api/Post/post
        ///     {
        ///        "postId": "8ymayJzHVVX393V9ZdO8",
        ///        "hostID": "5ymayJzHVVX393V9ZdO8",
        ///        "title": "Kültürel Yemek Festivali Güncellendi",
        ///        "description": "Festivalde yeni yemekler eklendi!",
        ///        "location": { "latitude": 40.7128, "longitude": -74.0060 },
        ///        "formattedDate": "05/10/2024",
        ///        "time": "17:00",
        ///        "maxParticipants": 150,
        ///        "images": ["https://example.com/images/new_festival.jpg"],
        ///        "postStatus": true,
        ///        "relatedFoods": ["Ramen", "Taco", "Pasta"],
        ///        "participants": ["user1", "user2", "user3"]
        ///     }
        ///
        /// ### Hatalar
        /// 
        /// - **400 Bad Request**: Gönderilen verilerin doğrulaması başarısız olduğunda bu hata kodu döner.
        /// - **404 Not Found**: Belirtilen ID'ye sahip gönderi bulunamazsa bu hata dönülür.
        /// - **500 Internal Server Error**: Gönderi güncelleme işlemi sırasında beklenmedik bir hata oluşursa, bu hata dönülür.
        /// </remarks>
        /// <param name="postDto">Güncellenmek istenen gönderinin bilgilerini içeren DTO nesnesi.</param>
        /// <param name="postId">Güncellenmek istenen gönderinin benzersiz tanımlayıcısı.</param>
        /// <returns>HTTP yanıtı olarak, başarılı ise boş bir cevap, başarısız ise hata mesajı içerir.</returns>
        /// <response code="200">Gönderi başarıyla güncellendi.</response>
        /// <response code="400">Gönderilen verilerin doğrulaması başarısız olduğunda bu hata kodu döner.</response>
        /// <response code="404">Belirtilen ID'ye sahip gönderi bulunamadı.</response>
        /// <response code="500">Gönderi güncelleme işlemi sırasında beklenmedik bir hata oluştuğunda bu hata döner.</response>
        [HttpPut("post")]
        public async Task<IActionResult> UpdatePost([FromBody] UpdatePostDto postDto, string postId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var updateResult = await _postservice.UpdatePostAsync(postId, postDto);
                if (!updateResult)
                    return NotFound("Belirtilen ID'ye sahip gönderi bulunamadı.");
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Gönderi güncelleme işlemi sırasında beklenmedik bir hata oluştuğunda bu hata döner.");
            }
        }
        /// <summary>
        /// Belirtilen ID'ye sahip gönderiyi sistemden siler.
        /// </summary>
        /// <remarks>
        /// Bu işlem, sistemde belirtilen ID'ye sahip gönderiyi arar ve siler. Gönderi başarıyla silinirse 204 No Content yanıtı döner. 
        /// Eğer belirtilen ID'ye sahip gönderi bulunamazsa, 404 Not Found hatası döner.
        ///
        /// ### Örnek İstek
        /// 
        ///     DELETE /api/Post/post
        ///     {
        ///         "postId": "8ymayJzHVVX393V9ZdO8"
        ///     }
        ///
        /// ### Hatalar
        /// 
        /// - **204 No Content**: Gönderi başarıyla silindi, hiçbir içerik dönmez.
        /// - **404 Not Found**: Belirtilen ID'ye sahip gönderi bulunamazsa bu hata dönülür.
        /// - **500 Internal Server Error**: Gönderi silme işlemi sırasında beklenmedik bir hata oluşursa, bu hata dönülür.
        /// </remarks>
        /// <param name="postId">Silinmek istenen gönderinin benzersiz tanımlayıcısı.</param>
        /// <returns>HTTP yanıtı olarak, başarılı ise içerik dönmeyen yanıt, başarısız ise hata mesajı içerir.</returns>
        /// <response code="204">Gönderi başarıyla silindi, içerik dönmez.</response>
        /// <response code="404">Belirtilen ID'ye sahip gönderi bulunamadı.</response>
        /// <response code="500">Gönderi silme işlemi sırasında beklenmedik bir hata oluştuğunda bu hata dönülür.</response>

        [HttpDelete("post")]
        public async Task<IActionResult> DeletePost(string postId)
        {
            try
            {
                var deleteResult = await _postservice.DeletePostAsync(postId);
                if (!deleteResult)
                    return NotFound("Belirtilen ID'ye sahip gönderi bulunamadı.");
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Gönderi silme işlemi sırasında beklenmedik bir hata oluştuğunda bu hata dönülür.");
            }
        }
        /// <summary>
        /// Belirtilen kullanıcı ID'sine ait tüm gönderilerin listesini döner.
        /// </summary>
        /// <remarks>
        /// Bu işlem, belirtilen kullanıcı ID'sine ait tüm gönderileri getirir.
        /// Eğer kayıtlı hiç gönderi yoksa, kullanıcıya 404 hatası ile bilgi verilir.
        /// </remarks>
        /// <param name="userId">Gönderileri getirilecek kullanıcının benzersiz tanımlayıcısı (ID).</param>
        /// <returns>Bir HTTP yanıtı döner ki bu, başarılı olursa gönderi listesini, başarısız olursa hata mesajını içerir.</returns>
        /// <response code="200">Gönderiler başarıyla bulundu ve döndürüldü.</response>
        /// <response code="404">Belirtilen ID'ye sahip gönderi bulunamadı.</response>
        /// <response code="500">Gönderi bilgisi getirilirken bir hata meydana geldi.</response>

        [HttpGet("get-user")]
        public async Task<IActionResult> GetByUserIdAsync([FromQuery] string userId)
        {
            try
            {
                var posts = await _postservice.GetByUserIdPostAllAsync(userId);
                if (posts == null || !posts.Any())
                    return NotFound("Belirtilen ID'ye sahip gönderi bulunamadı.");
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Gönderi bilgisi getirilirken bir hata meydana geldi.");
            }
        }
        /// <summary>
        /// Belirtilen kullanıcı ID'sine ait tüm gönderileri döner.
        /// </summary>
        /// <remarks>
        /// Bu işlem, belirtilen kullanıcı ID'sine ait tüm gönderileri getirir.
        /// Eğer kayıtlı hiç gönderi yoksa, kullanıcıya 404 hatası ile bilgi verilir.
        /// </remarks>
        /// <param name="userId">Gönderileri getirilecek kullanıcının benzersiz tanımlayıcısı (ID).</param>
        /// <returns>Bir HTTP yanıtı döner ki bu, başarılı olursa gönderi listesini, başarısız olursa hata mesajını içerir.</returns>
        /// <response code="200">Gönderiler başarıyla bulundu ve döndürüldü.</response>
        /// <response code="404">Belirtilen ID'ye sahip gönderi bulunamadı.</response>
        /// <response code="500">Gönderi bilgisi getirilirken bir hata meydana geldi.</response>
        [HttpGet("get-user-id-posts")]
        public async Task<IActionResult> GetUserIdPost([FromQuery] string userId)
        {
            try
            {
                var posts = await _participantService.GetUserIdPost(userId);
                if (posts == null || !posts.Any())
                    return NotFound("Belirtilen ID'ye sahip gönderi bulunamadı.");
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Gönderi bilgisi getirilirken bir hata meydana geldi.");
            }
        }
        /// <summary>
        /// Belirtilen gönderi ID'lerine sahip gönderilerin listesini döner.
        /// </summary>
        /// <remarks>
        /// Bu işlem, belirtilen gönderi ID'lerine sahip gönderileri getirir.
        /// Eğer kayıtlı hiç gönderi yoksa, kullanıcıya 404 hatası ile bilgi verilir.
        /// </remarks>
        /// <param name="postIds">Gönderileri getirilecek gönderi ID'lerinin listesi.</param>
        /// <returns>Bir HTTP yanıtı döner ki bu, başarılı olursa gönderi listesini, başarısız olursa hata mesajını içerir.</returns>
        /// <response code="200">Gönderiler başarıyla bulundu ve döndürüldü.</response>
        /// <response code="404">Belirtilen ID'ye sahip gönderi bulunamadı.</response>
        /// <response code="500">Gönderi bilgisi getirilirken bir hata meydana geldi.</response>
        [HttpPost("get-user-posts")]
        public async Task<IActionResult> GetPostsByIdsAsync([FromBody] List<string> postIds)
        {
            try
            {
                var posts = await _postservice.GetPostsByIdsAsync(postIds);
                if (posts == null || !posts.Any())
                    return NotFound("Belirtilen ID'ye sahip gönderi bulunamadı.");
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Gönderi bilgisi getirilirken bir hata meydana geldi.");
            }
        }
        /// <summary>
        /// Belirtilen gönderiyi sonlandırır.
        /// </summary>
        /// <remarks>
        /// Bu işlem, belirtilen gönderiyi sonlandırır.
        /// </remarks>
        /// <param name="postId">Sonlandırılacak gönderinin benzersiz tanımlayıcısı (ID).</param>
        /// <returns>Bir HTTP yanıtı döner ki bu, başarılı olursa 200 OK, başarısız olursa hata mesajını içerir.</returns>
        /// <response code="200">Gönderi başarıyla sonlandırıldı.</response>
        /// <response code="404">Belirtilen ID'ye sahip gönderi bulunamadı.</response>
        /// <response code="500">Gönderi güncelleme işlemi sırasında beklenmedik bir hata oluştuğunda bu hata dönülür.</response>
        [HttpPut("end-post")]
        public async Task<IActionResult> EndPost(string postId)
        {
            try
            {
                var endResult = await _postservice.EndPost(postId);
                if (!endResult)
                    return NotFound("Belirtilen ID'ye sahip gönderi bulunamadı.");
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Gönderi güncelleme işlemi sırasında beklenmedik bir hata oluştuğunda bu hata dönülür.");
            }
        }


    }

}