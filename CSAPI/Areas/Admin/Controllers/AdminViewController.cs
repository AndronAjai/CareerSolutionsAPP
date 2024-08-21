using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    public class AdminViewController : Controller
    {
        public IActionResult Index()
        {
            // Retrieve the token from ViewBag if needed
            var token = TempData["Token"] as string;

            // Handle token if needed
            ViewBag.Token = token;

            return View();
        }
    }
}
