
namespace SoframiPaylas.Application.DTOs.Food
{
    public class UpdateFoodDto
    {
        /// <summary>
        /// Güncellenecek yiyecek gönderisinin benzersiz tanımlayıcısı.
        /// </summary>
        /// <example>VNRPesXcUL01AOaqFouk</example>
        public string PostID { get; set; }
        /// <summary>
        /// Yiyeceğin güncellenen başlığı.
        /// </summary>
        /// <example>Baklava</example>
        public string Title { get; set; }
        /// <summary>
        /// Yiyeceğin güncellenen açıklaması.
        /// </summary>
        /// <example>Baklava, ince yufkalar arasında ceviz veya fıstık ile yapılan, şerbetli bir Türk tatlısıdır.</example>
        public string Description { get; set; }
        /// <summary>
        /// Yiyeceğin güncellenen görselini içeren resim URL'si.
        /// </summary>
        /// <example>https://example.com/images/baklava.jpg</example>
        public string Images { get; set; }
    }
}