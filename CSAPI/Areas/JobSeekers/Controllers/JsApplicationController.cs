using CSAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CSAPI.Areas.JobSeekers.Controllers
    {
    [Area("JobSeekers")]
    [Route("api/jobseekers/[controller]")]
    [ApiController]
    [Authorize(Roles = "JobSeeker")]
    public class JsApplicationController : ControllerBase
        {
        IApplicationRepo _AapnRepo;
        public JsApplicationController(IApplicationRepo AapnRepo)
            {
                _AapnRepo = AapnRepo;
            }

        // Job Seeker Can View His own JobApplication

        [HttpGet("ViewjsApplication")]
        public async Task<ActionResult<IEnumerable<JobApplication>>> jsviewAppln()
            {

            var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);


            var viewjsappln = await _AapnRepo.FindByJobSeekerIdAsync(userIdCookie);
            if (viewjsappln == null)
                {
                return NotFound();
                }
            return Ok(viewjsappln);
            }

        // Job Seeker Can Update His own JobApplication
        [HttpPut("UpdatejsApplication")]

        public async Task<ActionResult<IEnumerable<JobApplication>>> jsupdateAppln([FromBody] IEnumerable<JobApplication> Apj)
            {
            // Retrieve the 'UserId' cookie from the request
            var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
            if (userIdCookie == null)
                {
                return NotFound();
                }

            var success = await _AapnRepo.UpdateJobSeekerIdAsync(userIdCookie, Apj);
            return Ok(success);
            }

        // Job Seeker Can Delete His own JobApplication(need to implement(few confusions)

        [HttpDelete("DeletejsApplication")]
        public async Task<ActionResult<IEnumerable<JobApplication>>> jsdeleteAppln(int id)
            {
            // Retrieve the 'UserId' cookie from the request
            var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
            if (userIdCookie == null)
                {
                return NotFound();
                }

            var success = await _AapnRepo.DeleteApplicationAsync(userIdCookie);
            if (!success)
                {
                return BadRequest("Could not delete the Application.");
                }

            return NoContent();
            }
        }
    }
