using UrunSatisPortali.Data;
using UrunSatisPortali.Models;

namespace UrunSatisPortali.Repository
{
    public class OrderDetailRepository(ApplicationDbContext db) : Repository<OrderDetail>(db), IOrderDetailRepository
    {
        private readonly ApplicationDbContext _db = db;

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(OrderDetail obj)
        {
            _db.OrderDetails.Update(obj);
        }
    }
}
