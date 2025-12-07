using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace UrunSatisPortali.Models // Proje adına göre namespace'i kontrol et
{
    // ApplicationUser'dan miras alarak tüm hazır özelliklerini (Email, PasswordHash vb.) koruyoruz.
    public class ApplicationUser : IdentityUser
    {
        // Zaten eklemiş olmalıydık, kontrol et
        public string AdSoyad { get; set; } = "";

        // YENİ: Profil Fotoğrafı Yolu
        public string? ProfilResmiYolu { get; set; } = "/img/default-avatar.png"; // Varsayılan resim
        public string? ProfilResmiUrl { get; set; }
    }
}