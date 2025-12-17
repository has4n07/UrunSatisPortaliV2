using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrunSatisPortali.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kategori adı zorunludur.")]

        [Display(Name = "Kategori Adı")]
        public string? Name { get; set; }

        public List<Product> Products { get; set; } = [];
    }
}
