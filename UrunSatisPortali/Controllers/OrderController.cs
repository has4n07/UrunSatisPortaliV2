using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrunSatisPortali.Repository;
using UrunSatisPortali.ViewModels;

namespace UrunSatisPortali.Controllers
{
    [Authorize]
    public class OrderController(IOrderHeaderRepository orderHeaderRepo, IOrderDetailRepository orderDetailRepo) : Controller
    {
        private readonly IOrderHeaderRepository _orderHeaderRepo = orderHeaderRepo;
        private readonly IOrderDetailRepository _orderDetailRepo = orderDetailRepo;

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity!;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) return RedirectToAction("Login", "Access");

            var objOrderList = _orderHeaderRepo.GetAll(u => u.ApplicationUserId == userId, includeProperties: "ApplicationUser").OrderByDescending(u => u.Id).ToList();
            return View(objOrderList);
        }

        public IActionResult Details(int orderId)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity!;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var orderHeader = _orderHeaderRepo.Get(u => u.Id == orderId, includeProperties: "ApplicationUser");

            // Security check: Ensure order belongs to user
            if (orderHeader == null || orderHeader.ApplicationUserId != userId)
            {
                return NotFound();
            }

            OrderViewModel orderVM = new()
            {
                OrderHeader = orderHeader,
                OrderDetail = _orderDetailRepo.GetAll(u => u.OrderHeaderId == orderId, includeProperties: "Product")
            };
            return View(orderVM);
        }
    }
}
