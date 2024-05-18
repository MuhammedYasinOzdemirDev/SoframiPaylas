using System.ComponentModel.DataAnnotations;

namespace SoframiPaylas.WebUI.Models;
public class RegisterViewModel
{

    [Required(ErrorMessage = "Ad alanı gereklidir.")]
    [Display(Name = "Ad")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Soyad alanı gereklidir.")]
    [Display(Name = "Soyad")]
    public string Surname { get; set; }

    [Required(ErrorMessage = "Kullanıcı adı gereklidir.")]
    [Display(Name = "Kullanıcı Adı")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "E-posta adresi gereklidir.")]
    [EmailAddress(ErrorMessage = "Geçersiz e-posta adresi.")]
    [Display(Name = "E-posta")]
    public string Email { get; set; }
    [Display(Name = "Telefon")]
    [Required(ErrorMessage = "Telefon numarası gereklidir.")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "Şifre gereklidir.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Şifre en az 8 karakter olmalıdır.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessage = "Şifre en az bir büyük harf, bir küçük harf ve bir rakam içermelidir.")]
    [Display(Name = "Şifre")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Şifre tekrarı gereklidir.")]
    [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
    [Display(Name = "Şifre Tekrar")]
    public string ConfirmPassword { get; set; }
}