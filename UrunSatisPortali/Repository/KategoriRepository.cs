using UrunSatisPortali.Data;
using UrunSatisPortali.Models;

namespace UrunSatisPortali.Repository
{
    public class KategoriRepository(ApplicationDbContext db) : Repository<Kategori>(db), IKategoriRepository
    {
        private readonly ApplicationDbContext _db = db;

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Kategori obj)
        {
            _db.Kategoriler.Update(obj);
        }
    }
}
