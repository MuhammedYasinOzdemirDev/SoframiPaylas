
namespace SoframiPaylas.Application.DTOs
{
    public class CreateUserDto
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
        /// Kullanıcının ev sahibi olup olmadığını belirtir.
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
        /// <example>Hobileri, ilgi alanları gibi bilgiler.</example>
        public string? About { get; set; }

        public string Role { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
    }
}