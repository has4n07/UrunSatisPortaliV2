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
    public class ProductController(IProductRepository productRepo, ICategoryRepository categoryRepo, IWebHostEnvironment webHostEnvironment) : Controller
    {
        private readonly IProductRepository _productRepo = productRepo;
        private readonly ICategoryRepository _categoryRepo = categoryRepo;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        public IActionResult Index()
        {
            var objProductList = _productRepo.GetAll(includeProperties: "Category").ToList();
            // Optional: Filter low stock for a dashboard view if requested, but user asked for "stock tracking".
            // The JS update already highlights low stock in the main list.
            return View(objProductList);
        }

        public IActionResult Upsert(int? id)
        {
            ProductViewModel productVM = new()
            {
                CategoryList = _categoryRepo.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };

            if (id == null || id == 0)
            {
                // Create
                return View(productVM);
            }
            else
            {
                // Update
                var productFromDb = _productRepo.Get(u => u.Id == id);
                if (productFromDb == null)
                {
                    return NotFound();
                }
                productVM.Product = productFromDb;
                return View(productVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductViewModel productVM, IFormFile? file)
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

                    if (!string.IsNullOrEmpty(productVM.Product.ImagePath))
                    {
                        // Delete old image
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImagePath.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVM.Product.ImagePath = @"\images\product\" + fileName;
                }

                if (productVM.Product.Id == 0)
                {
                    _productRepo.Add(productVM.Product);
                }
                else
                {
                    _productRepo.Update(productVM.Product);
                }

                _productRepo.Save();
                TempData["success"] = "Ürün başarıyla kaydedildi";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _categoryRepo.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productVM);
            }
        }

        // API CALLS for AJAX
        [HttpGet]
        public IActionResult GetAll()
        {
            var objProductList = _productRepo.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _productRepo.Get(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Silme işlemi başarısız" });
            }

            if (!string.IsNullOrEmpty(productToBeDeleted.ImagePath))
            {
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ImagePath.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            _productRepo.Remove(productToBeDeleted);
            _productRepo.Save();

            return Json(new { success = true, message = "Silme işlemi başarılı" });
        }
    }
}
