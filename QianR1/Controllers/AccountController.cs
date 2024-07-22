using Microsoft.AspNetCore.Mvc;
using COMP3851B.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace COMP3851B.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 显示登录页面
        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        // 处理登录表单提交
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LogIn(User model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // 查找用户
                    var user = _context.Users.SingleOrDefault(u => u.UserName == model.UserName);
                    if (user == null)
                    {
                        ModelState.AddModelError("", "用户名或密码错误");
                        ViewBag.Message = "用户名或密码错误";
                        ViewBag.IsSuccess = false;
                        return View(model);
                    }

                    // 验证密码
                    var hashedPassword = HashPassword(model.HashedPw, user.Salt);
                    if (hashedPassword == user.HashedPw)
                    {
                        // 密码正确
                        ViewBag.Message = "登录成功";
                        ViewBag.IsSuccess = true;

                        // 设置身份验证cookie
                        var options = new CookieOptions
                        {
                            Expires = DateTime.Now.AddDays(7),
                            HttpOnly = true,
                            Secure = Request.IsHttps
                        };
                        Response.Cookies.Append("AuthCookie", user.UserName, options);

                        return RedirectToAction("Profile", "Account");
                    }
                    else
                    {
                        ModelState.AddModelError("", "用户名或密码错误");
                        ViewBag.Message = "用户名或密码错误";
                        ViewBag.IsSuccess = false;
                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    // 增加日志记录
                    ModelState.AddModelError("", "发生错误：" + ex.Message);
                    ViewBag.Message = "发生错误：" + ex.Message;
                    ViewBag.IsSuccess = false;
                    return View(model);
                }
            }

            ViewBag.Message = "请输入有效的信息";
            ViewBag.IsSuccess = false;
            return View(model);
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

        // 显示用户个人信息页面
        [HttpGet]
        public IActionResult Profile()
        {
            // 从Cookie中获取用户名
            var userName = Request.Cookies["AuthCookie"];
            if (userName == null)
            {
                return RedirectToAction("LogIn", "Account");
            }

            // 从数据库中查找用户信息
            var user = _context.Users.SingleOrDefault(u => u.UserName == userName);
            if (user == null)
            {
                return RedirectToAction("LogIn", "Account");
            }

            // 将用户信息传递给视图
            return View(user);
        }

        // 处理用户登出
        public IActionResult Logout()
        {
            // 删除身份验证Cookie
            if (Request.Cookies["AuthCookie"] != null)
            {
                Response.Cookies.Delete("AuthCookie");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
