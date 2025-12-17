using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrunSatisPortali.Models;
using UrunSatisPortali.Repository;
using UrunSatisPortali.ViewModels;
using Microsoft.AspNetCore.SignalR;
using UrunSatisPortali.Hubs;

namespace UrunSatisPortali.Controllers
{
    [Authorize]
    public class CartController(IShoppingCartRepository shoppingCartRepo, IOrderHeaderRepository orderHeaderRepo, IOrderDetailRepository orderDetailRepo, IProductRepository productRepo, IHubContext<OrderHub> hubContext) : Controller
    {
        private readonly IShoppingCartRepository _shoppingCartRepo = shoppingCartRepo;
        private readonly IOrderHeaderRepository _orderHeaderRepo = orderHeaderRepo;
        private readonly IOrderDetailRepository _orderDetailRepo = orderDetailRepo;
        private readonly IProductRepository _productRepo = productRepo;
        private readonly IHubContext<OrderHub> _hubContext = hubContext;

        [BindProperty]
        public ShoppingCartViewModel ShoppingCartVM { get; set; }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _shoppingCartRepo.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader = new()
            };

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Product = _productRepo.Get(u => u.Id == cart.ProductId); // Ensure product is loaded if includeProperties fails or for extra safety
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Product.Price * cart.Count);
            }

            return View(ShoppingCartVM);
        }

        public IActionResult Plus(int cartId)
        {
            var cartFromDb = _shoppingCartRepo.Get(u => u.Id == cartId);
            cartFromDb.Count += 1;
            _shoppingCartRepo.Update(cartFromDb);
            _shoppingCartRepo.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var cartFromDb = _shoppingCartRepo.Get(u => u.Id == cartId);
            if (cartFromDb.Count <= 1)
            {
                _shoppingCartRepo.Remove(cartFromDb);
            }
            else
            {
                cartFromDb.Count -= 1;
                _shoppingCartRepo.Update(cartFromDb);
            }
            _shoppingCartRepo.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            var cartFromDb = _shoppingCartRepo.Get(u => u.Id == cartId);
            _shoppingCartRepo.Remove(cartFromDb);
            _shoppingCartRepo.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _shoppingCartRepo.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader = new()
            };

            // Populate user details if available (skipped for now as we don't have ApplicationUser access easily, user enters manually)
            // But we can persist data if user posted back... valid for now.

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Product = _productRepo.Get(u => u.Id == cart.ProductId);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Product.Price * cart.Count);
            }
            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM.ShoppingCartList = _shoppingCartRepo.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product");

            if (!ShoppingCartVM.ShoppingCartList.Any())
            {
                return RedirectToAction(nameof(Index));
            }

            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = userId;

            if (!ModelState.IsValid)
            {
                // Re-calculate totals for display if validation fails
                foreach (var cart in ShoppingCartVM.ShoppingCartList)
                {
                    cart.Product = _productRepo.Get(u => u.Id == cart.ProductId);
                    ShoppingCartVM.OrderHeader.OrderTotal += (cart.Product.Price * cart.Count);
                }
                return View(ShoppingCartVM);
            }

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Product.Price * cart.Count);
            }

            ShoppingCartVM.OrderHeader.OrderStatus = "Pending";
            ShoppingCartVM.OrderHeader.PaymentStatus = "Pending";

            _orderHeaderRepo.Add(ShoppingCartVM.OrderHeader);
            _orderHeaderRepo.Save();

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
                    Price = cart.Product.Price,
                    Count = cart.Count
                };
                _orderDetailRepo.Add(orderDetail);
                _orderDetailRepo.Save();

                // Decrement Stock
                cart.Product.Stock -= cart.Count;
                _productRepo.Update(cart.Product);
                _productRepo.Save();
            }

            // Clear Cart
            var customerCarts = _shoppingCartRepo.GetAll(u => u.ApplicationUserId == userId).ToList();
            _shoppingCartRepo.RemoveRange(customerCarts);
            _shoppingCartRepo.Save();

            await _hubContext.Clients.All.SendAsync("ReceiveOrderNotification");

            return RedirectToAction(nameof(OrderConfirmation), new { id = ShoppingCartVM.OrderHeader.Id });
        }

        public IActionResult OrderConfirmation(int id)
        {
            return View(id);
        }
    }
}
