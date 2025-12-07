using UrunSatisPortali.Data;
using UrunSatisPortali.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore; // .Include() için bu gerekli

namespace UrunSatisPortali.Repository 
{
    public class UrunRepository : IUrunRepository
    {
        private readonly ApplicationDbContext _context;
        public UrunRepository(ApplicationDbContext context) { _context = context; }

        public IEnumerable<Urun> GetAll()
        {
            // .Include("Kategori") -> Ürünle birlikte Kategori'yi de getir
            return _context.Urunler
                            .Include(u => u.Kategori)
                            .Include(u => u.UrunResimleri)
                            .ToList();
        }
        public Urun GetById(int id)
        {
            return _context.Urunler
                   .Include(u => u.Kategori)
                   .Include(u => u.UrunResimleri) 
                   .FirstOrDefault(u => u.Id == id);
        }
        public IEnumerable<Urun> GetActive()
        {
            return _context.Urunler
                           .Where(u => u.Aktif == true)
                           .Include(u => u.Kategori)
                           .Include(u => u.UrunResimleri)
                           .ToList();
        }
        public void Add(Urun urun)
        {
            // EF Core'a bu nesneyi eklemesi gerektiğini açıkça söylüyoruz.
            // Bu, kısıtlamaları ve ilişkileri daha iyi çözmesine yardımcı olur.
            _context.Entry(urun).State = EntityState.Added;
            _context.SaveChanges();
        }        // UrunRepository.cs dosyasının içindeki Update metodu

        public void Update(Urun urun)
        {
            // 1. Veritabanındaki orijinal kaydı (eski ürün) bul
            // Burası, ürünün eski ResimYolu listesini ve diğer tüm verilerini getirir.
            var existingUrun = _context.Urunler.Find(urun.Id);

            if (existingUrun != null)
            {
                // 2. Orijinal kaydın üzerine YALNIZCA metin/sayı değerlerini yazar.
                // Bu, EF Core'a boş gelen navigation property'leri (Kategori nesnesi, UrunResimleri listesi) görmezden gelmesini söyler.
                existingUrun.Ad = urun.Ad;
                existingUrun.Fiyat = urun.Fiyat;
                existingUrun.Stok = urun.Stok;
                existingUrun.Aciklama = urun.Aciklama;
                existingUrun.Aktif = urun.Aktif;
                existingUrun.KategoriId = urun.KategoriId;

                // 3. Değişiklikleri kaydet (Bu, yeni UrunResmi ekleme kodumuzla çakışmaz)
                _context.SaveChanges();
            }
        }
            public void Delete(int id) { var u = GetById(id); if (u != null) { _context.Urunler.Remove(u); _context.SaveChanges(); } }
        public bool KategoriyeAitUrunVarMi(int kategoriId)
        {
           
            return _context.Urunler.Any(u => u.KategoriId == kategoriId);
        }
    }
}