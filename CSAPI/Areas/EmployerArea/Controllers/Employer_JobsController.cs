using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CSAPI.Areas.EmployerArea.Models;
using CSAPI.Models;
using System.Net;

namespace CSAPI.Areas.EmployerArea.Controllers
{
    [Area("EmployerArea")]
    [Route("api/[controller]/")]
    [ApiController]
    [Authorize(Roles = "Employer")]
    public class Employer_JobsController : ControllerBase
    {
        private readonly IEmployerRepo _repo;
        private readonly IJobsRepo _jobsRepo;
        private readonly IEmployerAreaRepo _eRepo;
       

        public Employer_JobsController(IEmployerRepo repo, IJobsRepo jobsRepo,IEmployerAreaRepo empRepo)
        {
            _repo = repo;     
            _jobsRepo = jobsRepo;
            _eRepo = empRepo;
        }

        [HttpGet("AllJobs")]
        public async Task<IQueryable<Job>> ShowAllJobs()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            bool x = int.TryParse(userIdClaim, out var userIdCookie);
            int Empid = _eRepo.GetEmpID(userIdCookie);
            return await _eRepo.GetMyJobs(Empid);
        }

        [HttpGet("Jobs/{id}")]
        public async Task<Job> GetJob(int id)
        {
            var job = await _jobsRepo.FindByIdAsync(id);
            return job;
        }


        [HttpPost("AddJob")]
        public async Task<ActionResult> PostJob([FromBody] Job job)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            bool x = int.TryParse(userIdClaim, out var userIdCookie);
            int Empid = _eRepo.GetEmpID(userIdCookie);
            job.EmployerID = Empid;
            var success = await _jobsRepo.AddJobAsync(job);
            if (!success)
            {
                return NotFound("Job not found or data invalid.");
            }
            return StatusCode((int)HttpStatusCode.Created);
        }

        [HttpPut("UpdateJob/{id}")]
        public async Task<ActionResult> UpdateJob(int id, [FromBody] Job job)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            bool x = int.TryParse(userIdClaim, out var userIdCookie);
            int Empid = _eRepo.GetEmpID(userIdCookie);
            job.EmployerID = Empid;
            var success = await _jobsRepo.UpdateJobAsync(id, job);
            if (!success)
            {
                return NotFound("Job not found or data invalid.");
            }
            return NoContent();
        }

        [HttpDelete("DeleteJob/{jobId}")]
        public async Task<ActionResult> DeleteJob(int jobId)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            bool x = int.TryParse(userIdClaim, out var userIdCookie);
            int Empid = _eRepo.GetEmpID(userIdCookie);
            
            var job = await _jobsRepo.FindByIdAsync(jobId);

            if (job == null || job.EmployerID != Empid)
            {
                return NotFound("Job not found or you're not authorized to delete this job.");
            }

            var success = await _jobsRepo.DeleteJobAsync(jobId);
            if (!success)
            {
                return BadRequest("Failed to delete the job.");
            }
            return NoContent();
        }
    }
}
