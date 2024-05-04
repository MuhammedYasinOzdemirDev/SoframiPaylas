using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using SoframiPaylas.Domain.Entities;

namespace SoframiPaylas.Application.DTOs.Post
{
    public class CreatePostDto
    {

        /// <summary>
        /// Gönderiyi oluşturan kullanıcının ID'si.
        /// </summary>
        /// <example>12345</example>
        public string HostID { get; set; }

        /// <summary>
        /// Gönderinin başlığı.
        /// </summary>
        /// <example>Yemek Paylaşım Etkinliği</example>
        public string Title { get; set; }

        /// <summary>
        /// Gönderi açıklaması.
        /// </summary>
        /// <example>Bu etkinlikte ev yapımı yemeklerimizi paylaşıyoruz!</example>
        public string Description { get; set; }

        /// <summary>
        /// Gönderinin gerçekleşeceği lokasyon.
        /// </summary>
        /// <example>{ Latitude = 41.0082, Longitude = 28.9784 }</example>
        public GeoPoint Location { get; set; }

        /// <summary>
        /// Gönderinin tarih bilgisi.
        /// </summary>
        /// <example>23/04/2024</example>
        public string FormattedDate { get; set; }

        /// <summary>
        /// Etkinliğin başlama saati.
        /// </summary>
        /// <example>18:30</example>
        public string Time { get; set; }

        /// <summary>
        /// Maksimum katılımcı sayısı.
        /// </summary>
        /// <example>20</example>
        public int MaxParticipants { get; set; }

        /// <summary>
        /// Gönderi ile ilişkili resimlerin listesi.
        /// </summary>
        /// <example>["https://example.com/images/event1.jpg", "https://example.com/images/event2.jpg"]</example>
        public List<string> Images { get; set; }

        /// <summary>
        /// Gönderinin aktif olup olmadığını gösterir.
        /// </summary>
        /// <example>true</example>
        public bool PostStatus { get; set; }

        /// <summary>
        /// Gönderi ile ilişkili yemeklerin listesi.
        /// </summary>
        /// <example>["Lahmacun", "Baklava"]</example>
        public List<string> RelatedFoods { get; set; }

        /// <summary>
        /// Etkinliğe katılan kullanıcıların listesi.
        /// </summary>
        /// <example>["user1", "user2"]</example>
        public List<string> Participants { get; set; }
    }
}