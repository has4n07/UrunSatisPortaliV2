using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrunSatisPortali.Models
{
    public class Kategori
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kategori adı zorunludur.")]
        [Display(Name = "Kategori Adı")]
        [Column("Name")]
        public string? Ad { get; set; }

        public List<Urun> Urunler { get; set; } = [];
    }
}
