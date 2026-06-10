using HealthyFoodOrdering.Models;

namespace HealthyFoodOrdering.Data
{
    public static class DbSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (context.Categories.Any())
                return;

        var categories = new List<Category>
        {
            new()
            {
                Name = "Eat Clean",
                Description = "Thực đơn ăn sạch",
                ImageUrl = "/images/categories/eatclean.jpg"
            },

            new()
            {
                Name = "Tăng Cân",
                Description = "Thực đơn tăng cân",
                ImageUrl = "/images/categories/gain.jpg"
            },

            new()
            {
                Name = "Giảm Cân",
                Description = "Thực đơn giảm cân",
                ImageUrl = "/images/categories/loss.jpg"
            },

            new()
            {
                Name = "Gym Fitness",
                Description = "Thực đơn cho gymer",
                ImageUrl = "/images/categories/gym.jpg"
            },

            new()
            {
                Name = "Tiểu Đường",
                Description = "Thực đơn cho người tiểu đường",
                ImageUrl = "/images/categories/diabetes.jpg"
            }
        };

            context.Categories.AddRange(categories);
            context.SaveChanges();

            var eatClean = categories[0].Id;
            var gain = categories[1].Id;
            var loss = categories[2].Id;
            var gym = categories[3].Id;
            var diabetes = categories[4].Id;

            var products = new List<Product>
        {
            new() { Name="Salad Ức Gà", SKU="SP001", Price=85000, Calories=320, Protein=35, Carbs=15, Fat=8, GoalType="Weight Loss", PreparationTime="10 phút", CategoryId=loss, StockQuantity=100, Featured=true, SoldQuantity=40, ImageUrl="/images/products/p1.jpg" },

            new() { Name="Salad Cá Ngừ", SKU="SP002", Price=89000, Calories=340, Protein=38, Carbs=12, Fat=10, GoalType="Eat Clean", PreparationTime="10 phút", CategoryId=eatClean, StockQuantity=100, SoldQuantity=22, ImageUrl="/images/products/p2.jpg" },

            new() { Name="Yogurt Hy Lạp", SKU="SP003", Price=39000, Calories=150, Protein=12, Carbs=8, Fat=4, GoalType="Healthy", PreparationTime="3 phút", CategoryId=eatClean, StockQuantity=100, SoldQuantity=18, ImageUrl="/images/products/p3.jpg" },

            new() { Name="Nước Ép Detox", SKU="SP004", Price=45000, Calories=120, Protein=2, Carbs=18, Fat=1, GoalType="Weight Loss", PreparationTime="5 phút", CategoryId=loss, StockQuantity=100, SoldQuantity=33, ImageUrl="/images/products/p4.jpg" },

            new() { Name="Smoothie Xanh", SKU="SP005", Price=50000, Calories=180, Protein=4, Carbs=25, Fat=3, GoalType="Healthy", PreparationTime="5 phút", CategoryId=eatClean, StockQuantity=100, SoldQuantity=21, ImageUrl="/images/products/p5.jpg" },

            new() { Name="Protein Bowl", SKU="SP006", Price=105000, Calories=520, Protein=45, Carbs=42, Fat=15, GoalType="Gym", PreparationTime="12 phút", CategoryId=gym, StockQuantity=100, Featured=true, SoldQuantity=60, ImageUrl="/images/products/p6.jpg" },

            new() { Name="Ức Gà Áp Chảo", SKU="SP007", Price=95000, Calories=410, Protein=48, Carbs=10, Fat=12, GoalType="Gym", PreparationTime="12 phút", CategoryId=gym, StockQuantity=100, SoldQuantity=50, ImageUrl="/images/products/p7.jpg" },

            new() { Name="Khoai Lang Nướng", SKU="SP008", Price=35000, Calories=200, Protein=3, Carbs=42, Fat=0.5, GoalType="Gym", PreparationTime="8 phút", CategoryId=gym, StockQuantity=100, SoldQuantity=45, ImageUrl="/images/products/p8.jpg" },

            new() { Name="Cơm Gạo Lứt Cá Hồi", SKU="SP009", Price=120000, Calories=620, Protein=42, Carbs=55, Fat=20, GoalType="Eat Clean", PreparationTime="15 phút", CategoryId=eatClean, StockQuantity=100, SoldQuantity=29, ImageUrl="/images/products/p9.jpg" },

            new() { Name="Cơm Bò Eat Clean", SKU="SP010", Price=110000, Calories=680, Protein=48, Carbs=60, Fat=18, GoalType="Weight Gain", PreparationTime="15 phút", CategoryId=gain, StockQuantity=100, SoldQuantity=35, ImageUrl="/images/products/p10.jpg" },

            new() { Name="Yến Mạch Trái Cây", SKU="SP011", Price=55000, Calories=260, Protein=10, Carbs=35, Fat=5, GoalType="Healthy", PreparationTime="5 phút", CategoryId=eatClean, StockQuantity=100, SoldQuantity=12, ImageUrl="/images/products/p11.jpg" },

            new() { Name="Sandwich Bơ", SKU="SP012", Price=65000, Calories=310, Protein=11, Carbs=32, Fat=12, GoalType="Healthy", PreparationTime="6 phút", CategoryId=eatClean, StockQuantity=100, SoldQuantity=17, ImageUrl="/images/products/p12.jpg" },

            new() { Name="Trái Cây Tổng Hợp", SKU="SP013", Price=45000, Calories=180, Protein=3, Carbs=28, Fat=1, GoalType="Healthy", PreparationTime="5 phút", CategoryId=eatClean, StockQuantity=100, SoldQuantity=11, ImageUrl="/images/products/p13.jpg" },

            new() { Name="Sữa Hạt Óc Chó", SKU="SP014", Price=40000, Calories=170, Protein=6, Carbs=10, Fat=8, GoalType="Healthy", PreparationTime="2 phút", CategoryId=eatClean, StockQuantity=100, SoldQuantity=13, ImageUrl="/images/products/p14.jpg" },

            new() { Name="Bánh Pancake Healthy", SKU="SP015", Price=60000, Calories=280, Protein=14, Carbs=35, Fat=7, GoalType="Healthy", CategoryId=eatClean, StockQuantity=100, SoldQuantity=10, ImageUrl="/images/products/p15.jpg" },

            new() { Name="Cơm Gà Tăng Cân", SKU="SP016", Price=115000, Calories=780, Protein=52, Carbs=78, Fat=22, GoalType="Weight Gain", CategoryId=gain, StockQuantity=100, SoldQuantity=19, ImageUrl="/images/products/p16.jpg" },

            new() { Name="Mì Ý Bò Healthy", SKU="SP017", Price=125000, Calories=720, Protein=50, Carbs=75, Fat=20, GoalType="Weight Gain", CategoryId=gain, StockQuantity=100, SoldQuantity=16, ImageUrl="/images/products/p17.jpg" },

            new() { Name="Salad Rau Củ", SKU="SP018", Price=65000, Calories=220, Protein=5, Carbs=18, Fat=6, GoalType="Diabetes", CategoryId=diabetes, StockQuantity=100, SoldQuantity=8, ImageUrl="/images/products/p18.jpg" },

            new() { Name="Sữa Hạt Không Đường", SKU="SP019", Price=45000, Calories=140, Protein=7, Carbs=8, Fat=5, GoalType="Diabetes", CategoryId=diabetes, StockQuantity=100, SoldQuantity=7, ImageUrl="/images/products/p19.jpg" },

            new() { Name="Ức Gà Luộc", SKU="SP020", Price=90000, Calories=330, Protein=42, Carbs=0, Fat=7, GoalType="Diabetes", CategoryId=diabetes, StockQuantity=100, SoldQuantity=15, ImageUrl="/images/products/p20.jpg" }
        };

            context.Products.AddRange(products);
            context.SaveChanges();

            var combos = new List<NutritionCombo>
        {
            new()
            {
                ComboName="Combo Giảm Cân",
                Description="Salad + Detox + Yogurt",
                TotalPrice=159000,
                TotalCalories=450,
                TotalProtein=35,
                TotalCarbs=25,
                TotalFat=12,
                GoalType="Weight Loss",
                Featured=true,
                SoldQuantity=30,
                Thumbnail="/images/combos/combo1_thumb.jpg",
                ImageUrl="/images/combos/combo1.jpg"
            },

            new()
            {
                ComboName="Combo Tăng Cân",
                Description="Cơm bò + Protein Bowl + Sữa hạt",
                TotalPrice=255000,
                TotalCalories=1200,
                TotalProtein=75,
                TotalCarbs=110,
                TotalFat=30,
                GoalType="Weight Gain",
                Featured=true,
                SoldQuantity=25,
                Thumbnail="/images/combos/combo2_thumb.jpg",
                ImageUrl="/images/combos/combo2.jpg"
            },

            new()
            {
                ComboName="Combo Gym Fitness",
                Description="Ức gà + Khoai lang + Protein Bowl",
                TotalPrice=235000,
                TotalCalories=950,
                TotalProtein=80,
                TotalCarbs=70,
                TotalFat=20,
                GoalType="Gym",
                Featured=true,
                SoldQuantity=42,
                Thumbnail="/images/combos/combo3_thumb.jpg",
                ImageUrl="/images/combos/combo3.jpg"
            },

            new()
            {
                ComboName="Combo Eat Clean",
                Description="Salad cá ngừ + Smoothie + Trái cây",
                TotalPrice=175000,
                TotalCalories=550,
                TotalProtein=40,
                TotalCarbs=40,
                TotalFat=15,
                GoalType="Eat Clean",
                SoldQuantity=18,
                Thumbnail="/images/combos/combo4_thumb.jpg",
                ImageUrl="/images/combos/combo4.jpg"
            },

            new()
            {
                ComboName="Combo Tiểu Đường",
                Description="Salad + Ức gà + Sữa hạt",
                TotalPrice=180000,
                TotalCalories=480,
                TotalProtein=42,
                TotalCarbs=20,
                TotalFat=14,
                GoalType="Diabetes",
                SoldQuantity=12,
                Thumbnail="/images/combos/combo5_thumb.jpg",
                ImageUrl="/images/combos/combo5.jpg"
            }
        };

            context.NutritionCombos.AddRange(combos);
            context.SaveChanges();
        }
    }

}

