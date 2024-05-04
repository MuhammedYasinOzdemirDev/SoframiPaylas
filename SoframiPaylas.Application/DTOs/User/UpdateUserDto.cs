using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoframiPaylas.Application.DTOs
{
    public class UpdateUserDto
    {
        /// <summary>
        /// Güncellenmek istenen kullanıcının e-posta adresi.
        /// </summary>
        /// <example>updated_user@example.com</example>
        public string? Email { get; set; }
        /// <summary>
        /// Güncellenmek istenen kullanıcının tam adı.
        /// </summary>
        /// <example>Mehmet Öz</example>
        public string? FullName { get; set; }
        /// <summary>
        /// Kullanıcının ev sahibi olup olmadığı bilgisinin güncellenmesi.
        /// </summary>
        /// <example>false</example>
        public bool IsHost { get; set; }
        /// <summary>
        /// Kullanıcının güncellenmiş profil resmi URL'si.
        /// </summary>
        /// <example>https://example.com/images/updated_profile.jpg</example>
        public string? ProfilePicture { get; set; }
        /// <summary>
        /// Kullanıcı hakkında güncellenmiş ek bilgiler.
        /// </summary>
        /// <example>Yeni eklenen hobiler, değişen ilgi alanları gibi bilgiler.</example>
        public string? About { get; set; }
    }
}