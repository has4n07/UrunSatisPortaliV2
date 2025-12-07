using UrunSatisPortali.Data;
using UrunSatisPortali.Models;
using System.Collections.Generic;
using System.Linq;

namespace UrunSatisPortali.Repository // Proje adına göre namespace'i düzelt
{
    public class KategoriRepository : IKategoriRepository
    {
        private readonly ApplicationDbContext _context;
        public KategoriRepository(ApplicationDbContext context) { _context = context; }
        public IEnumerable<Kategori> GetAll() { return _context.Kategoriler.ToList(); }
        public Kategori GetById(int id) { return _context.Kategoriler.Find(id); }
        public void Add(Kategori kategori) { _context.Kategoriler.Add(kategori); _context.SaveChanges(); }
        public void Update(Kategori kategori) { _context.Kategoriler.Update(kategori); _context.SaveChanges(); }
        public void Delete(int id) { var k = GetById(id); if (k != null) { _context.Kategoriler.Remove(k); _context.SaveChanges(); } }
    }
}