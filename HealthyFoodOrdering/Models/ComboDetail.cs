namespace HealthyFoodOrdering.Models
{
    public class ComboDetail
    {
        public int ComboId { get; set; }
        public NutritionCombo NutritionCombo { get; set; } = null!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
