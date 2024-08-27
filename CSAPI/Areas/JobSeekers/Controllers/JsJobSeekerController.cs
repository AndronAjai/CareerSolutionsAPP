using CSAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CSAPI.Areas.JobSeekers.Controllers
    {

    [Area("JobSeekers")]
    [Route("api/jobseekers/[controller]")]
    [ApiController]
    [Authorize(Roles = "JobSeeker")]
    public class JsJobSeekerController : ControllerBase
        {
        IJobSeekerRepo _AjsRepo;

        public JsJobSeekerController(IJobSeekerRepo AjsRepo)
            {
            _AjsRepo = AjsRepo;
            }
        // GET: api/JobSeeker/5
        [HttpGet("viewjsprofile")]
        public async Task<ActionResult<JobSeeker>> FindJobSeeker()
            {
            //var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
            var userIdClaim = User.FindFirst("UserId")?.Value;
            bool x = int.TryParse(userIdClaim, out var userIdCookie);
            var jobSeeker = await _AjsRepo.FindByIdAsync(userIdCookie);
            if (jobSeeker == null)
                {
                return NotFound();
                }
            return Ok(jobSeeker);
            }

        // PUT: api/JobSeeker/5
        [HttpPut("editjsuserprofile")]
        public async Task<ActionResult> Put([FromBody] JobSeeker js)
            {
            //var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
            var userIdClaim = User.FindFirst("UserId")?.Value;
            bool x = int.TryParse(userIdClaim, out var userIdCookie);
            var success = await _AjsRepo.UpdateJobSeekerAsync(userIdCookie, js);
            if (!success)
                {
                return NotFound("JobSeeker not found or user does not exist.");
                }
            return NoContent();
            }

        // DELETE: api/JobSeeker/5
        [HttpDelete("Deletejsuserprofile")]
        public async Task<ActionResult> Delete()
            {
            //var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
            var userIdClaim = User.FindFirst("UserId")?.Value;
            bool x = int.TryParse(userIdClaim, out var userIdCookie);
            var success = await _AjsRepo.DeleteJobSeekerAsync(userIdCookie);
            if (!success)
                {
                return NotFound();
                }
            return NoContent();
            }



        }
    }
