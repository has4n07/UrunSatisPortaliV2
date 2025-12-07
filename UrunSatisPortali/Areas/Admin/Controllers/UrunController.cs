using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UrunSatisPortali.Models;
using UrunSatisPortali.Repository;
using UrunSatisPortali.ViewModels;

namespace UrunSatisPortali.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UrunController(IUrunRepository urunRepo, IKategoriRepository kategoriRepo, IWebHostEnvironment webHostEnvironment) : Controller
    {
        private readonly IUrunRepository _urunRepo = urunRepo;
        private readonly IKategoriRepository _kategoriRepo = kategoriRepo;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        public IActionResult Index()
        {
            var objUrunList = _urunRepo.GetAll(includeProperties: "Kategori").ToList();
            return View(objUrunList);
        }

        public IActionResult Upsert(int? id)
        {
            UrunViewModel urunVM = new()
            {
                KategoriListesi = _kategoriRepo.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Ad,
                    Value = u.Id.ToString()
                }),
                Urun = new Urun()
            };

            if (id == null || id == 0)
            {
                // Create
                return View(urunVM);
            }
            else
            {
                // Update
                var urunFromDb = _urunRepo.Get(u => u.Id == id);
                if (urunFromDb == null)
                {
                    return NotFound();
                }
                urunVM.Urun = urunFromDb;
                return View(urunVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(UrunViewModel urunVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if (!Directory.Exists(productPath))
                    {
                        Directory.CreateDirectory(productPath);
                    }

                    if (!string.IsNullOrEmpty(urunVM.Urun.ResimYolu))
                    {
                        // Delete old image
                        var oldImagePath = Path.Combine(wwwRootPath, urunVM.Urun.ResimYolu.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    urunVM.Urun.ResimYolu = @"\images\product\" + fileName;
                }

                if (urunVM.Urun.Id == 0)
                {
                    _urunRepo.Add(urunVM.Urun);
                }
                else
                {
                    _urunRepo.Update(urunVM.Urun);
                }

                _urunRepo.Save();
                TempData["success"] = "Ürün başarıyla kaydedildi";
                return RedirectToAction("Index");
            }
            else
            {
                urunVM.KategoriListesi = _kategoriRepo.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Ad,
                    Value = u.Id.ToString()
                });
                return View(urunVM);
            }
        }

        // API CALLS for AJAX
        [HttpGet]
        public IActionResult GetAll()
        {
            var objUrunList = _urunRepo.GetAll(includeProperties: "Kategori").ToList();
            return Json(new { data = objUrunList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var urunToBeDeleted = _urunRepo.Get(u => u.Id == id);
            if (urunToBeDeleted == null)
            {
                return Json(new { success = false, message = "Silme işlemi başarısız" });
            }

            if (!string.IsNullOrEmpty(urunToBeDeleted.ResimYolu))
            {
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, urunToBeDeleted.ResimYolu.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            _urunRepo.Remove(urunToBeDeleted);
            _urunRepo.Save();

            return Json(new { success = true, message = "Silme işlemi başarılı" });
        }
    }
}
