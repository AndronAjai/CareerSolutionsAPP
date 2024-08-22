using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Controllers
{
    [AllowAnonymous]
    public class LoginViewController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
