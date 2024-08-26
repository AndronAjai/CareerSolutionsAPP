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

        public IActionResult EmployerSignUp()
        {
            return View();
        }

        public IActionResult JobSeekerSignUp()
        {
            return View();
        }

        public IActionResult NewEmployer()
        {
            return View();
        }

        public IActionResult NewJobSeeker()
        {
            return View();
        }
    }
}
