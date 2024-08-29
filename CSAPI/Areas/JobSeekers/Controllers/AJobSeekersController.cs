
using CSAPI.Areas.JobSeekers.Models;
using CSAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;

namespace CSAPI.Areas.JobSeekers.Controllers
{
    [Area("JobSeekers")]
    [Route("api/jobseekers/[controller]")]
    [ApiController]
    [Authorize(Roles = "JobSeeker")]
    public class AJobSeekersController : ControllerBase
    {
        // Constructor creation mapping Irepository (Function Connecting Backend)
        IBranchOfficeRepo _AbrRepo;
        IJobsRepo _AjbRepo;
        IApplicationRepo _AapnRepo;
        AIApplicationRepo _AzapnRepo;
        IUserRepo _AusRepo;
        IJobSeekerRepo _AjsRepo;

        public AJobSeekersController(IBranchOfficeRepo Arepo, IJobsRepo AjbRepo, IApplicationRepo AapnRepo, IUserRepo AusRepo, IJobSeekerRepo AjsRepo)
            {
            _AbrRepo = Arepo;
            _AjbRepo = AjbRepo;
            _AapnRepo = AapnRepo;
            _AusRepo = AusRepo;
            _AjsRepo = AjsRepo;
            }

        //[HttpGet("jsResume")]
        //[Authorize(Roles = "User", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //public async Task<ActionResult<JobSeeker>> GetMyResume(int id)
        //{
        //    var js = await _AjsRepo.GetByIdAsync(id);
        //    var currentUserId = User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
        //    if (userBooking.UserId_FK.ToString() != currentUserId)
        //    {
        //        return Forbid("You are not authorized to download this ticket.");
        //    }

        //    var booking = await _bookingRepo.GetByIdAsync(id);
        //    if (booking == null)
        //    {
        //        return NotFound();
        //    }

        //    var content = new StringBuilder();
        //    content.AppendLine($"Booking ID: {booking.BookingId}");
        //    content.AppendLine($"Flight Number:{booking.FlightIdFK}");
        //    content.AppendLine($"Booking Date:{booking.BookingDate}");
        //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Exports", $"Booking_{booking.BookingId}.txt");
        //    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        //    await System.IO.File.WriteAllTextAsync(filePath, content.ToString());
        //    var bytes = await System.IO.File.ReadAllBytesAsync(filePath);
        //    return File(bytes, "text/plain", Path.GetFileName(filePath));
        //}

        [HttpGet("viewBOR")]
        public async Task<ActionResult<IEnumerable<BranchOffice>>> ShowAll()
            {
            List<BranchOffice>? jsviewbor = await _AbrRepo.GetAllAsync();
            return Ok(jsviewbor);
            }

        // Job Seeker Can see the jobs available 
        [HttpGet("ViewJobs")]
        public async Task<ActionResult<IEnumerable<Job>>> DisplayJobs()
            {
            List<Job> jsviewjobs = await _AjbRepo.GetAllAsync();
            return Ok(jsviewjobs);
            }


        // Job Seeker Can View His own Application

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

        // Job Seeker Can Update His own Application
        [HttpPut("UpdatejsApplication")]

        public async Task<ActionResult<IEnumerable<JobApplication>>> jsupdateAppln([FromBody] IEnumerable<JobApplication>  Apj)
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

        // Job Seeker Can Delete His own Application(need to implement(few confusions)

        [HttpDelete("DeletejsApplication")]
        public async Task<ActionResult<IEnumerable<JobApplication>>> jsdeleteAppln(int id)
            {
            // Retrieve the 'UserId' cookie from the request
            // var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
            var userIdClaim = User.FindFirst("UserId")?.Value;
            bool x = int.TryParse(userIdClaim, out var userIdCookie);
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


        // Job Seeker Can View His own User profile

        [HttpGet("ViewjsUser")]
        public async Task<ActionResult<IEnumerable<User>>> jsviewUser()
            {

            //var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
            var userIdClaim = User.FindFirst("UserId")?.Value;
            bool x = int.TryParse(userIdClaim, out var userIdCookie);


            var viewjsappln = await _AusRepo.FindByIdAsync(userIdCookie);
            if (viewjsappln == null)
                {
                return NotFound();
                }
            return Ok(viewjsappln);
            }



        // Job seeker Basic Controlling
        // GET: api/JobSeeker
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<JobSeeker>>> ShowAlljs()
        //    {
        //    var jobSeekers = await _AjsRepo.GetAllAsync();
        //    return Ok(jobSeekers);
        //    }

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

        // POST: api/JobSeeker
        [HttpPost("Addjsprofile")]
        public async Task<ActionResult> Post([FromBody] JobSeeker js)
            {
            var success = await _AjsRepo.AddJobSeekerAsync(js);
            if (!success)
                {
                return BadRequest("Invalid data or user does not exist.");
                }
            return StatusCode((int)HttpStatusCode.Created);
            }

        // PUT: api/JobSeeker/5
        [HttpPut("editjsuserprofile")]
        public async Task<ActionResult> Put( [FromBody] JobSeeker js)
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
