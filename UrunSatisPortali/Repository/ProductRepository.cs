using UrunSatisPortali.Data;
using UrunSatisPortali.Models;

namespace UrunSatisPortali.Repository
{
    public class ProductRepository(ApplicationDbContext db) : Repository<Product>(db), IProductRepository
    {
        private readonly ApplicationDbContext _db = db;

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Product obj)
        {
            var objFromDb = _db.Products.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = obj.Name;
                objFromDb.Description = obj.Description;
                objFromDb.Price = obj.Price;
                objFromDb.Stock = obj.Stock;
                objFromDb.CategoryId = obj.CategoryId;
                if (obj.ImagePath != null)
                {
                    objFromDb.ImagePath = obj.ImagePath;
                }
            }
        }
    }
}
