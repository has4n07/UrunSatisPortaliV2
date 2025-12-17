namespace UrunSatisPortali.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalProducts { get; set; } // Optional nice-to-have
        public string[] ChartLabels { get; set; }
        public decimal[] ChartData { get; set; }
    }
}
