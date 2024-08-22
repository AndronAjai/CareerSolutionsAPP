using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminMVCController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }

        // Branch Office Views
        public IActionResult Branches()
        {
            return View();
        }

        public IActionResult BranchDetails(int id)
        {
            ViewBag.BranchId = id;
            return View();
        }

        public IActionResult AddBranch()
        {
            return View();
        }

        public IActionResult UpdateBranch(int id)
        {
            ViewBag.BranchId = id;
            return View();
        }

        // User Views
        public IActionResult Users()
        {
            return View();
        }

        public IActionResult UserDetails(int id)
        {
            ViewBag.UserId = id;
            return View();
        }

        // Employer Views
        public IActionResult Employers()
        {
            return View();
        }

        public IActionResult EmployerDetails(int id)
        {
            ViewBag.EmployerId = id;
            return View();
        }

        // Job Seeker Views
        public IActionResult JobSeekers()
        {
            return View();
        }

        public IActionResult JobSeekerDetails(int id)
        {
            ViewBag.JobSeekerId = id;
            return View();
        }

        // Job Views
        public IActionResult Jobs()
        {
            return View();
        }

        public IActionResult JobDetails(int id)
        {
            ViewBag.JobId = id;
            return View();
        }

        // Application Views
        public IActionResult Applications()
        {
            return View();
        }

        public IActionResult ApplicationDetails(int id)
        {
            ViewBag.ApplicationId = id;
            return View();
        }
    }
}
