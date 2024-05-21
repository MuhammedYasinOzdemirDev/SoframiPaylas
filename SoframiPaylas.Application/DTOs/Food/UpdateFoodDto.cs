
namespace SoframiPaylas.Application.DTOs.Food
{
    public class UpdateFoodDto
    {

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


    }
}