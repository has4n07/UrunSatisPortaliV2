using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace UrunSatisPortali.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ürün adı zorunludur.")]
        [Display(Name = "Ürün Adı")]
        public string? Name { get; set; }

        [Display(Name = "Açıklama")]
        public string Description { get; set; } = "";

        [Required(ErrorMessage = "Fiyat zorunludur.")]
        [Range(0, double.MaxValue, ErrorMessage = "Geçerli bir fiyat giriniz.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stok zorunludur.")]
        [Range(0, int.MaxValue, ErrorMessage = "Geçerli bir stok giriniz.")]
        public int Stock { get; set; }

        [Display(Name = "Resim")]
        public string? ImagePath { get; set; }

        [Display(Name = "Kategori")]
        [Column("CategoryId")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category? Category { get; set; }
    }
}
