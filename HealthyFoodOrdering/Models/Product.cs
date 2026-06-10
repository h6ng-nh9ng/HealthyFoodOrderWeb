using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthyFoodOrdering.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        [Range(0, 10000)]
        public int Calories { get; set; }

        [Range(0, 1000)]
        public double Protein { get; set; }

        [Range(0, 1000)]
        public double Carbs { get; set; }

        [Range(0, 1000)]
        public double Fat { get; set; }

        public bool Featured { get; set; }

        public int StockQuantity { get; set; }

        public string GoalType { get; set; } = "Healthy";

        public string PreparationTime { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public int CategoryId { get; set; }

        public Category Category { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string SKU { get; set; }

        public int SoldQuantity { get; set; }

        public decimal DiscountPercent { get; set; }

        [NotMapped]
        public decimal FinalPrice
        {
            get
            {
                return Price - (Price * DiscountPercent / 100);
            }
        }
    }
}