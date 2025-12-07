using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrunSatisPortali.Models;
using UrunSatisPortali.Repository;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace UrunSatisPortali.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class KategoriController(IKategoriRepository kategoriRepo, IUrunRepository urunRepo, INotyfService notyf) : Controller
    {
        private readonly IKategoriRepository _kategoriRepo = kategoriRepo;
        private readonly IUrunRepository _urunRepo = urunRepo;
        private readonly INotyfService _notyf = notyf;

        public IActionResult Index()
        {
            var objKategoriList = _kategoriRepo.GetAll().ToList();
            return View(objKategoriList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Kategori obj)
        {
            if (ModelState.IsValid)
            {
                _kategoriRepo.Add(obj);
                _kategoriRepo.Save();
                _notyf.Success("Kategori başarıyla oluşturuldu");
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Kategori? kategoriFromDb = _kategoriRepo.Get(u => u.Id == id);

            if (kategoriFromDb == null)
            {
                return NotFound();
            }
            return View(kategoriFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Kategori obj)
        {
            if (ModelState.IsValid)
            {
                _kategoriRepo.Update(obj);
                _kategoriRepo.Save();
                _notyf.Success("Kategori başarıyla güncellendi");
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Kategori? kategoriFromDb = _kategoriRepo.Get(u => u.Id == id);

            if (kategoriFromDb == null)
            {
                return NotFound();
            }
            return View(kategoriFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Kategori? obj = _kategoriRepo.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            // Check if there are products in this category
            var productCount = _urunRepo.GetAll(u => u.KategoriId == id).Count();
            if (productCount > 0)
            {
                _notyf.Error("Bu kategoride ürünler var, silinemez!");
                return RedirectToAction("Index");
            }

            _kategoriRepo.Remove(obj);
            _kategoriRepo.Save();
            _notyf.Success("Kategori başarıyla silindi");
            return RedirectToAction("Index");
        }
    }
}
