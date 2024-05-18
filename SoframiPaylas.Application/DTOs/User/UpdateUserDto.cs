
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
        public string? UserName { get; set; }
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

        public string? Role { get; set; }

        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Phone { get; set; }
    }
}