using System.ComponentModel.DataAnnotations;

namespace UrunSatisPortali.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email alanı zorunludur")]
        [EmailAddress]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Ad Soyad alanı zorunludur")]
        public required string AdSoyad { get; set; }

        [Required(ErrorMessage = "Şifre alanı zorunludur")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Şifre Tekrar alanı zorunludur")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor")]
        public required string ConfirmPassword { get; set; }
    }
}
