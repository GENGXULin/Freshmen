using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Entities;
using web.Models;

namespace web.Controllers
{

    public class AddressController : Controller
    {
        private readonly StoreDbContext _context;




        public AddressController(StoreDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            var userName = User.Identity.Name;
            var userAddresses = _context.Addresses.Where(a => a.UserName == userName).ToList();
            return View(userAddresses);
        }

        public IActionResult Add()

        {
            return View();
        }




        [HttpPost]
        public IActionResult Add(Address model)
        {
            if (!ModelState.IsValid)
            {
                // 如果 ModelState 验证失败，返回当前视图并显示错误消息
                return View(model);
            }

            // 创建新的 Address 实体
            var newAddress = new Address
            {
                UserName = model.UserName,
                Street = model.Street,
                City = model.City,
                State = model.State,
                PostCode = model.PostCode,
                Recipient = model.Recipient
            };

            // 添加到数据库
            _context.Addresses.Add(newAddress);
            _context.SaveChanges();

            // 成功时重定向到 UserAddresses 视图
            return RedirectToAction("Index");
        }


        public IActionResult Edit(int id)
        {
            var address = _context.Addresses.Find(id);
            if (address == null)
            {
                return NotFound();
            }
            return View(address);
        }

        [HttpPost]
        public IActionResult Edit(Address model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var address = _context.Addresses.Find(model.Id);
            if (address == null)
            {
                return NotFound();
            }

            address.Street = model.Street;
            address.City = model.City;
            address.State = model.State;
            address.PostCode = model.PostCode;
            address.Recipient = model.Recipient;

            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            var address = _context.Addresses.Find(id);
            if (address == null)
            {
                return Json(new { success = false, message = "Address not found." });
            }

            _context.Addresses.Remove(address);
            _context.SaveChanges();
            return Json(new { success = true, message = "Address deleted successfully." });
        }

        [HttpPost]
        public async Task<IActionResult> SetDefault(int id)
        {
            var userName = User.Identity.Name;
            var address = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == id && a.UserName == userName);

            if (address == null)
            {
                return NotFound();
            }

            // Set all other addresses to not default
            var userAddresses = await _context.Addresses.Where(a => a.UserName == userName).ToListAsync();
            foreach (var addr in userAddresses)
            {
                addr.isDefault = false;
            }

            // Set the selected address as default
            address.isDefault = true;
            _context.Addresses.UpdateRange(userAddresses);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



    }
}

