using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using UrunSatisPortali.Repository;
using UrunSatisPortali.ViewModels;
using System.Globalization;

namespace UrunSatisPortali.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly IOrderHeaderRepository _orderHeaderRepo;

        public HomeController(IOrderHeaderRepository orderHeaderRepo)
        {
            _orderHeaderRepo = orderHeaderRepo;
        }

        public IActionResult Index()
        {
            var orders = _orderHeaderRepo.GetAll();

            var totalOrders = orders.Count();
            var totalRevenue = orders.Sum(u => u.OrderTotal);

            // Calculate Monthly Revenue for the current year
            var currentYear = DateTime.Now.Year;
            var monthlyData = orders
                .Where(u => u.OrderDate.Year == currentYear)
                .GroupBy(u => u.OrderDate.Month)
                .Select(g => new { Month = g.Key, Revenue = g.Sum(u => u.OrderTotal) })
                .OrderBy(x => x.Month)
                .ToList();

            string[] chartLabels = new string[12];
            decimal[] chartData = new decimal[12];

            for (int i = 1; i <= 12; i++)
            {
                chartLabels[i - 1] = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i);
                var monthRecord = monthlyData.FirstOrDefault(x => x.Month == i);
                chartData[i - 1] = monthRecord != null ? monthRecord.Revenue : 0;
            }

            DashboardViewModel dashboardVM = new()
            {
                TotalOrders = totalOrders,
                TotalRevenue = totalRevenue,
                ChartLabels = chartLabels,
                ChartData = chartData
            };

            return View(dashboardVM);
        }
        [HttpGet]
        public IActionResult GetDashboardStats()
        {
            var orders = _orderHeaderRepo.GetAll();
            var totalOrders = orders.Count();
            var totalRevenue = orders.Sum(u => u.OrderTotal);

            var currentYear = DateTime.Now.Year;
            var monthlyData = orders
                .Where(u => u.OrderDate.Year == currentYear)
                .GroupBy(u => u.OrderDate.Month)
                .Select(g => new { Month = g.Key, Revenue = g.Sum(u => u.OrderTotal) })
                .ToList();

            decimal[] chartData = new decimal[12];
            for (int i = 1; i <= 12; i++)
            {
                var monthRecord = monthlyData.FirstOrDefault(x => x.Month == i);
                chartData[i - 1] = monthRecord != null ? monthRecord.Revenue : 0;
            }

            return Json(new { totalOrders, totalRevenue, chartData });
        }
    }
}
