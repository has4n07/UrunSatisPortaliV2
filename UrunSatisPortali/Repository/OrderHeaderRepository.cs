using UrunSatisPortali.Data;
using UrunSatisPortali.Models;

namespace UrunSatisPortali.Repository
{
    public class OrderHeaderRepository(ApplicationDbContext db) : Repository<OrderHeader>(db), IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _db = db;

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(OrderHeader obj)
        {
            _db.OrderHeaders.Update(obj);
        }
    }
}
