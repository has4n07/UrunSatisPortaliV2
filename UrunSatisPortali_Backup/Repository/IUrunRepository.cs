using UrunSatisPortali.Models;
using System.Collections.Generic;

namespace UrunSatisPortali.Repository 
{
    public interface IUrunRepository
    {
        IEnumerable<Urun> GetAll(); 
        IEnumerable<Urun> GetActive();
        bool KategoriyeAitUrunVarMi(int kategoriId);
        Urun GetById(int id);
        void Add(Urun urun);
        void Update(Urun urun);
        void Delete(int id);
    }
}