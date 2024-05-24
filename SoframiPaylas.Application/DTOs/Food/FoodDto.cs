
namespace SoframiPaylas.Application.DTOs.Food
{
    public class FoodDto
    {

        public string FoodId { get; set; }
        /// <summary>
        /// Yiyeceğin başlığı.
        /// </summary>
        /// <example>İskender Kebap</example>
        public string Title { get; set; }
        /// <summary>
        /// Yiyeceğin açıklaması.
        /// </summary>
        /// <example>İskender Kebap, döner kebap üzerine yoğurt ve tereyağlı domates sosu ile servis edilen, Bursa bölgesine özgü bir yemektir.</example>
        public string Description { get; set; }


    }
}