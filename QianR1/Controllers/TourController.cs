using Microsoft.AspNetCore.Mvc;

namespace COMP3851B.Controllers
{
    public class TourController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
