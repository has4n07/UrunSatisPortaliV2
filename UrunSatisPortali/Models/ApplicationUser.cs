

namespace UrunSatisPortali.Models
{
    public class ApplicationUser
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? AdSoyad { get; set; }
        public string Role { get; set; } = "User";
        public string? ProfilePhotoPath { get; set; }
    }
}
