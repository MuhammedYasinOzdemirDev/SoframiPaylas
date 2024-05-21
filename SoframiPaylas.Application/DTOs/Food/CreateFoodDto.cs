
namespace SoframiPaylas.Application.DTOs.Food
{
    public class CreateFoodDto
    {

        /// <summary>
        /// Yiyeceğin başlığı.
        /// </summary>
        /// <example>Lahmacun</example>
        public string Title { get; set; }
        /// <summary>
        /// Yiyeceğin açıklaması.
        /// </summary>
        /// <example>Lahmacun, ince hamur üzerine kıyma, soğan, domates ve baharatların karışımı ile hazırlanan, özellikle Gaziantep bölgesine ait bir Türk yemegidir.</example>
        public string Description { get; set; }


    }
}