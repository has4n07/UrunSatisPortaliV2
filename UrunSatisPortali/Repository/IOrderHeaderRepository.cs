using UrunSatisPortali.Models;

namespace UrunSatisPortali.Repository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader obj);
        void Save();
    }
}
