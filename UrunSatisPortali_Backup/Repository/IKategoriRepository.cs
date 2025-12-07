using UrunSatisPortali.Models;
using System.Collections.Generic;

namespace UrunSatisPortali.Repository 
{
    public interface IKategoriRepository
    {
        IEnumerable<Kategori> GetAll();
        Kategori GetById(int id);
        void Add(Kategori kategori);
        void Update(Kategori kategori);
        void Delete(int id);
    }
}