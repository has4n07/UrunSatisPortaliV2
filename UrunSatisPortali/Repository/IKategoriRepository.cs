using UrunSatisPortali.Models;

namespace UrunSatisPortali.Repository
{
    public interface IKategoriRepository : IRepository<Kategori>
    {
        void Update(Kategori obj);
        void Save();
    }
}
