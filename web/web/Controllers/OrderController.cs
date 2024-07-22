using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Model;

namespace web.Controllers
{
    public class OrderController : Controller
    {
        private readonly StoreDbContext _context;

        public OrderController(StoreDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Checkout()
        {
            var userName = User.Identity.Name;
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserName == userName);

            if (cart == null || !cart.CartItems.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            var defaultAddress = await _context.Addresses.FirstOrDefaultAsync(a => a.UserName == userName && a.isDefault);

            ViewBag.DefaultAddress = defaultAddress;
            return View(cart);
        }
        [HttpPost]
        public async Task<IActionResult> CompletePurchase()
        {
            var userName = User.Identity.Name;
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserName == userName);

            if (cart != null && cart.CartItems.Any())
            {
                var defaultAddress = await _context.Addresses.FirstOrDefaultAsync(a => a.UserName == userName && a.isDefault);

                var orders = cart.CartItems.Select(ci => new Order
                {
                    UserName = userName,
                    OrderDate = DateTime.Now,
                    OrderStatus = "Pending", // Initial status
                    ProductID = ci.ProductID,
                    ProductName = ci.Product.Name,
                    Quantity = ci.Quantity,
                    Price = ci.Price,
                    // Copy default address details into the order
                    Address = defaultAddress?.Street,
                    City = defaultAddress?.City,
                    State = defaultAddress?.State,
                    PostCode = defaultAddress?.PostCode,
                    Recipient = defaultAddress?.Recipient
                }).ToList();

                // Add orders to the database
                _context.Orders.AddRange(orders);

                // Remove the cart
                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("OrderHistory", "Order");
        }

        public async Task<IActionResult> OrderHistory()
        {
            var userName = User.Identity.Name;
            var orders = await _context.Orders
                .Where(o => o.UserName == userName)
                .ToListAsync();

            return View(orders);
        }


    }
}
