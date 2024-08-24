using CSAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CSAPI.Areas.JobSeekers.Controllers
    {
    [Area("JobSeekers")]
    [Route("api/jobseekers/[controller]")]
    [ApiController]
    [Authorize(Roles = "JobSeeker")]
    public class JsUserController : ControllerBase
        {

        IUserRepo _AusRepo;
        public JsUserController(IUserRepo AusRepo)
            {
            _AusRepo =  AusRepo;
            }
        // Job Seeker Can View His own User profile

        [HttpGet("ViewjsUser")]
        public async Task<ActionResult<IEnumerable<User>>> jsviewUser()
            {

            var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);


            var viewjsappln = await _AusRepo.FindByIdAsync(userIdCookie);
            if (viewjsappln == null)
                {
                return NotFound();
                }
            return Ok(viewjsappln);
            }
        }
    }
