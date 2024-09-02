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
        public IActionResult Registration()
        {
            return View("Registration");
        }
        public IActionResult AddEmployerProfile()
        {
            return View();
        }
        public IActionResult AddJobseekerProfile()
        {
            return View();
        }
    }
}