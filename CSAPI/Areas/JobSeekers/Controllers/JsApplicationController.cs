using CSAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
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
        INotificationRepo _AnotiRepo;
        public JsApplicationController(IApplicationRepo AapnRepo, INotificationRepo AnotiRepo)
            {
                _AapnRepo = AapnRepo;
                _AnotiRepo = AnotiRepo;
            }
        [HttpPost("AddJsapplication{jobid}")]
        public async Task<ActionResult> PostJob(int jobid,[FromBody] JobApplication appln)
            {
            //var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
            var userIdClaim = User.FindFirst("UserId")?.Value;
            bool x = int.TryParse(userIdClaim, out var userIdCookie);


            if (appln.JobID != jobid)
                {
                throw new ArgumentException("Wrong Job Id Failed to Apply Job");
                }
         
         

            var success = await _AapnRepo.AddApplicationAsync(userIdCookie,appln);
            if (!success)
                {
                return NotFound("Job not found or data invalid. or Already Applied to the job");
                }
            else
                {
                // JobSeeker can Applied to ApplicationID
                Notification InsertObj = new Notification();
                InsertObj.Message = "Have Successfully Applied to the job";
                InsertObj.ApplicationID = appln.ApplicationID;
                InsertObj.EmployerID = await _AapnRepo.FindEmpID(appln.JobID);

                var messageposted = await _AnotiRepo.AddNotificationAsync(InsertObj);
                if (!messageposted)
                    {
                    return NotFound("Message failed to insert");
                    }
                else
                    {
                    return StatusCode((int)HttpStatusCode.Created);
                    }

                
                }
            
            }

        // Job Seeker Can View His own JobApplication

        [HttpGet("ViewjsApplication")]
        public async Task<ActionResult<IEnumerable<JobApplication>>> jsviewAppln()
            {

            //var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
            var userIdClaim = User.FindFirst("UserId")?.Value;
            bool x = int.TryParse(userIdClaim, out var userIdCookie);

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
            //var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
            var userIdClaim = User.FindFirst("UserId")?.Value;
            bool x = int.TryParse(userIdClaim, out var userIdCookie);
            if (userIdCookie == null)
                {
                return NotFound();
                }

            var success = await _AapnRepo.UpdateJobSeekerIdAsync(userIdCookie, Apj);
            return Ok(success);
            }

        // Job Seeker Can Delete His own JobApplication(need to implement(few confusions)

        [HttpDelete("DeletejsApplication")]
        public async Task<ActionResult<IEnumerable<JobApplication>>> jsdeleteAppln(int applnid)
            {
            // Retrieve the 'UserId' cookie from the request
            //var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
            var userIdClaim = User.FindFirst("UserId")?.Value;
            bool x = int.TryParse(userIdClaim, out var userIdCookie);
            if (userIdCookie == null)
                {
                return NotFound();
                }

            var success = await _AapnRepo.DeleteApplicationAsync(applnid);
            if (!success)
                {
                return BadRequest("Could not delete the Application.");
                }

            else
                {
                // job application deleted


                Notification noti2 = new Notification();
                var messageposted = await _AnotiRepo.AddDeleteNotificationAsync(applnid, noti2);
                if (!messageposted)
                    {
                    return NotFound("Message failed to insert");
                    }
                else
                    {
                    return StatusCode((int)HttpStatusCode.Created);
                    }

                }

            return NoContent();
            }
        }
    }