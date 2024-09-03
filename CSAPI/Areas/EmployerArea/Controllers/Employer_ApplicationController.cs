using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using CSAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using CSAPI.Areas.EmployerArea.Models;

namespace CSAPI.Areas.EmployerArea.Controllers
{
    [Area("EmployerArea")]
    [Route("api/[controller]/")]
    [ApiController]
    [Authorize(Roles = "Employer")]
    public class Employer_ApplicationController : ControllerBase
    {
        private readonly IEmployerRepo _repo;
        private readonly IApplicationRepo _applicationRepo;
        private readonly IEmployerAreaRepo _eRepo;
        private readonly IJobsRepo _jobsRepo;
        private readonly IJobStatusNotificationRepo _jsnrepo;

        public Employer_ApplicationController(IEmployerRepo repo, IApplicationRepo applicationRepo,IEmployerAreaRepo empRepo, IJobsRepo jobsRepo, IJobStatusNotificationRepo jsnrepo)
        {
            _repo = repo;
            _applicationRepo = applicationRepo;
            _eRepo = empRepo;
            _jobsRepo = jobsRepo;
            _jsnrepo = jsnrepo;
        }

        [HttpGet("AllApplications")]
        public async Task<IQueryable<JobApplication>> ShowAllApplications()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            bool x = int.TryParse(userIdClaim, out var userIdCookie);
            int Empid = _eRepo.GetEmpID(userIdCookie);
            return await _eRepo.GetMyApplications(Empid);
        }

        [HttpGet("ApplicationDetailsByApplicationId/{id}")]
        public async Task<JobApplication> GetApplication(int id)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            bool x = int.TryParse(userIdClaim, out var userIdCookie);
            int Empid = _eRepo.GetEmpID(userIdCookie);
            var app = _eRepo.GetApplication(id);

            return await app;
        }

        [HttpGet("AllApplicationsForJobByJobID/{id}")]
        public async Task<IQueryable<JobApplication>> ShowApplicationsForJob(int id)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            bool x = int.TryParse(userIdClaim, out var userIdCookie);
            int Empid = _eRepo.GetEmpID(userIdCookie);
            var job = await _jobsRepo.FindByIdAsync(id);
            if (job != null && job.EmployerID == Empid)
            {
                return await _eRepo.GetApplicationsOfJob(id);
            }
            else
                return null;

        }

        [HttpPost("AcceptApplication")]
        public async Task<IActionResult> AcceptApplication(int applicationId)
        {
            JobApplication appln = await _applicationRepo.FindByIdAsync(applicationId);
            await _eRepo.SelectApplicationsForJobAsync(appln.JobID, applicationId);
            return Ok("Application processed successfully.");
        }

    }


}

