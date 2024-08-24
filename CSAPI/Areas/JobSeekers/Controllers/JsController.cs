using CSAPI.Models;
using DbCreationApp.Models;
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
    public class JsController : ControllerBase
        {
        IJobSeekerRepo _AjsRepo;

        public JsController(IJobSeekerRepo AjsRepo)
            {
            _AjsRepo = AjsRepo;
            }
        // GET: api/JobSeeker/5
        [HttpGet("viewjsprofile")]
        public async Task<ActionResult<JobSeeker>> FindJobSeeker()
            {
            var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
            var jobSeeker = await _AjsRepo.FindByIdAsync(userIdCookie);
            if (jobSeeker == null)
                {
                return NotFound();
                }
            return Ok(jobSeeker);
            }

        //// POST: api/JobSeeker
        //[HttpPost("Addjsprofile")]
        //public async Task<ActionResult> Post([FromBody] JobSeeker js)
        //    {
        //    var success = await _AjsRepo.AddJobSeekerAsync(js);
        //    if (!success)
        //        {
        //        return BadRequest("Invalid data or user does not exist.");
        //        }
        //    return StatusCode((int)HttpStatusCode.Created);
        //    }

        // PUT: api/JobSeeker/5
        [HttpPut("editjsuserprofile")]
        public async Task<ActionResult> Put([FromBody] JobSeeker js)
            {
            var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
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
            var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
            var success = await _AjsRepo.DeleteJobSeekerAsync(userIdCookie);
            if (!success)
                {
                return NotFound();
                }
            return NoContent();
            }



        }
    }
