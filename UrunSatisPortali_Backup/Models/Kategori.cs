using System.ComponentModel.DataAnnotations;

namespace UrunSatisPortali.Models // Proje adına göre namespace'i düzelt
{
    public class Kategori
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kategori adı boş bırakılamaz.")]
        public string Ad { get; set; } = "";
    }
}