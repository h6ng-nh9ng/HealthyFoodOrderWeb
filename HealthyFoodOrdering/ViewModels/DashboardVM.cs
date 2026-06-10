namespace HealthyFoodOrdering.ViewModels
{
    public class DashboardVM
    {
        public int TotalProducts { get; set; }

        public int TotalCategories { get; set; }

        public int TotalCombos { get; set; }

        public int TotalOrders { get; set; }

        public decimal Revenue { get; set; }

        public List<TopProductVM> TopProducts { get; set; }
            = new List<TopProductVM>();

        public List<LatestOrderVM> LatestOrders { get; set; }
            = new List<LatestOrderVM>();

        public List<string> RevenueLabels { get; set; }
            = new List<string>();

        public List<decimal> RevenueValues { get; set; }
            = new List<decimal>();
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
