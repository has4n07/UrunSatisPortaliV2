using UrunSatisPortali.Models;

namespace UrunSatisPortali.ViewModels
{
    public class ShoppingCartViewModel
    {
        public IEnumerable<ShoppingCart>? ShoppingCartList { get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}
