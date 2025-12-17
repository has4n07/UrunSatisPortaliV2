using UrunSatisPortali.Models;

namespace UrunSatisPortali.Repository
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        void Update(OrderDetail obj);
        void Save();
    }
}
