using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Areas.Employer.Controllers
{
    [Area("Employer")]
    [Route("api/employer/[controller]")]
    [ApiController]
    [Authorize(Roles = "Employer")]
    public class EmployerController : ControllerBase
    {
        [HttpGet("dashboard")]
        public IActionResult GetDashboard()
        {
            // Employer-specific logic
            return Ok("Employer Dashboard Data");
        }
    }
}