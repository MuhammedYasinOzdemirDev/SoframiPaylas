using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace SoframiPaylas.Application.DTOs.Food
{
    public class CreateFoodDto
    {
        /// <summary>
        /// Yiyecek gönderisinin benzersiz tanımlayıcısı.
        /// </summary>
        /// <example>VNRPesXcUL01AOaqFouk</example>
        public string PostID { get; set; }
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
        /// <summary>
        /// Yiyeceğin görselini içeren resim URL'si.
        /// </summary>
        /// <example>https://example.com/images/lahmacun.jpg</example>
        public string Images { get; set; }
    }
}