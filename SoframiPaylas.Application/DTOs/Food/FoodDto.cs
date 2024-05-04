using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoframiPaylas.Application.DTOs.Food
{
    public class FoodDto
    {
        /// <summary>
        /// Yiyecek gönderisinin benzersiz tanımlayıcısı.
        /// </summary>
        /// <example>VNRPesXcUL01AOaqFouk</example>
        public string PostID { get; set; }
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

        /// <summary>
        /// Yiyeceğin görselini içeren resim URL'si.
        /// </summary>
        /// <example>https://example.com/images/iskender.jpg</example>
        public string Images { get; set; }
    }
}