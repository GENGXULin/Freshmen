using Microsoft.AspNetCore.Mvc;
using COMP3851B.Models;
using System.Security.Cryptography;
using System.Text;

namespace COMP3851B.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 显示注册页面
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // 处理表单提交
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // 创建盐和哈希密码
                    var salt = GenerateSalt();
                    var hashedPassword = HashPassword(model.HashedPw, salt);

                    var user = new User
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        StudentNumber = model.StudentNumber,
                        IsAdmin = model.IsAdmin,
                        Salt = salt,
                        HashedPw = hashedPassword
                    };

                    _context.Users.Add(user);
                    _context.SaveChanges();

                    TempData["Success"] = true;
                    return RedirectToAction("Register");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                }
            }
            else
            {
                TempData["Error"] = string.Join("; ", ModelState.Values
                                                    .SelectMany(v => v.Errors)
                                                    .Select(e => e.ErrorMessage));
            }

            return RedirectToAction("Register");
        }

        private string GenerateSalt()
        {
            var rng = new RNGCryptoServiceProvider();
            var saltBytes = new byte[32];
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        private string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = password + salt;
                var saltedPasswordBytes = Encoding.UTF8.GetBytes(saltedPassword);
                var hashedPasswordBytes = sha256.ComputeHash(saltedPasswordBytes);
                return Convert.ToBase64String(hashedPasswordBytes);
            }
        }
    }
}
