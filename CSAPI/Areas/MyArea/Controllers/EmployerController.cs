using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CSAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using CSAPI.Areas.MyArea.Models;

namespace CSAPI.Areas.MyArea.Controllers
{
    [Area("MyArea")]
    [Route("api/[controller]/")]
    [ApiController]
    [Authorize(Roles = "Employer")]
    public class EmployerController : ControllerBase
    {
       
        private readonly IEmployerRepo _repo;
        private readonly IApplicationRepo _applicationRepo;
        private readonly IJobsRepo _jobsRepo;
        private readonly IUserRepo _userRepo;
        private readonly IEmployerAreaRepo _eRepo;
        private readonly IJobSeekerRepo _JobSeekerRepo;
        private readonly IBranchOfficeRepo _BranchOfficeRepo;


        public EmployerController(IEmployerRepo repo, IApplicationRepo applicationRepo, IJobsRepo jobsRepo, IUserRepo userRepo, IEmployerAreaRepo empRepo,IJobSeekerRepo JobSeekerRepo, IBranchOfficeRepo BranchOfficeRepo)
        {
            _repo = repo;
            _applicationRepo = applicationRepo;
            _jobsRepo = jobsRepo;
            _userRepo = userRepo;
            _eRepo = empRepo;
            _JobSeekerRepo = JobSeekerRepo;
            _BranchOfficeRepo = BranchOfficeRepo;
        }

        //EmployerTable-------------------------------------------------------------------------------------------------

        [HttpGet("MyAccount")]
        public async Task<ActionResult<Employer>> ShowMyAccount()
        {
            
            var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
            int Empid = _eRepo.GetEmpID(userIdCookie);
            Employer e = await _repo.FindByIdAsync(Empid);

            if (e == null)
            {
                return NotFound("Employer not found.");
            }
           
            return Ok(e);
        }

        [HttpPut("UpdateInfo")]
        public async Task<bool> UpdateInfo([FromBody] Employer updatedEmployer)
        {
            var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
            int Empid = _eRepo.GetEmpID(userIdCookie);
            Employer e = await _repo.FindByIdAsync(Empid);
            return await _repo.UpdateEmployerAsync(Empid,e);         
        }

        [HttpDelete("DeleteAccount")]
        public async Task<bool> DeleteAccount()
        {
            var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
            int Empid = _eRepo.GetEmpID(userIdCookie);
            Employer e = await _repo.FindByIdAsync(Empid);
            return await _repo.DeleteEmployerAsync(Empid);     
        }

        //ApplicationTable---------------------------------------------------------------------------------------------------------------

        [HttpGet("AllApplications")]
        public async Task<IQueryable<JobApplication>> ShowAllApplications()
        {
            var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
            int Empid = _eRepo.GetEmpID(userIdCookie);
            return await _eRepo.GetMyApplications(Empid);
           
        }

        [HttpGet("ApplicationDetails/{id}")]
        public async Task<JobApplication> GetApplication(int id)
        {
            var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
            int Empid = _eRepo.GetEmpID(userIdCookie);
            var app = _eRepo.GetApplication(id);

            return await app;
        }

        [HttpGet("AllApplications/{id}")]
        public async Task<IQueryable<JobApplication>> ShowApplicationsForJob(int id)
        {
            var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
            int Empid = _eRepo.GetEmpID(userIdCookie);
            return await _eRepo.GetApplicationsOfJob(id);

            //Job job = await _jobsRepo.FindByIdAsync(jobid);
            //if (job.EmployerID == Empid)
            //{
            //    return await _eRepo.GetApplicationsOfJob(jobid);
            //}
        }

        //JobTable-----------------------------------------------------------------------------------------------------------------------

        [HttpGet("AllJobs")]
        public async Task<IQueryable<Job>> ShowAllJobs()
        {
            var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
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
            var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
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
            var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
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
            var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
            int Empid = _eRepo.GetEmpID(userIdCookie);
            //int authenticatedEmployerId = int.Parse(User.FindFirst("EmployerID").Value);
            
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

        //JobSeekerTable------------------------------------------------------------------------------------------------------

        [HttpGet("AllJobSeeker")]
        public async Task<ActionResult<IEnumerable<JobSeeker>>> ShowAllJobSeeker()
        {
            var jobseeker = await _JobSeekerRepo.GetAllAsync();
            return Ok(jobseeker);
        }

        [HttpGet("JobSeeker/{id}")]
        public async Task<JobSeeker> GetJobSeeker(int id)
        {
            var jobseeker = await _JobSeekerRepo.FindByIdAsync(id);
            return jobseeker;
        }

        //BranchOfficeTable------------------------------------------------------------------------------------------------------

        [HttpGet("AllBranch")]
        public async Task<ActionResult<IEnumerable<BranchOffice>>> ShowAll()
        {
            var branchoffice = await _BranchOfficeRepo.GetAllAsync();

            return Ok(branchoffice);
        }

        [HttpGet("BranchDetails/{id}")]
        public async Task<BranchOffice> GetBranch(int id)
        {
            var branch = await _BranchOfficeRepo.FindByIdAsync(id);
            return branch;
        }

        //UserTable----------------------------------------------------------------------------------------------------------------

        [HttpGet("UserDetails/{id}")]
        public async Task<User> GetUser()
        {
            var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
            //int Empid = _eRepo.GetEmpID(userIdCookie);
            var user = _userRepo.FindByIdAsync(userIdCookie);

            return await user;
        }

        //[HttpGet("MyProfile")]
        //public async Task<ActionResult<User>> GetMyProfile()
        //{
        //    int authenticatedUserId = int.Parse(User.FindFirst("UserID").Value);
        //    var user = await _userRepo.FindByIdAsync(authenticatedUserId);
        //    if (user == null || user.Role != "Employer")
        //    {
        //        return NotFound("Employer not found or unauthorized access.");
        //    }
        //    return Ok(user);
        //}

        //[HttpDelete("DeleteMyAccount")]
        //public async Task<ActionResult> DeleteMyAccount()
        //{
        //    int authenticatedUserId = int.Parse(User.FindFirst("UserID").Value);
        //    var success = await _userRepo.DeleteUserAsync(authenticatedUserId);
        //    if (!success)
        //    {
        //        return NotFound("Employer not found or already deleted.");
        //    }
        //    return NoContent();
        //}


        //UpdateInfo Function
        //if (existingEmployer == null || existingEmployer.EmployerID != id)
        //{
        //    return Forbid("You are not authorized to update this employer's details.");
        //}
        //var success = await _repo.UpdateEmployerAsync(id, emp);
        //if (!success)
        //{
        //    return NotFound("Employer not found or user does not exist.");
        //}
        //return NoContent();


        //DeleteAccount Function
        //int authenticatedEmployerId = int.Parse(User.FindFirst("EmployerID").Value);
        //var employer = await _repo.FindByIdAsync(authenticatedEmployerId);
        //if (employer == null)
        //{
        //    return NotFound("Employer not found or already deleted.");
        //}
        //var success = await _repo.DeleteEmployerAsync(authenticatedEmployerId);
        //if (!success)
        //{
        //    return BadRequest("Unable to delete employer account.");
        //}
        //return NoContent();

        //GetApplications
        //int authenticatedEmployerId = int.Parse(User.FindFirst("EmployerID").Value);
        //var applications = await _applicationRepo.GetAllAsync(a => a.Job.EmployerID == authenticatedEmployerId);
        //return Ok(applications);

        //GetOneApplication
        //    int authenticatedEmployerId = int.Parse(User.FindFirst("EmployerID").Value);
        //    var application = await _applicationRepo.FindByIdAsync(id);
        //    if (application == null || application.Job.EmployerID != authenticatedEmployerId)
        //    {
        //        return NotFound("Application not found or access denied.");
        //    }
        //    return Ok(application);
        //}

        //DeleteApplication
        //[HttpDelete("DeleteApplication/{id}")]
        //public async Task<ActionResult> DeleteApplication(int id)
        //{
        //    int authenticatedEmployerId = int.Parse(User.FindFirst("EmployerID").Value);
        //    var application = await _applicationRepo.FindByIdAsync(id);
        //    if (application == null || application.Job.EmployerID != authenticatedEmployerId)
        //    {
        //        return NotFound("Application not found or access denied.");
        //    }
        //    var success = await _applicationRepo.DeleteApplicationAsync(id);
        //    if (!success)
        //    {
        //        return BadRequest("Failed to delete application.");
        //    }
        //    return NoContent();
        //}
    }
}