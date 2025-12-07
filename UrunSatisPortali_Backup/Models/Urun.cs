using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Http;    
using System.ComponentModel.DataAnnotations.Schema; 

namespace UrunSatisPortali.Models
{
    public class Urun
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ürün adı boş bırakılamaz.")]
        [Display(Name = "Ürün Adı")]
        public string Ad { get; set; } = "";

        [Required(ErrorMessage = "Fiyat alanı boş bırakılamaz.")]
        [Range(1, 10000000, ErrorMessage = "Fiyat 1'den büyük olmalıdır.")]
        public decimal Fiyat { get; set; }

        [Required(ErrorMessage = "Stok alanı boş bırakılamaz.")]
        [Range(0, 100000, ErrorMessage = "Stok 0'dan az olamaz.")]
        public int Stok { get; set; }

        [Display(Name = "Açıklama")]
        public string? Aciklama { get; set; } 

        [Range(1, int.MaxValue, ErrorMessage = "Lütfen bir kategori seçiniz.")]
        [Display(Name = "Kategori")]
        public int KategoriId { get; set; }

        
        [ValidateNever]
        public Kategori? Kategori { get; set; }
       
        
        

        [Display(Name = "Aktif Mi?")]
        public bool Aktif { get; set; }

        [ValidateNever]
        public List<UrunResmi>? UrunResimleri { get; set; }
    }
}