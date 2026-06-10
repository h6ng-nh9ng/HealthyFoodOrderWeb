using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace HealthyFoodOrdering.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = "";

        public string Avatar { get; set; } = "";

        public DateTime CreatedDate { get; set; }
            = DateTime.Now;

        public ICollection<Address> Addresses { get; set; } 
            = new List<Address>();

        public DateTime? DateOfBirth { get; set; }

        public string Gender { get; set; } = "";

        public decimal Height { get; set; } = 0;

        public decimal Weight { get; set; } = 0;

        public string FitnessGoal { get; set; } = "";
    }
}