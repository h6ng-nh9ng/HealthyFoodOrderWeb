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

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Admin
        public IActionResult Index()
        {
            var vm = new DashboardVM();
            var today = DateTime.Today;

            // Tổng số liệu
            vm.TotalProducts = _context.Products.Count();
            vm.TotalCategories = _context.Categories.Count();
            vm.TotalCombos = _context.NutritionCombos.Count();
            vm.TotalOrders = _context.Orders.Count();

            vm.Revenue = _context.Orders
                .Where(x => x.Status == OrderStatus.Completed)
                .Sum(x => (decimal?)x.TotalAmount) ?? 0;

            // Top 5 sản phẩm bán chạy nhất
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

            // 5 đơn hàng mới nhất
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

            // Doanh thu trong 7 ngày gần nhất
            var dailyStart = today.AddDays(-6);
            var dailyEnd = today.AddDays(1);

            var completedOrdersDaily = _context.Orders
                .Where(x => x.Status == OrderStatus.Completed
                         && x.OrderDate >= dailyStart
                         && x.OrderDate < dailyEnd)
                .Select(x => new { x.OrderDate, x.TotalAmount })
                .ToList();

            for (int i = 0; i < 7; i++)
            {
                var date = dailyStart.AddDays(i);
                vm.RevenueLabels.Add(date.ToString("dd/MM"));
                vm.RevenueValues.Add(
                    completedOrdersDaily
                        .Where(x => x.OrderDate.Date == date)
                        .Sum(x => x.TotalAmount)
                );
            }

            // Doanh thu trong 6 tháng gần nhất
            var monthlyStart = new DateTime(today.Year, today.Month, 1).AddMonths(-5);

            var completedOrdersMonthly = _context.Orders
                .Where(x => x.Status == OrderStatus.Completed
                         && x.OrderDate >= monthlyStart)
                .Select(x => new { x.OrderDate, x.TotalAmount })
                .ToList();

            for (int i = 0; i < 6; i++)
            {
                var month = monthlyStart.AddMonths(i);
                vm.MonthlyRevenueLabels.Add($"T{month.Month}/{month.Year}");
                vm.MonthlyRevenueValues.Add(
                    completedOrdersMonthly
                        .Where(x => x.OrderDate.Year == month.Year
                                 && x.OrderDate.Month == month.Month)
                        .Sum(x => x.TotalAmount)
                );
            }

            // Số lượng đơn hàng theo trạng thái
            var statusGroups = _context.Orders
                .GroupBy(x => x.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToList();

            foreach (var group in statusGroups)
            {
                vm.OrderStatusLabels.Add(OrderStatus.GetDisplayName(group.Status));
                vm.OrderStatusCounts.Add(group.Count);
            }

            return View(vm);
        }
    }
}
