
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CSAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;


namespace CSAPI.Areas.Employers.Controllers
{
    [Area("Employers")]
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class EmployerController : ControllerBase
    {
        IEmployerRepo _repo;
        IApplicationRepo _applicationRepo;
        IJobsRepo _jobsRepo;
        IJobSeekerRepo _jobSeekerRepo;

        public EmployerController(IEmployerRepo repo, IApplicationRepo applicationRepo, IJobsRepo jobsRepo, IJobSeekerRepo jobSeekerRepo)
        {
            _repo = repo;
            _applicationRepo = applicationRepo;
            _jobsRepo = jobsRepo;
            _jobSeekerRepo = jobSeekerRepo;
        }

        // GET: api/Employers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employer>>> ShowAll()
        {
            var employers = await _repo.GetAllAsync();
            return Ok(employers);
        }

        // GET: api/Employers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employer>> FindEmployer(int id)
        {
            var employer = await _repo.FindByIdAsync(id);
            if (employer == null)
            {
                return NotFound();
            }
            return Ok(employer);
        }

        // POST: api/Employers
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Employer emp)
        {
            var success = await _repo.AddEmployerAsync(emp);
            if (!success)
            {
                return BadRequest("Invalid data or user does not exist.");
            }
            return StatusCode((int)HttpStatusCode.Created);
        }

        // PUT: api/Employers/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Employer emp)
        {
            var success = await _repo.UpdateEmployerAsync(id, emp);
            if (!success)
            {
                return NotFound("Employer not found or user does not exist.");
            }
            return NoContent();
        }

        // DELETE: api/Employers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _repo.DeleteEmployerAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        // GET: api/Employer/Applications
        [HttpGet("AllApplications")]
        public async Task<ActionResult<IEnumerable<Application>>> ShowAllApplications()
        {
            var applications = await _applicationRepo.GetAllAsync();
            return Ok(applications);
        }

        // GET: api/Employer/Applications/5
        [HttpGet("ApplicationDetails/{id}")]
        public async Task<ActionResult<Application>> GetApplication(int id)
        {
            var application = await _applicationRepo.FindByIdAsync(id);
            if (application == null)
            {
                return NotFound();
            }
            return Ok(application);
        }

        // GET: api/Employer/Jobs
        [HttpGet("AllJobs")]
        public async Task<ActionResult<IEnumerable<Job>>> ShowAllJobs()
        {
            var jobs = await _jobsRepo.GetAllAsync();
            return Ok(jobs);
        }

        // POST: api/Employer/Jobs
        [HttpPost("AddJob")]
        public async Task<ActionResult> PostJob([FromBody] Job job)
        {
            var success = await _jobsRepo.AddJobAsync(job);
            if (!success)
            {
                return BadRequest("Invalid job data.");
            }
            return StatusCode((int)HttpStatusCode.Created);
        }

        // PUT: api/Employer/Jobs/5
        [HttpPut("UpdateJob/{id}")]
        public async Task<ActionResult> UpdateJob(int id, [FromBody] Job job)
        {
            var success = await _jobsRepo.UpdateJobAsync(id, job);
            if (!success)
            {
                return NotFound("Job not found or data invalid.");
            }
            return NoContent();
        }

        // DELETE: api/Employer/Jobs/5
        [HttpDelete("DeleteJob/{id}")]
        public async Task<ActionResult> DeleteJob(int id)
        {
            var success = await _jobsRepo.DeleteJobAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        // GET: api/Employer/JobSeeker
        [HttpGet("AllJobSeeker")]
        public async Task<ActionResult<IEnumerable<Job>>> ShowAllJobSeeker()
        {
            var jobseeker = await _jobSeekerRepo.GetAllAsync();
            return Ok(jobseeker);

        }
    }
}