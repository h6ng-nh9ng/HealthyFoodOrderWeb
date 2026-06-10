using HealthyFoodOrdering.Data;
using HealthyFoodOrdering.Models;
using HealthyFoodOrdering.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthyFoodOrdering.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            DashboardVM vm =
                new DashboardVM();

            vm.TotalProducts =
                _context.Products.Count();

            vm.TotalCategories =
                _context.Categories.Count();

            vm.TotalCombos =
                _context.NutritionCombos.Count();

            vm.TotalOrders =
                _context.Orders.Count();

            vm.Revenue = _context.Orders
                .Where(x => x.Status == OrderStatus.Completed)
                .Sum(x => (decimal?)x.TotalAmount) ?? 0;

            vm.TopProducts = _context.Products
                .OrderByDescending(x => x.SoldQuantity)
                .Take(5)
                .Select(x => new TopProductVM
                {
                    Id = x.Id,
                    Name = x.Name,
                    ImageUrl = x.ImageUrl,
                    SoldQuantity = x.SoldQuantity,
                    Revenue = (x.Price - (x.Price * x.DiscountPercent / 100)) * x.SoldQuantity
                })
                .ToList();

            vm.LatestOrders = _context.Orders
                .OrderByDescending(x => x.OrderDate)
                .Take(5)
                .Select(x => new LatestOrderVM
                {
                    Id = x.Id,
                    ReceiverName = x.ReceiverName,
                    OrderDate = x.OrderDate,
                    TotalAmount = x.TotalAmount,
                    Status = x.Status
                })
                .ToList();

            var today = DateTime.Today;
            var startDate = today.AddDays(-6);
            var endDate = today.AddDays(1);

            var completedOrders = _context.Orders
                .Where(x => x.Status == OrderStatus.Completed
                    && x.OrderDate >= startDate
                    && x.OrderDate < endDate)
                .Select(x => new
                {
                    x.OrderDate,
                    x.TotalAmount
                })
                .ToList();

            for (int i = 0; i < 7; i++)
            {
                var date = startDate.AddDays(i);

                vm.RevenueLabels.Add(date.ToString("dd/MM"));
                vm.RevenueValues.Add(
                    completedOrders
                        .Where(x => x.OrderDate.Date == date)
                        .Sum(x => x.TotalAmount)
                );
            }

            return View(vm);
        }
    }
}
