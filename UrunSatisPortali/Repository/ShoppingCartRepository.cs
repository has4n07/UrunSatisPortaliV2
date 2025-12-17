using UrunSatisPortali.Data;
using UrunSatisPortali.Models;

namespace UrunSatisPortali.Repository
{
    public class ShoppingCartRepository(ApplicationDbContext db) : Repository<ShoppingCart>(db), IShoppingCartRepository
    {
        private readonly ApplicationDbContext _db = db;

        public void Save()
        {
                _db.SaveChanges();
        }

        public void Update(ShoppingCart obj)
        {
            _db.ShoppingCarts.Update(obj);
        }
    }
}
