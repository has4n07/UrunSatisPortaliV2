using UrunSatisPortali.Models;

namespace UrunSatisPortali.Repository
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product obj);
        void Save();
    }
}
