using HealthyFoodOrdering.Data;
using HealthyFoodOrdering.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

public class CartController : Controller
{
    private readonly ApplicationDbContext _context;
    public CartController(ApplicationDbContext context) => _context = context;

    public IActionResult Index() => View(GetCart());

    public IActionResult Add(int id)
    {
        return AddProduct(id);
    }

    // Thêm Sản phẩm
    public IActionResult AddProduct(int id)
    {
        var product = _context.Products.Find(id);
        if (product == null) return NotFound();

        var cart = GetCart();
        var item = cart.FirstOrDefault(x => x.ProductId == id && x.Type == "Product");

        if (item == null)
        {
            cart.Add(new CartItem
            {
                ProductId = id,
                ProductName = product.Name,
                Price = product.Price,
                Quantity = 1,
                ImageUrl = product.ImageUrl,
                Type = "Product"
            });
        }
        else { item.Quantity++; }

        SaveCart(cart);

        var referer = Request.Headers["Referer"].ToString();

        if (!string.IsNullOrEmpty(referer))
            return Redirect(referer);

        return RedirectToAction("Index", "Product");
    }

    // Thêm Combo
    public IActionResult AddCombo(int id)
    {
        var combo = _context.NutritionCombos.Find(id);
        if (combo == null) return NotFound();

        var cart = GetCart();
        var item = cart.FirstOrDefault(x => x.ComboId == id && x.Type == "Combo");

        if (item == null)
        {
            cart.Add(new CartItem
            {
                ComboId = id,
                ComboName = combo.ComboName,
                Price = combo.TotalPrice,
                Quantity = 1,
                ImageUrl = combo.ImageUrl,
                Type = "Combo"
            });
        }
        else { item.Quantity++; }

        SaveCart(cart);

        var referer = Request.Headers["Referer"].ToString();

        if (!string.IsNullOrEmpty(referer))
            return Redirect(referer);

        return RedirectToAction("Index", "Product");
    }

    public IActionResult Remove(int id, string type)
    {
        var cart = GetCart();
        // Xóa dựa trên cả ID và Type để phân biệt Combo/Product
        cart.RemoveAll(x => (type == "Combo" ? x.ComboId == id : x.ProductId == id) && x.Type == type);
        SaveCart(cart);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult UpdateQuantity(int id, int quantity, string type)
    {
        var cart = GetCart();
        var item = cart.FirstOrDefault(x => (type == "Combo" ? x.ComboId == id : x.ProductId == id) && x.Type == type);
        if (item != null)
        {
            if (quantity <= 0)
            {
                cart.Remove(item);
            }
            else
            {
                item.Quantity = quantity;
            }
        }
        SaveCart(cart);
        return RedirectToAction("Index");
    }

    private List<CartItem> GetCart()
    {
        var session = HttpContext.Session.GetString("Cart");
        return string.IsNullOrEmpty(session)
                     ? new List<CartItem>()
                     : JsonSerializer.Deserialize<List<CartItem>>(session)
                     ?? new List<CartItem>();
    }

    private void SaveCart(List<CartItem> cart) => HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
}