// --- Gerekli Tüm Kütüphaneler ---
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http; // IFormCollection (ihtiyaç olursa) için
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // SelectListItem için
using System;
using System.Collections.Generic;
using System.Linq;
using UrunSatisPortali.Models;      // Proje adın 'UrunSatisTEMIZ' ise burayı düzelt
using UrunSatisPortali.Repository;
using UrunSatisPortali.Data;
using Microsoft.AspNetCore.Hosting; 
using System.IO; 

// ÖNEMLİ: 'namespace' adının YENİ projenle eşleştiğinden emin ol
// (örn: UrunSatisPortaliTEMIZ.Controllers)
namespace UrunSatisPortali.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        // --- Depo Görevlilerimiz ---
        private readonly IKategoriRepository _kategoriRepository;
        private readonly IUrunRepository _urunRepository;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env; // <-- YENİ EKLENDİ

        public AdminController(IKategoriRepository kategoriRepository, IUrunRepository urunRepository, ApplicationDbContext context, IWebHostEnvironment env) // <-- YENİ PARAMETRE EKLENDİ
        {
            _kategoriRepository = kategoriRepository;
            _urunRepository = urunRepository;
            _context = context;
            _env = env; // <-- EKLENDİ
        }

        public IActionResult Index()
        {
            return View();
        }

        // --- #################################### ---
        // ---        KATEGORİ CRUD İŞLEMLERİ       ---
        // --- #################################### ---

        // Controllers/AdminController.cs
        public IActionResult Kategoriler()
        {
            var kategoriler = _kategoriRepository.GetAll(); // Veriyi çeker
            return View(kategoriler); // View'a gönderir
        }

        public IActionResult KategoriEkle()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult KategoriEkle(Kategori kategori)
        {
            if (ModelState.IsValid)
            {
                _kategoriRepository.Add(kategori);
                return RedirectToAction(nameof(Kategoriler));
            }
            return View(kategori);
        }

        public IActionResult KategoriDuzenle(int id)
        {
            var k = _kategoriRepository.GetById(id);
            if (k == null) { return NotFound(); }
            return View(k);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult KategoriDuzenle(int id, Kategori kategori)
        {
            if (id != kategori.Id) { return BadRequest(); }
            if (ModelState.IsValid)
            {
                _kategoriRepository.Update(kategori);
                return RedirectToAction(nameof(Kategoriler));
            }
            return View(kategori);
        }

        // --- ESKİ SİLME METOTLARI (Artık AJAX kullanıyoruz ama dursunlar) ---
        public IActionResult KategoriSil(int id)
        {
            var k = _kategoriRepository.GetById(id);
            if (k == null) { return NotFound(); }
            return View(k); // Bu, KategoriSil.cshtml onay sayfasını açar
        }
        [HttpPost, ActionName("KategoriSil")]
        [ValidateAntiForgeryToken]
        public IActionResult KategoriSilOnay(int id)
        {
            _kategoriRepository.Delete(id);
            return RedirectToAction(nameof(Kategoriler));
        }

        // --- #################################### ---
        // ---      YENİ AJAX SİLME METODU (ADIM 8)   ---
        // POST: /Admin/KategoriSilAJAX/5 (AJAX için)
        [HttpPost]
        // [ValidateAntiForgeryToken] // AJAX için kaldırmıştık
        public IActionResult KategoriSilAJAX(int id)
        {
            try
            {
                // --- GÜVENLİK KONTROLÜ BAŞLANGICI ---
                // 1. Ürün depo görevlisine sor: Bu kategoriye bağlı ürün var mı?
                if (_urunRepository.KategoriyeAitUrunVarMi(id))
                {
                    // 2. Eğer "EVET" derse, SİLME! Hata mesajı gönder.
                    return Json(new { success = false, message = "Bu kategoriye bağlı ürünler (örn: Laptop) varken kategori silinemez! Önce ürünleri başka kategoriye taşıyın veya silin." });
                }
                // --- GÜVENLİK KONTROLÜ BİTTİ ---

                // 3. Eğer "HAYIR" derse, güvenle sil.
                _kategoriRepository.Delete(id);

                // Başarılı olursa, JavaScript'e "başarılı" JSON mesajı gönder
                return Json(new { success = true, message = "Kategori başarıyla silindi." });
            }
            catch (Exception ex)
            {
                // Başarısız olursa, "başarısız" JSON mesajı gönder
                return Json(new { success = false, message = "Hata oluştu: " + ex.Message });
            }
        }
        // --- #################################### ---
        // ---          ÜRÜN CRUD İŞLEMLERİ         ---
        // --- #################################### ---
        // (ViewBag Yöntemi - KategoriId=0 hatasını çözen)

        public IActionResult Urunler()
        {
            var u = _urunRepository.GetAll();
            return View(u);
        }

        // GET: UrunEkle
        public IActionResult UrunEkle()
        {
            ViewBag.KategoriListesi = KategoriListesiniDoldur();
            return View(new Urun());
        }

        // POST: UrunEkle (Kaydetme - YENİ Dosya Yüklemeli Versiyon)
        // POST: UrunEkle (Kaydetme - YENİ ÇOKLU Dosya Yüklemeli Versiyon)
        [HttpPost]
        [ValidateAntiForgeryToken]
        // DİKKAT: Parametre listesine 'List<IFormFile> ResimDosyalari' eklendi.
        // 'name="ResimDosyalari"' (formdaki) ile 'ResimDosyalari' (buradaki) eşleşmeli.
        public IActionResult UrunEkle(Urun urun, List<IFormFile> ResimDosyalari)
        {
            try
            {
                // Model geçerli mi? (Fiyat, KategoriId vb.)
                if (ModelState.IsValid)
                {
                    // 1. Önce Ürünün KENDİSİNİ kaydet (ki bir ID alsın)
                    // (Henüz resim eklemedik)
                    _urunRepository.Add(urun);

                    // 'urun.Id' artık veritabanından gelen yeni ID'yi içerir.
                    // (Not: Bu, _urunRepository.Add metodunun 'urun' nesnesini
                    // ID ile güncellemesine bağlıdır, EF Core bunu otomatik yapar.)

                    // --- Çoklu Dosya Yükleme Kodu ---

                    // 2. Bir dosya yüklendi mi?
                    if (ResimDosyalari != null && ResimDosyalari.Count > 0)
                    {
                        // 3. Yüklenen HER BİR dosya için dön
                        foreach (var dosya in ResimDosyalari)
                        {
                            // 4. Dosyaya benzersiz bir isim oluştur
                            var dosyaAdi = Guid.NewGuid().ToString() + Path.GetExtension(dosya.FileName);

                            // 1. Güvenli wwwroot yolunu al ve uploads klasörüne ekle
                            var uploadKlasoru = Path.Combine(_env.WebRootPath, "uploads");

                            // 2. Klasör yoksa oluştur (İzin hatasını engeller)
                            if (!Directory.Exists(uploadKlasoru))
                            {
                                Directory.CreateDirectory(uploadKlasoru);
                            }

                            // 3. Dosyanın kaydedileceği nihai yolu belirle
                            var kaydetmeYolu = Path.Combine(uploadKlasoru, dosyaAdi);
                            // 6. Dosyayı o yola kaydet
                            using (var stream = new FileStream(kaydetmeYolu, FileMode.Create))
                            {
                                dosya.CopyTo(stream);
                            }

                            // 7. YENİ 'UrunResmi' nesnesi oluştur
                            var urunResmi = new UrunResmi
                            {
                                ResimYolu = "/uploads/" + dosyaAdi,
                                UrunId = urun.Id // Bu resmi, az önce kaydettiğimiz 'urun.Id'ye bağla
                            };

                            // 8. Resim yolunu YENİ 'UrunResimleri' tablosuna kaydet
                            _context.UrunResimleri.Add(urunResmi); // <-- Bu satır için DbContext'e ihtiyacımız var
                        }

                        // 9. Tüm resim yollarını veritabanına işle
                        _context.SaveChanges();
                    }

                    return RedirectToAction(nameof(Urunler));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Kaydetme hatası: " + ex.Message);
            }

            // --- Hata Varsa ---
            ViewBag.KategoriListesi = KategoriListesiniDoldur();
            return View(urun);
        }

        // GET: UrunDuzenle
        public IActionResult UrunDuzenle(int id)
        {
            var urun = _urunRepository.GetById(id);
            if (urun == null) { return NotFound(); }
            ViewBag.KategoriListesi = KategoriListesiniDoldur();
            return View(urun);
        }

        // POST: UrunDuzenle
        // POST: UrunDuzenle (Güncelleme - Artık List<IFormFile> alacak)
        [HttpPost]
        [ValidateAntiForgeryToken]
        // PARAMETRE DEĞİŞTİ: List<IFormFile> eklendi
        public IActionResult UrunDuzenle(Urun urun, List<IFormFile> ResimDosyalari)
        {
            try
            {
                // Controllers/AdminController.cs - inside [HttpPost] UrunDuzenle

                if (ModelState.IsValid)
                {
                    // 1. Ürünün metin bilgilerini güncelle (Ad, Fiyat, Stok, KategoriId vb.)
                    // Bu, formdan gelen metin verilerini kaydeder.
                    _urunRepository.Update(urun);

                    // 2. Yeni resim dosyaları yüklendi mi? (Çoklu Yükleme Kontrolü)
                    if (ResimDosyalari != null && ResimDosyalari.Count > 0)
                    {
                        // 3. Yüklenen HER BİR dosya için dön
                        foreach (var dosya in ResimDosyalari)
                        {
                            // 4. Dosya Kaydetme İşlemleri (Benzersiz isim ve yol oluşturma)
                            var dosyaAdi = Guid.NewGuid().ToString() + Path.GetExtension(dosya.FileName);

                            // Güvenli wwwroot/uploads yolunu al
                            var uploadKlasoru = Path.Combine(_env.WebRootPath, "uploads");

                            if (!Directory.Exists(uploadKlasoru))
                            {
                                Directory.CreateDirectory(uploadKlasoru);
                            }
                            var kaydetmeYolu = Path.Combine(uploadKlasoru, dosyaAdi);

                            // 5. Dosyayı sunucuya kaydet
                            using (var stream = new FileStream(kaydetmeYolu, FileMode.Create))
                            {
                                dosya.CopyTo(stream);
                            }

                            // 6. Yeni 'UrunResmi' nesnesi oluştur (Veritabanı için)
                            var urunResmi = new UrunResmi
                            {
                                ResimYolu = "/uploads/" + dosyaAdi,
                                UrunId = urun.Id // Var olan ürünün ID'sine bağlı
                            };

                            // 7. Resim yolunu YENİ 'UrunResimleri' tablosuna kaydet
                            _context.UrunResimleri.Add(urunResmi);
                        }

                        // 8. Tüm yeni resim yollarını veritabanına işle (SaveChanges)
                        _context.SaveChanges();
                    }

                    // İşlem başarılı, listeye dön
                    return RedirectToAction(nameof(Urunler));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Güncelleme hatası: " + ex.Message);
            }

            // Hata varsa Viewbag'i yeniden doldur
            ViewBag.KategoriListesi = KategoriListesiniDoldur();
            return View(urun);
        }
        // GET: UrunSil
        public IActionResult UrunSil(int id)
        {
            var urun = _urunRepository.GetById(id);
            if (urun == null) { return NotFound(); }
            return View(urun);
        }

        // POST: UrunSil
        [HttpPost, ActionName("UrunSil")]
        [ValidateAntiForgeryToken]
        public IActionResult UrunSilOnay(int id)
        {
            _urunRepository.Delete(id);
            return RedirectToAction(nameof(Urunler));
        }

        // --- YARDIMCI METOT (ViewBag için) ---
        private IEnumerable<SelectListItem> KategoriListesiniDoldur()
        {
            try
            {
                return _kategoriRepository.GetAll().Select(k => new SelectListItem
                {
                    Text = k.Ad,
                    Value = k.Id.ToString()
                }).ToList();
            }
            catch
            {
                return new List<SelectListItem>();
            }
        }
        // POST: /Admin/ResimSil/5 (AJAX için)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResimSilAJAX(int id)
        {
            try
            {
                // 1. Silinecek resim kaydını (UrunResmi) veritabanından bul
                var resimKaydi = _context.UrunResimleri.FirstOrDefault(r => r.Id == id);
                if (resimKaydi == null)
                {
                    return Json(new { success = false, message = "Resim kaydı zaten silinmiş." });
                }

                // 2. Fiziksel dosyanın sunucudaki yolunu bul (wwwroot/uploads)
                var dosyaYolu = Path.Combine(_env.WebRootPath, resimKaydi.ResimYolu.TrimStart('/'));

                // 3. Dosya sunucuda varsa SİL
                if (System.IO.File.Exists(dosyaYolu))
                {
                    System.IO.File.Delete(dosyaYolu);
                }

                // 4. Veritabanı kaydını sil
                _context.UrunResimleri.Remove(resimKaydi);
                _context.SaveChanges();

                return Json(new { success = true, message = "Resim başarıyla silindi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Silme sırasında hata oluştu: " + ex.Message });
            }
        }
    } 

} 