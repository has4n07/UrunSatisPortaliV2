using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UrunSatisPortali.Models;
using UrunSatisPortali.Repository;

namespace UrunSatisPortali.Controllers
{
    public class HomeController(ILogger<HomeController> logger, IUrunRepository urunRepo) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly IUrunRepository _urunRepo = urunRepo;

        public IActionResult Index()
        {
            var urunListesi = _urunRepo.GetAll(includeProperties: "Kategori");
            return View(urunListesi);
        }

        public IActionResult Details(int productId)
        {
            var urun = _urunRepo.Get(u => u.Id == productId, includeProperties: "Kategori");
            if (urun == null)
            {
                return NotFound();
            }
            return View(urun);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
