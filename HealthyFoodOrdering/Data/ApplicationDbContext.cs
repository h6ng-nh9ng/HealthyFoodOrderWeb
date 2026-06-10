using HealthyFoodOrdering.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HealthyFoodOrdering.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<NutritionCombo> NutritionCombos { get; set; }
        public DbSet<ComboDetail> ComboDetails { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Cấu hình khóa chính kép cho ComboDetail
            builder.Entity<ComboDetail>()
                   .HasKey(x => new { x.ComboId, x.ProductId });

            builder.Entity<Product>()
                   .Property(x => x.Price)
                   .HasColumnType("decimal(18,2)");

            builder.Entity<Product>()
                   .HasIndex(x => x.Name);

            builder.Entity<Category>()
                   .HasIndex(x => x.Name);

            builder.Entity<NutritionCombo>()
                   .HasIndex(x => x.ComboName);

            builder.Entity<NutritionCombo>()
                   .Property(x => x.TotalPrice)
                   .HasColumnType("decimal(18,2)");

            builder.Entity<ComboDetail>()
                   .HasOne(x => x.NutritionCombo)
                   .WithMany(x => x.ComboDetails)
                   .HasForeignKey(x => x.ComboId);

            builder.Entity<ComboDetail>()
                   .HasOne(x => x.Product)
                   .WithMany()
                   .HasForeignKey(x => x.ProductId);
        }
    }
}