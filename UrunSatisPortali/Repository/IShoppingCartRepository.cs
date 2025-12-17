using UrunSatisPortali.Models;

namespace UrunSatisPortali.Repository
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        void Update(ShoppingCart obj);
        void Save();
    }
}
