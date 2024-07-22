//author：Mark Lim Shan Jin
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using web.Entities;
using web.Models;

namespace web.Controllers
{
    public class AccountController : Controller
    {
        private readonly StoreDbContext _context;


        public AccountController(StoreDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public IActionResult Index()
        {
            return View(_context.UserAccounts.ToList());
        }

        public IActionResult Registration()
        {
            return View();

        }
        [HttpPost]
        public IActionResult Registration(Registration model)
        {
            if (ModelState.IsValid)
            {
                // 检查邮箱和用户名是否唯一
                var existingUser = _context.UserAccounts
                    .FirstOrDefault(u => u.Email == model.Email || u.UserName == model.UserName);

                if (existingUser != null)
                {
                    if (existingUser.Email == model.Email)
                    {
                        ModelState.AddModelError("Email", "The email address is already registered.");
                    }

                    if (existingUser.UserName == model.UserName)
                    {
                        ModelState.AddModelError("UserName", "The username is already taken.");
                    }

                    return View(model);
                }

                // 如果唯一性检查通过，创建用户账户
                UserAccount account = new UserAccount
                {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Password = model.Password,
                    UserName = model.UserName,
                    Role = model.Role
                };

                try
                {
                    _context.UserAccounts.Add(account);
                    _context.SaveChanges();
                    ModelState.Clear();
                    ViewBag.Message = $"{account.FirstName} {account.LastName} registered successfully, Please login";
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving the user. Please try again.");
                    return View(model);
                }

                return View();
            }

            return View(model);
        }

        public IActionResult login()
        {

            return View();

        }




        [HttpPost]
        public IActionResult Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.UserAccounts.Where
                    (x => (x.UserName == model.UserNameOrEmail || x.Email == model.UserNameOrEmail)
                    && x.Password == model.Password).FirstOrDefault();
                if (user != null)
                {
                    //success

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,user. UserName),
                        new Claim(ClaimTypes.Email,user. Email),
                        new Claim("Name",user.FirstName),
                        new Claim(ClaimTypes.Role,user.Role),
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    if (user.Role == "Admin")
                    {
                        return RedirectToAction("AdminPage");
                    }
                    if (user.Role == "Seller")
                    {
                        return RedirectToAction("SecurePage_seller");
                    }
                    else
                    {
                        return RedirectToAction("SecurePage");
                    }
                }



                else
                {
                    ModelState.AddModelError("", "UserName/Email or password incorrect");
                }


            }
            return View(model);
        }

        public IActionResult logOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["Message"] = "You have been successfully logged out.";
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "User")]
        public IActionResult SecurePage()
        {
            ViewBag.Name = HttpContext.User.Identity.Name;
            return View();
        }

        [Authorize(Roles = "Seller")]
        public IActionResult SecurePage_seller()
        {
            ViewBag.Name = HttpContext.User.Identity.Name;
            return View();
        }



        [Authorize(Roles = "Admin")]
        public IActionResult AdminPage()
        {
            ViewBag.Name = HttpContext.User.Identity.Name;
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ManageAccounts()
        {
            var users = _context.UserAccounts.Where(u => u.Role == "User").ToList();
            return View(users);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteAccount(int id)
        {
            var user = _context.UserAccounts.Find(id);
            if (user != null)
            {
                _context.UserAccounts.Remove(user);
                _context.SaveChanges();
            }
            return RedirectToAction("ManageAccounts");
        }


        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult ChangePassword(Models.ChangePassword model)
        {
            if (ModelState.IsValid)
            {
                var userName = User.Identity.Name;
                var user = _context.UserAccounts.FirstOrDefault(u => u.UserName == userName);
                if (user != null && model.NewPassword == model.ConfirmPassword)
                {
                    user.Password = model.NewPassword;
                    _context.SaveChanges();
                    ViewBag.Message = "Password changed successfully.";
                }
                else
                {
                    ModelState.AddModelError("", "Password confirmation does not match.");
                }
            }
            return View(model);
        }


     }

}
