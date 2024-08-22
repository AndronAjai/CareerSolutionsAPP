using CSAPI.Models;
using DbCreationApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        //[HttpGet("dashboard")]
        //public IActionResult GetDashboard()
        //{
        //    // Admin-specific logic
        //    return Ok("Admin Dashboard Data");
        //}

        private readonly IUserRepo _repo;

        public AdminController(IUserRepo repo)
        {
            _repo = repo;
        }

        // GET: api/User
        [HttpGet("dashboard")]
        //[AllowAnonymous]
        public async Task<ActionResult<IEnumerable<User>>> ShowAll()
        {
            var users = await _repo.GetAllAsync();
            return Ok(users);
        }

    }
}
