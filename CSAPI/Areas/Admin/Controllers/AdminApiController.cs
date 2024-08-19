using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class AdminApiController : Controller
    {
        [HttpGet("page1")]
        public IActionResult Page1()
        {
            return View();
        }


        [HttpGet("page2")]
        public IActionResult Page2()
        {
            return View();
        }
    }
}

