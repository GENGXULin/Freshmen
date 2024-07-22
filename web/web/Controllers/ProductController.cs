using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using web.Models;
using web.Entities;
using Microsoft.AspNetCore.Authorization;
using web.Model;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace web.Controllers
{
    public class ProductController : Controller
    {
        private readonly StoreDbContext _context;

        public ProductController(StoreDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Inventory(int? genreId, int? subGenreId)
        {
            var products = _context.Products
                .Include(p => p.GenreNavigation)
                .Include(p => p.subGenreNavigation)
                .AsQueryable();

            if (genreId.HasValue)
            {
                products = products.Where(p => p.Genre == genreId.Value);
            }

            if (subGenreId.HasValue)
            {
                products = products.Where(p => p.subGenre == subGenreId.Value);
            }

            var genreList = await _context.Genres.ToListAsync();
            var subGenreList = new List<subGenre>();
            if (genreId.HasValue)
            {
                subGenreList = await _context.subGenres.Where(sg => sg.genreID == genreId.Value).ToListAsync();
            }

            ViewBag.Genres = genreList.Select(g => new { Value = g.genreID, Text = g.Name }).ToList();
            ViewBag.SubGenres = subGenreList.Select(sg => new { Value = sg.subGenreID, Text = sg.Name }).ToList();
            ViewBag.SelectedGenreId = genreId;
            ViewBag.SelectedSubGenreId = subGenreId;

            return View(await products.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateEdit(int? id)
        {
            var genres = await _context.Genres.ToListAsync();
            var subGenres = await _context.subGenres.ToListAsync();

            ViewBag.Genres = genres.Select(g => new SelectListItem { Value = g.genreID.ToString(), Text = g.Name }).ToList();
            ViewBag.SubGenres = subGenres.Select(sg => new SelectListItem { Value = sg.subGenreID.ToString(), Text = sg.Name }).ToList();

            if (id == null)
            {
                return View(new Product());
            }

            var product = _context.Products.FirstOrDefault(p => p.ID == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateEdit(Product model)
        {
            if (ModelState.IsValid)
            {
                if (model.ID == 0)
                {
                    _context.Add(model);
                }
                else
                {
                    model.LastUpdated = DateTime.Now;
                    model.LastUpdatedBy = User.Identity.Name;
                    _context.Update(model);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Inventory");
            }

            var genres = await _context.Genres.ToListAsync();
            var subGenres = await _context.subGenres.ToListAsync();

            ViewBag.Genres = genres.Select(g => new SelectListItem { Value = g.genreID.ToString(), Text = g.Name }).ToList();
            ViewBag.SubGenres = subGenres.Select(sg => new SelectListItem { Value = sg.subGenreID.ToString(), Text = sg.Name }).ToList();
            return View(model);
        }

        [Authorize(Roles = "User")]
        public IActionResult Purchase(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ID == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var userEmail = User.Identity.Name;
            var cart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserName == userEmail);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserName = userEmail,
                    CreatedDate = DateTime.Now,
                    CartItems = new List<CartItem>()
                };
                _context.Carts.Add(cart);
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductID == productId);
            if (cartItem == null)
            {
                var product = await _context.Products.FindAsync(productId);
                cartItem = new CartItem
                {
                    CartID = cart.CartID,
                    ProductID = productId,
                    Quantity = quantity,
                    Price = product.Price ?? 0
                };
                cart.CartItems.Add(cartItem);
                _context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
                _context.CartItems.Update(cartItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> PurchaseConfirmed(int id, int quantity)
        {
            // Implement your purchase confirmation logic here
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Here you can implement the logic to create an order and save it in the database

            return RedirectToAction("Inventory");
        }
    }
}
