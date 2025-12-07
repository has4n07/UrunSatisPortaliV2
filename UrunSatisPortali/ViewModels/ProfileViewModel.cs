using System.ComponentModel.DataAnnotations;

namespace UrunSatisPortali.ViewModels
{
    public class ProfileViewModel
    {
        [Required(ErrorMessage = "Ad Soyad alanı zorunludur.")]
        [Display(Name = "Ad Soyad")]
        public string AdSoyad { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Profil Fotoğrafı")]
        public IFormFile? ProfilePhoto { get; set; }

        public string? CurrentProfilePhotoPath { get; set; }
    }
}
