using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace UrunSatisPortali.Models
{
    public class Urun
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ürün adı zorunludur.")]
        [Display(Name = "Ürün Adı")]
        [Column("Name")]
        public string? Ad { get; set; }

        [Display(Name = "Açıklama")]
        [Column("Description")]
        public string Aciklama { get; set; } = "";

        [Required(ErrorMessage = "Fiyat zorunludur.")]
        [Range(0, double.MaxValue, ErrorMessage = "Geçerli bir fiyat giriniz.")]
        [Column("Price")]
        public decimal Fiyat { get; set; }

        [Required(ErrorMessage = "Stok zorunludur.")]
        [Range(0, int.MaxValue, ErrorMessage = "Geçerli bir stok giriniz.")]
        [Column("Stock")]
        public int Stok { get; set; }

        [Display(Name = "Resim")]
        [Column("ImageUrl")]
        public string? ResimYolu { get; set; }

        [Display(Name = "Kategori")]
        [Column("CategoryId")]
        public int KategoriId { get; set; }

        [ForeignKey("KategoriId")]
        [ValidateNever]
        public Kategori? Kategori { get; set; }
    }
}
