using System.ComponentModel.DataAnnotations;

namespace UrunSatisPortali.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email alanı zorunludur")]
        [EmailAddress]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Şifre alanı zorunludur")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
