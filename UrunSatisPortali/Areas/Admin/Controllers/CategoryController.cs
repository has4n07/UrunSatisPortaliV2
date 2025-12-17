using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrunSatisPortali.Models;
using UrunSatisPortali.Repository;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace UrunSatisPortali.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController(ICategoryRepository categoryRepo, IProductRepository productRepo, INotyfService notyf) : Controller
    {
        private readonly ICategoryRepository _categoryRepo = categoryRepo;
        private readonly IProductRepository _productRepo = productRepo;
        private readonly INotyfService _notyf = notyf;

        public IActionResult Index()
        {
            var objCategoryList = _categoryRepo.GetAll().ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                _categoryRepo.Add(obj);
                _categoryRepo.Save();
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
            Category? categoryFromDb = _categoryRepo.Get(u => u.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _categoryRepo.Update(obj);
                _categoryRepo.Save();
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
            Category? categoryFromDb = _categoryRepo.Get(u => u.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _categoryRepo.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            // Check if there are products in this category
            var productCount = _productRepo.GetAll(u => u.CategoryId == id).Count();
            if (productCount > 0)
            {
                _notyf.Error("Bu kategoride ürünler var, silinemez!");
                return RedirectToAction("Index");
            }

            _categoryRepo.Remove(obj);
            _categoryRepo.Save();
            _notyf.Success("Kategori başarıyla silindi");
            return RedirectToAction("Index");
        }
    }
}
