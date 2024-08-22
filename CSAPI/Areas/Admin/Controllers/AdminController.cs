using CSAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CSAPI.Models;
using System.Net;

namespace CSAPI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        IBranchOfficeRepo _Repo;
        IUserRepo _UserRepo;
        IEmployerRepo _EmployerRepo;
        IJobSeekerRepo _JobSeekerRepo;
        IJobsRepo _JobsRepo;
        IApplicationRepo _ApplicationRepo;

        public AdminController(IBranchOfficeRepo repo, IUserRepo repo1, IEmployerRepo repo2, IJobSeekerRepo repo3,  IJobsRepo repo4, IApplicationRepo repo5)
        {
            _Repo = repo;
            _UserRepo = repo1;
            _EmployerRepo = repo2;
            _JobSeekerRepo = repo3;
            _JobsRepo = repo4;
            _ApplicationRepo = repo5;
            
        }

        //Branch Table--------------------------------------------------------------

        [HttpGet("AllBranches")]
        //[AllowAnonymous]
        public async Task<ActionResult<IEnumerable<BranchOffice>>> ShowAllBranches()
        {
            var branch = await _Repo.GetAllAsync();
            return Ok(branch);
        }

       
        [HttpGet("BranchDetails/{id}")]
        public async Task<ActionResult<BranchOffice>> GetBranchOffice(int id)
        {
            var branchOffice = await _Repo.FindByIdAsync(id);
            if (branchOffice == null)
            {
                return NotFound();
            }
            return branchOffice;
        }

        // POST api/BranchOffice
        [HttpPost("AddBranch")]
        public async Task<HttpStatusCode> Post([FromBody] BranchOffice branchOffice)
        {
            await _Repo.AddBranchOfficesAsync(branchOffice);
            return HttpStatusCode.Created;
        }

        // PUT api/BranchOffice/5
        [HttpPut("UpdateBranch/{id}")]
        public async Task<HttpStatusCode> Put(int id, [FromBody] BranchOffice branchOffice)
        {
            await _Repo.UpdateBranchOfficesAsync(id, branchOffice);
            return HttpStatusCode.NoContent;
        }

        [HttpDelete("DeleteBranch/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> DeleteBranch(int id)
        {
            var branch = await _Repo.FindByIdAsync(id);
            if (branch == null)
            {
                return NotFound();
            }

            var success = await _Repo.DeleteBranchOfficesAsync(id);

            if (!success)
            {
                return BadRequest("Could not delete the branch office.");
            }

            return NoContent();
        }

        //Users Table---------------------------------------------------------------

        [HttpGet("AllUsers")]
        //[AllowAnonymous]
        public async Task<ActionResult<IEnumerable<User>>> ShowAll()
        {
            var users = await _UserRepo.GetAllAsync();
            return Ok(users);
        }

        // GET: api/User/5
        [HttpGet("UserDetails/{id}")]
        //[AllowAnonymous]
        public async Task<ActionResult<User>> FindUser(int id)
        {
            var user = await _UserRepo.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpDelete("DeleteUser/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var user = await _UserRepo.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var success = await _UserRepo.DeleteUserAsync(id);

            if (!success)
            {
                return BadRequest("Could not delete the user.");
            }

            return NoContent();
        }

        //Employer Table-----------------------------------------------------------

        [HttpGet("AllEmployers")]
        //[AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Employer>>> ShowAllEmployer()
        {
            var employers = await _EmployerRepo.GetAllAsync();
            return Ok(employers);
        }

        // GET: api/User/5
        [HttpGet("EmployerDetails/{id}")]
        //[AllowAnonymous]
        public async Task<ActionResult<Employer>> FindEmployer(int id)
        {
            var employer = await _EmployerRepo.FindByIdAsync(id);
            if (employer == null)
            {
                return NotFound();
            }
            return Ok(employer);
        }

        [HttpDelete("DeleteEmployer/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> DeleteEmployer(int id)
        {
            var employer = await _EmployerRepo.FindByIdAsync(id);
            if (employer == null)
            {
                return NotFound();
            }

            var success = await _EmployerRepo.DeleteEmployerAsync(id);

            if (!success)
            {
                return BadRequest("Could not delete the employer.");
            }

            return NoContent();
        }

        //Job Seeker Table-------------------------------------------------------------

        [HttpGet("AllJobSeekers")]
        //[AllowAnonymous]
        public async Task<ActionResult<IEnumerable<JobSeeker>>> ShowAllJobSeeker()
        {
            var js = await _JobSeekerRepo.GetAllAsync();
            return Ok(js);
        }

        // GET: api/User/5
        [HttpGet("JobSeekerDetails/{id}")]
        //[AllowAnonymous]
        public async Task<ActionResult<JobSeeker>> FindJobSeeker(int id)
        {
            var jobSeeker = await _JobSeekerRepo.FindByIdAsync(id);
            if (jobSeeker == null)
            {
                return NotFound();
            }
            return Ok(jobSeeker);
        }

        [HttpDelete("DeleteJobSeeker/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> DeleteJobSeeker(int id)
        {
            var js = await _JobSeekerRepo.FindByIdAsync(id);
            if (js == null)
            {
                return NotFound();
            }

            var success = await _JobSeekerRepo.DeleteJobSeekerAsync(id);

            if (!success)
            {
                return BadRequest("Could not delete the job seeker.");
            }

            return NoContent();
        }

        //Job Table-----------------------------------------------------------------

        [HttpGet("AllJobs")]
        //[AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Job>>> ShowAllJob()
        {
            var j = await _JobsRepo.GetAllAsync();
            return Ok(j);
        }

        // GET: api/User/5
        [HttpGet("JobDetails/{id}")]
        //[AllowAnonymous]
        public async Task<ActionResult<Job>> FindJob(int id)
        {
            var job = await _JobsRepo.FindByIdAsync(id);
            if (job == null)
            {
                return NotFound();
            }
            return Ok(job);
        }

        [HttpDelete("DeleteJob/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> DeleteJob(int id)
        {
            var j = await _JobsRepo.FindByIdAsync(id);
            if (j == null)
            {
                return NotFound();
            }

            var success = await _JobsRepo.DeleteJobAsync(id);

            if (!success)
            {
                return BadRequest("Could not delete the job seeker.");
            }

            return NoContent();
        }

        //Application Table------------------------------------------------------------


        [HttpGet("AllApplications")]
        //[AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Application>>> ShowAllApplications()
        {
            var app = await _ApplicationRepo.GetAllAsync();
            return Ok(app);
        }

        // GET: api/User/5
        [HttpGet("ApplicationDetails/{id}")]
        //[AllowAnonymous]
        public async Task<ActionResult<Application>> FindApplication(int id)
        {
            var application = await _ApplicationRepo.FindByIdAsync(id);
            if (application == null)
            {
                return NotFound();
            }
            return Ok(application);
        }

        [HttpDelete("DeleteApplication/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> DeleteApplication(int id)
        {
            var application = await _ApplicationRepo.FindByIdAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            var success = await _ApplicationRepo.DeleteApplicationAsync(id);

            if (!success)
            {
                return BadRequest("Could not delete the job seeker.");
            }

            return NoContent();

        }

    }
}
