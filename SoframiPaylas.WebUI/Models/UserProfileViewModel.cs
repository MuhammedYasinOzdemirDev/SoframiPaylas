using System.ComponentModel.DataAnnotations;

namespace SoframiPaylas.WebUI.Models;

public class UserProfileViewModel
{

    [Required(ErrorMessage = "Email alanı zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
    [Display(Name = "Email Adresi")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
    [StringLength(50, ErrorMessage = "Kullanıcı adı en fazla 50 karakter olabilir.")]
    [Display(Name = "Kullanıcı Adı")]
    public string? UserName { get; set; }

    [Display(Name = "Ev Sahibi mi?")]
    public bool IsHost { get; set; }

    [Display(Name = "Profil Resmi")]
    public string? ProfilePicture { get; set; }

    [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
    [Display(Name = "Telefon Numarası")]
    public string? Phone { get; set; }
    public string? Role { get; set; }

    [StringLength(500, ErrorMessage = "Hakkında bölümü en fazla 500 karakter olabilir.")]
    [Display(Name = "Hakkında")]
    public string? About { get; set; }


    [Required(ErrorMessage = "İsim alanı zorunludur.")]
    [StringLength(100, ErrorMessage = "İsim en fazla 100 karakter olabilir.")]
    [Display(Name = "İsim")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Soyisim alanı zorunludur.")]
    [StringLength(100, ErrorMessage = "Soyisim en fazla 100 karakter olabilir.")]
    [Display(Name = "Soyisim")]
    public string Surname { get; set; }
}