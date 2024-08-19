using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
