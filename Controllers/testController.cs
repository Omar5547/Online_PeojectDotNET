using Microsoft.AspNetCore.Mvc;

namespace Souq.Controllers
{
    public class testController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult xyz()
        {
            return View();
        }
    }
}
