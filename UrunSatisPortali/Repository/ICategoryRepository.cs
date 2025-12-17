using UrunSatisPortali.Models;

namespace UrunSatisPortali.Repository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category obj);
        void Save();
    }
}
