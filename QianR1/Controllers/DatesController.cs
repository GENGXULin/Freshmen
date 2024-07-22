using Microsoft.AspNetCore.Mvc;

namespace COMP3851B.Controllers
{
    public class DatesController : Controller
    {
        public IActionResult Dates()
        {
            return View();
        }
    }
}
