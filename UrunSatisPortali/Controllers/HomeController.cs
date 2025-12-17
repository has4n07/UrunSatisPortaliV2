using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UrunSatisPortali.Models;
using UrunSatisPortali.Repository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using UrunSatisPortali.ViewModels;

namespace UrunSatisPortali.Controllers
{
    public class HomeController(ILogger<HomeController> logger, IProductRepository productRepo, IShoppingCartRepository shoppingCartRepo, IOrderDetailRepository orderDetailRepo) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly IProductRepository _productRepo = productRepo;
        private readonly IShoppingCartRepository _shoppingCartRepo = shoppingCartRepo;
        private readonly IOrderDetailRepository _orderDetailRepo = orderDetailRepo;

        public IActionResult Index()
        {
            var productList = _productRepo.GetAll(includeProperties: "Category");

            // Calculate Best Sellers
            // Group by ProductId, Sum Count, Order Descending
            var bestSellerIds = _orderDetailRepo.GetAll()
                .GroupBy(u => u.ProductId)
                .Select(g => new { ProductId = g.Key, TotalSold = g.Sum(x => x.Count) })
                .OrderByDescending(x => x.TotalSold)
                .Take(4)
                .Select(x => x.ProductId)
                .ToList();

            var bestSellers = _productRepo.GetAll(u => bestSellerIds.Contains(u.Id), includeProperties: "Category")
                                          .ToList()
                                          .OrderBy(u => bestSellerIds.IndexOf(u.Id)); // Preserve order

            HomeViewModel homeVM = new()
            {
                NewArrivals = productList,
                BestSellers = bestSellers
            };

            return View(homeVM);
        }

        public IActionResult Details(int id)
        {
            var product = _productRepo.Get(u => u.Id == id, includeProperties: "Category");
            if (product == null)
            {
                return NotFound();
            }
            ShoppingCart cart = new()
            {
                Product = product,
                ProductId = id,
                Count = 1
            };
            return View(cart);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;

            ShoppingCart cartFromDb = _shoppingCartRepo.Get(u => u.ApplicationUserId == userId && u.ProductId == shoppingCart.ProductId);

            if (cartFromDb != null)
            {
                // Update count
                cartFromDb.Count += shoppingCart.Count;
                _shoppingCartRepo.Update(cartFromDb);
            }
            else
            {
                // Add new record
                shoppingCart.Id = 0;
                _shoppingCartRepo.Add(shoppingCart);
            }
            _shoppingCartRepo.Save();

            return RedirectToAction(nameof(Index));
        }



        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
