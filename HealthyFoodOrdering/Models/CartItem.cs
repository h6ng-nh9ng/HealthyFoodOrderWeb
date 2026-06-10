namespace HealthyFoodOrdering.Models
{
    public class CartItem
    {
        // Định danh
        public int ProductId { get; set; }
        public string ProductName { get; set; } = "";
        public int? ComboId { get; set; }
        public string ComboName { get; set; } = "";

        // Thông tin hiển thị
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; } = "";

        // Loại mục (phân biệt Product / Combo)
        public string Type { get; set; } = "";

        // Computed
        public decimal Total => Price * Quantity;

        // Helper: tên hiển thị chung (dùng được ở View)
        public string DisplayName => Type == "Combo" ? ComboName : ProductName;
    }
}