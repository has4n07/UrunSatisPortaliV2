

using Microsoft.AspNetCore.Identity;

namespace UrunSatisPortali.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? NameSurname { get; set; }
        public string Role { get; set; } = "User";
        public string? ProfilePhotoPath { get; set; }
        public string? Address { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Gender { get; set; }
    }
}
