using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrunSatisPortali.Models;
using UrunSatisPortali.Repository;
using UrunSatisPortali.ViewModels;

namespace UrunSatisPortali.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController(IOrderHeaderRepository orderHeaderRepo, IOrderDetailRepository orderDetailRepo) : Controller
    {
        private readonly IOrderHeaderRepository _orderHeaderRepo = orderHeaderRepo;
        private readonly IOrderDetailRepository _orderDetailRepo = orderDetailRepo;

        public IActionResult Index()
        {
            var objOrderList = _orderHeaderRepo.GetAll(includeProperties: "ApplicationUser").ToList();
            return View(objOrderList);
        }

        public IActionResult Details(int orderId)
        {
            OrderViewModel orderVM = new()
            {
                OrderHeader = _orderHeaderRepo.Get(u => u.Id == orderId, includeProperties: "ApplicationUser"),
                OrderDetail = _orderDetailRepo.GetAll(u => u.OrderHeaderId == orderId, includeProperties: "Product")
            };
            return View(orderVM);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateOrderDetail(int orderId, string orderStatus)
        {
            var orderHeader = _orderHeaderRepo.Get(u => u.Id == orderId);
            if (orderHeader != null)
            {
                orderHeader.OrderStatus = orderStatus;
                if (orderStatus == "Shipped")
                {
                    orderHeader.ShippingDate = DateTime.Now;
                }
                _orderHeaderRepo.Update(orderHeader);
                _orderHeaderRepo.Save();
            }
            return RedirectToAction(nameof(Details), new { orderId = orderId });
        }
    }
}
