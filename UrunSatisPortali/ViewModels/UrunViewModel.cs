using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using UrunSatisPortali.Models;

namespace UrunSatisPortali.ViewModels
{
    public class UrunViewModel
    {
        public Urun Urun { get; set; } = new();

        [ValidateNever]
        public IEnumerable<SelectListItem> KategoriListesi { get; set; } = [];
    }
}
