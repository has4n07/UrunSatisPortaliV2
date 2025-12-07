using Microsoft.AspNetCore.Mvc;
using UrunSatisPortali.Models; // ErrorViewModel için 
using UrunSatisPortali.Repository; // Depo görevlileri için 
using System.Diagnostics;

namespace UrunSatisPortali.Controllers // Proje adýna göre düzelt
{
    // BU KÝLÝTLÝ DEÐÝL. Buraya herkes (giriþ yapmayanlar) eriþebilir.
    public class HomeController : Controller
    {
        // Vitrinin de ürünlere eriþmesi lazým, bu yüzden
        // 'Urun' depo görevlisini buraya da çaðýrýyoruz.
        private readonly IUrunRepository _urunRepository;

        // Dependency Injection ile 'IUrunRepository'yi alýyoruz
        public HomeController(IUrunRepository urunRepository)
        {
            _urunRepository = urunRepository;
        }

        // --- VÝTRÝNÝN ANA SAYFASI ---
        // GET: / veya /Home/Index
        public IActionResult Index()
        {
            // Depo görevlisine git, BÜTÜN ürünleri (kategorileriyle birlikte) al
            var urunler = _urunRepository.GetActive();

            // Modeli (ürün listesini) 'Views/Home/Index.cshtml' sayfasýna gönder
            return View(urunler);
        }

        // --- YENÝ EKLENEN METOT (Detay Sayfasý) ---
        // GET: /Home/Detay/5  (5 = ürünün ID'si)
        public IActionResult Detay(int id)
        {
            // 1. Depo görevlisine git, bu 'id'li ürünü bul
            //    (GetById metodu kategoriyi de .Include() ediyordu, harika)
            var urun = _urunRepository.GetById(id); 

            // 2. Eðer o ID'de bir ürün bulamazsan (veya ürün pasifse)
            //    (Güvenlik için pasif ürünleri de gizleyelim)
            if (urun == null || urun.Aktif == false)
            {
                return NotFound(); // 404 Sayfa Bulunamadý hatasý ver
            }

            // 3. Ürünü bulduysan, 'Detay.cshtml' sayfasýna
            //    bu 'urun' nesnesini model olarak gönder.
            return View(urun);
        }   
        public IActionResult Privacy()
        {
            return View();
        }

        // --- HATA SAYFASI ---
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}