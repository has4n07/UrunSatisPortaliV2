using UrunSatisPortali.Data;
using UrunSatisPortali.Models;

namespace UrunSatisPortali.Repository
{
    public class CategoryRepository(ApplicationDbContext db) : Repository<Category>(db), ICategoryRepository
    {
        private readonly ApplicationDbContext _db = db;

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Category obj)
        {
            _db.Categories.Update(obj);
        }
    }
}
