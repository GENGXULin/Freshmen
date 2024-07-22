using COMP3851B.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace COMP3851B.Controllers
{
    public class FreshmenController : Controller
    {

        private readonly ApplicationDbContext _context;

        public FreshmenController(ILogger<HomeController> logger, ApplicationDbContext context)
        {

            _context = context;
        }
        public IActionResult Index()
        {
            var userName = Request.Cookies["AuthCookie"];

            if (string.IsNullOrEmpty(userName))
            {
                // 如果没有登录，直接返回默认视图或空视图
                return View(); // 这里返回默认视图，你可以根据需要创建一个空视图
            }

            var user = _context.Users.SingleOrDefault(u => u.UserName == userName);

            if (user == null)
            {
                // 用户存在但找不到对应的记录，可以返回错误视图或默认视图
                return View(); // 这里返回默认视图，你可以根据需要创建一个错误视图
            }

            var model = new User
            {
                IsAdmin = user.IsAdmin,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                StudentNumber = user.StudentNumber
            };

            return View(model);
        }
    }
}
