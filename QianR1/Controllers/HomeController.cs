using COMP3851B.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace COMP3851B.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult location()
        {
            {
                // 你可以在这里处理逻辑，并传递数据到视图中
                ViewData["Latitude"] = -34.397;
                ViewData["Longitude"] = 150.644;
                return View();
            }
        }

        public IActionResult Announcement()
        {
            var announcements = _context.Announcements.OrderByDescending(a => a.LastUpdateDate).ToList();
            return View(announcements);
        }

        public async Task<IActionResult> FAQs()
        {
            var faqs = await _context.FAQs.ToListAsync();
            return View(faqs);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult DocumentsNeeded()
        {
            return View();
        }

        public IActionResult Guide()
        {

            var userName = Request.Cookies["AuthCookie"];
            if (userName == null)
            {
                return RedirectToAction("LogIn", "Account");
            }

            var user = _context.Users.SingleOrDefault(u => u.UserName == userName);
            if (user == null)
            {
                return RedirectToAction("LogIn", "Account");
            }


              return View(user);


        }
    }
}
