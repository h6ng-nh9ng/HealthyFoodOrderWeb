namespace HealthyFoodOrdering.ViewModels
{
    public class DashboardVM
    {
        // Tổng số liệu
        public int TotalProducts { get; set; }

        public int TotalCategories { get; set; }

        public int TotalCombos { get; set; }

        public int TotalOrders { get; set; }

        public decimal Revenue { get; set; }

        // Top 5 sản phẩm bán chạy nhất
        public List<TopProductVM> TopProducts { get; set; }
            = new List<TopProductVM>();

        public List<LatestOrderVM> LatestOrders { get; set; }
            = new List<LatestOrderVM>();

        // ── Biểu đồ doanh thu theo ngày (7 ngày gần nhất) ──
        //Nhãn trục X – định dạng dd/MM.
        public List<string> RevenueLabels { get; set; }
            = new List<string>();

        //Doanh thu tương ứng với từng ngày trong RevenueLabels.
        public List<decimal> RevenueValues { get; set; }
            = new List<decimal>();

        // ── Biểu đồ doanh thu theo tháng (6 tháng gần nhất) ───────────────
        //Nhãn trục X – định dạng T{M}/yyyy.
        public List<string> MonthlyRevenueLabels { get; set; }
            = new List<string>();

        //Doanh thu tương ứng với từng tháng trong MonthlyRevenueLabels.
        public List<decimal> MonthlyRevenueValues { get; set; }
            = new List<decimal>();

        // ── Biểu đồ phân bổ trạng thái đơn hàng (doughnut) ────────────────
        //Tên hiển thị của từng trạng thái đơn hàng.
        public List<string> OrderStatusLabels { get; set; }
            = new List<string>();

        //Số lượng đơn theo từng trạng thái tương ứng.
        public List<int> OrderStatusCounts { get; set; }
            = new List<int>();
    }

    public class TopProductVM
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string ImageUrl { get; set; } = "";

        public int SoldQuantity { get; set; }

        public decimal Revenue { get; set; }
    }

    public class LatestOrderVM
    {
        public int Id { get; set; }

        public string ReceiverName { get; set; } = "";

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "";
    }
}
