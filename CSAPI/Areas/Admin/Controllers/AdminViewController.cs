using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    //[AllowAnonymous]
    public class AdminViewController : Controller
    {
        public IActionResult Index()
        {
            //// Retrieve the token from ViewBag if needed
            //var token = TempData["token"] as string;

            ////// Handle token if needed
            ////ViewBag.Token = token;

            ////return View();

            //// Access the token (optional, for logging or other purposes)
            ////var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            //// Access the user's claims (e.g., username, role, etc.)
            //var userName = User.Identity.Name;
            //var userRole = User.FindFirst("role")?.Value;

            //// You can now use this information in your controller actions
            //ViewBag.Token = token;
            //ViewBag.UserName = userName;
            //ViewBag.UserRole = userRole;

            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            ViewBag.Token = token;

            return View();

        }
    }
}
