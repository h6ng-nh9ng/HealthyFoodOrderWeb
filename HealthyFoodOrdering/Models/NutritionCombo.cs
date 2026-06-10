namespace HealthyFoodOrdering.Models
{
    public class NutritionCombo
    {
        public int Id { get; set; }

        public string ComboName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal TotalPrice { get; set; }

        public int TotalCalories { get; set; }

        public double TotalProtein { get; set; }

        public double TotalCarbs { get; set; }

        public double TotalFat { get; set; }

        public string Thumbnail { get; set; }

        public bool Featured { get; set; }

        public string GoalType { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public ICollection<ComboDetail> ComboDetails { get; set; }
            = new List<ComboDetail>();

        public decimal DiscountPercent { get; set; }

        public int SoldQuantity { get; set; }

        public DateTime CreatedDate { get; set; }
            = DateTime.Now;
    }
}