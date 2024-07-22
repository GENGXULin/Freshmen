using COMP3851B.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace COMP3851B.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult AdminPanel()
        {
            return View();
        }

        // FAQ 管理视图方法
        public async Task<IActionResult> FAQsManagement()
        {
            var faqs = await _context.FAQs.ToListAsync();
            return View(faqs);
        }

        // 创建 FAQ 视图方法
        public IActionResult CreateFAQ()
        {
            return View();
        }

        // 创建 FAQ POST 方法
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFAQ([Bind("Id,Question,Answer")] FAQs faq)
        {
            if (ModelState.IsValid)
            {
                _context.Add(faq);
                await _context.SaveChangesAsync();

                // 获取当前登录用户的用户名
                var userName = Request.Cookies["AuthCookie"];

                // 检查用户名是否为 null
                if (string.IsNullOrEmpty(userName))
                {
                    throw new Exception("当前用户未登录或用户名为空");
                }

                // 创建日志记录
                var log = new Log
                {
                    UserName = userName,
                    LogMessage = "creat a new FAQ",
                    LogDate = DateTime.Now,
                    Number = faq.Id.ToString()
                };

                // 添加日志记录到数据库
                _context.Logs.Add(log);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(FAQsManagement));
            }
            return View(faq);
        }

        // 编辑 FAQ 视图方法
        public async Task<IActionResult> EditFAQ(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faq = await _context.FAQs.FindAsync(id);
            if (faq == null)
            {
                return NotFound();
            }
            return View(faq);
        }

        // 编辑 FAQ POST 方法
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFAQ(int id, [Bind("Id,Question,Answer")] FAQs faq)
        {
            if (id != faq.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(faq);
                    await _context.SaveChangesAsync();

                    // 获取当前登录用户的用户名
                    var userName = Request.Cookies["AuthCookie"];

                    // 检查用户名是否为 null
                    if (string.IsNullOrEmpty(userName))
                    {
                        throw new Exception("当前用户未登录或用户名为空");
                    }

                    // 创建日志记录
                    var log = new Log
                    {
                        UserName = userName,
                        LogMessage = "edit FAQ",
                        LogDate = DateTime.Now,
                        Number = faq.Id.ToString()
                    };

                    // 添加日志记录到数据库
                    _context.Logs.Add(log);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FAQExists(faq.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(FAQsManagement));
            }
            return View(faq);
        }


        // 删除 FAQ 视图方法
        public async Task<IActionResult> DeleteFAQ(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faq = await _context.FAQs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (faq == null)
            {
                return NotFound();
            }

            return View(faq);
        }

        // 删除 FAQ POST 方法
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFAQ(int id)
        {
            var faq = await _context.FAQs.FindAsync(id);
            if (faq == null)
            {
                return NotFound();
            }

            _context.FAQs.Remove(faq);
            await _context.SaveChangesAsync();

            // 获取当前登录用户的用户名
            var userName = Request.Cookies["AuthCookie"];

            // 检查用户名是否为 null
            if (string.IsNullOrEmpty(userName))
            {
                throw new Exception("当前用户未登录或用户名为空");
            }

            // 创建日志记录
            var log = new Log
            {
                UserName = userName,
                LogMessage = "delete FAQ",
                LogDate = DateTime.Now,
                Number = faq.Id.ToString()
            };

            // 添加日志记录到数据库
            _context.Logs.Add(log);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(FAQsManagement));
        }

        private bool FAQExists(int id)
        {
            return _context.FAQs.Any(e => e.Id == id);
        }



        // 用户管理视图方法
        public async Task<IActionResult> UserManagement()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        // 删除用户的方法
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(UserManagement));
        }

        // 任命用户为admin的方法
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakeAdmin(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.IsAdmin = true;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(UserManagement));
        }

        // Announcements 管理视图方法
        public async Task<IActionResult> AnnouncementsManagement()
        {
            var announcements = await _context.Announcements.ToListAsync();
            return View(announcements);
        }

        // 创建 Announcement 视图方法
        public IActionResult CreateAnnouncement()
        {
            return View();
        }

        // 创建 Announcement POST 方法
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAnnouncement([Bind("Id,Title,Content,Publisher")] Announcements announcement)
        {
            if (ModelState.IsValid)
            {
                announcement.PublishDate = DateTime.Now;
                announcement.LastUpdateDate = DateTime.Now;
                _context.Add(announcement);
                await _context.SaveChangesAsync();

                // 获取当前登录用户的用户名
                var userName = Request.Cookies["AuthCookie"];

                // 检查用户名是否为 null
                if (string.IsNullOrEmpty(userName))
                {
                    throw new Exception("Current user is not logged in or the username is empty");
                }

                // 创建日志记录
                var log = new Log
                {
                    UserName = userName,
                    LogMessage = "Created a new Announcement",
                    LogDate = DateTime.Now,
                    Number = announcement.Id.ToString()
                };

                // 添加日志记录到数据库
                _context.Logs.Add(log);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(AnnouncementsManagement));
            }
            return View(announcement);
        }

        // 编辑 Announcement 视图方法
        public async Task<IActionResult> EditAnnouncement(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement == null)
            {
                return NotFound();
            }
            return View(announcement);
        }

        // 编辑 Announcement POST 方法
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAnnouncement(int id, [Bind("Id,Title,Content,Publisher,PublishDate,LastUpdateDate")] Announcements announcement)
        {
            if (id != announcement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    announcement.LastUpdateDate = DateTime.Now;
                    _context.Update(announcement);
                    await _context.SaveChangesAsync();

                    // 获取当前登录用户的用户名
                    var userName = Request.Cookies["AuthCookie"];

                    // 检查用户名是否为 null
                    if (string.IsNullOrEmpty(userName))
                    {
                        throw new Exception("Current user is not logged in or the username is empty");
                    }

                    // 创建日志记录
                    var log = new Log
                    {
                        UserName = userName,
                        LogMessage = "Edited Announcement",
                        LogDate = DateTime.Now,
                        Number = announcement.Id.ToString()
                    };

                    // 添加日志记录到数据库
                    _context.Logs.Add(log);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnnouncementExists(announcement.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(AnnouncementsManagement));
            }
            return View(announcement);
        }

        // 删除 Announcement 视图方法
        public async Task<IActionResult> DeleteAnnouncement(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcement = await _context.Announcements
                .FirstOrDefaultAsync(m => m.Id == id);
            if (announcement == null)
            {
                return NotFound();
            }

            return View(announcement);
        }

        // 删除 Announcement POST 方法
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAnnouncement(int id)
        {
            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement == null)
            {
                return NotFound();
            }

            _context.Announcements.Remove(announcement);
            await _context.SaveChangesAsync();

            // 获取当前登录用户的用户名
            var userName = Request.Cookies["AuthCookie"];

            // 检查用户名是否为 null
            if (string.IsNullOrEmpty(userName))
            {
                throw new Exception("Current user is not logged in or the username is empty");
            }

            // 创建日志记录
            var log = new Log
            {
                UserName = userName,
                LogMessage = "Deleted Announcement",
                LogDate = DateTime.Now,
                Number = announcement.Id.ToString()
            };

            // 添加日志记录到数据库
            _context.Logs.Add(log);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(AnnouncementsManagement));
        }

        private bool AnnouncementExists(int id)
        {
            return _context.Announcements.Any(e => e.Id == id);
        }



        // 显示日志记录的视图方法
        public async Task<IActionResult> LogsManagement()
        {
            var logs = await _context.Logs.ToListAsync();
            return View(logs);
        }

    }
}
