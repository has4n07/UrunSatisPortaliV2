using System.ComponentModel.DataAnnotations;

namespace UrunSatisPortali.ViewModels
{
    public class RoleViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Rol adı gereklidir.")]
        [Display(Name = "Rol Adı")]
        public string Name { get; set; } = string.Empty;
    }

    public class UserRolesViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<RoleSelection> Roles { get; set; } = new List<RoleSelection>();
    }

    public class RoleSelection
    {
        public string RoleName { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
    }
}
