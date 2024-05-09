using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoframiPaylas.Application.DTOs
{
    public class UserDto
    {
        /// <summary>
        /// Kullanıcının e-posta adresi.
        /// </summary>
        /// <example>user@example.com</example>
        public string? Email { get; set; }
        /// <summary>
        /// Kullanıcının tam adı.
        /// </summary>
        /// <example>Ahmet Yılmaz</example>
        public string? UserName { get; set; }
        /// <summary>
        /// Kullanıcının ev sahibi olup olmadığını gösterir.
        /// </summary>
        /// <example>true</example>
        public bool IsHost { get; set; }
        /// <summary>
        /// Kullanıcının profil resmi URL'si.
        /// </summary>
        /// <example>https://example.com/images/profile.jpg</example>
        public string? ProfilePicture { get; set; }
        /// <summary>
        /// Kullanıcı hakkında ek bilgiler.
        /// </summary>
        /// <example>Kısa biyografi ve ilgi alanları.</example>
        public string? About { get; set; }

        public string Role { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}