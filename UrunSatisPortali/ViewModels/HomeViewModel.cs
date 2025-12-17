using UrunSatisPortali.Models;

namespace UrunSatisPortali.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Product> NewArrivals { get; set; } = new List<Product>();
        public IEnumerable<Product> BestSellers { get; set; } = new List<Product>();
    }
}
