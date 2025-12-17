using System.ComponentModel.DataAnnotations;

namespace UrunSatisPortali.ViewModels
{
    public class ProfileViewModel
    {
        [Required(ErrorMessage = "Ad Soyad alanı zorunludur.")]
        [Display(Name = "Ad Soyad")]
        public string NameSurname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Profil Fotoğrafı")]
        public IFormFile? ProfilePhoto { get; set; }

        public string? CurrentProfilePhotoPath { get; set; }

        [Display(Name = "Telefon Numarası")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Adres")]
        public string? Address { get; set; }

        [Display(Name = "Doğum Tarihi")]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Cinsiyet")]
        public string? Gender { get; set; }
    }
}
