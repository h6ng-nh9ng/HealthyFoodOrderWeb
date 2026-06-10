namespace HealthyFoodOrdering.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string UserId { get; set; } = "";

        public ApplicationUser? User { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = OrderStatus.Pending;

        public string PaymentMethod { get; set; } = "";

        public string ReceiverName { get; set; } = "";

        public string Phone { get; set; } = "";

        public string Address { get; set; } = "";

        public ICollection<OrderDetail> OrderDetails { get; set; }
            = new List<OrderDetail>();
    }
}
