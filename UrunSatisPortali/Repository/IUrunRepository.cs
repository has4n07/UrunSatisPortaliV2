using UrunSatisPortali.Models;

namespace UrunSatisPortali.Repository
{
    public interface IUrunRepository : IRepository<Urun>
    {
        void Update(Urun obj);
        void Save();
    }
}
