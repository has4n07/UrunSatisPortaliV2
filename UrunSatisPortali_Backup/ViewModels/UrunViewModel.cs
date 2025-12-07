using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using UrunSatisPortali.Models;
using System.Collections.Generic;

namespace UrunSatisPortali.ViewModels
{
    public class UrunViewModel
    {
        // 'Urun' nesnesinin 'null' olmasını engeller
        public UrunViewModel()
        {
            Urun = new Urun();
        }

        public Urun Urun { get; set; }

        // 'ModelState'in bu listeyi denetlemesini engeller
        [ValidateNever]
        public IEnumerable<SelectListItem> KategoriListesi { get; set; } = new List<SelectListItem>();
    }
}