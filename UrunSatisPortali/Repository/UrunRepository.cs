using UrunSatisPortali.Data;
using UrunSatisPortali.Models;

namespace UrunSatisPortali.Repository
{
    public class UrunRepository(ApplicationDbContext db) : Repository<Urun>(db), IUrunRepository
    {
        private readonly ApplicationDbContext _db = db;

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Urun obj)
        {
            var objFromDb = _db.Urunler.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Ad = obj.Ad;
                objFromDb.Aciklama = obj.Aciklama;
                objFromDb.Fiyat = obj.Fiyat;
                objFromDb.Stok = obj.Stok;
                objFromDb.KategoriId = obj.KategoriId;
                if (obj.ResimYolu != null)
                {
                    objFromDb.ResimYolu = obj.ResimYolu;
                }
            }
        }
    }
}
