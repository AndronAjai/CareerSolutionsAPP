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

        public Employer_ApplicationController(IEmployerRepo repo, IApplicationRepo applicationRepo,IEmployerAreaRepo empRepo, IJobsRepo jobsRepo)
        {
            _repo = repo;
            _applicationRepo = applicationRepo;
            _eRepo = empRepo;
            _jobsRepo = jobsRepo;
        }

        [HttpGet("AllApplications")]
        public async Task<IQueryable<JobApplication>> ShowAllApplications()
        {
            //var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
            var userIdClaim = User.FindFirst("UserId")?.Value;
            bool x = int.TryParse(userIdClaim, out var userIdCookie);
            int Empid = _eRepo.GetEmpID(userIdCookie);
            return await _eRepo.GetMyApplications(Empid);
        }

        [HttpGet("ApplicationDetails/{id}")]
        public async Task<JobApplication> GetApplication(int id)
        {
            //var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
            var userIdClaim = User.FindFirst("UserId")?.Value;
            bool x = int.TryParse(userIdClaim, out var userIdCookie);
            int Empid = _eRepo.GetEmpID(userIdCookie);
            var app = _eRepo.GetApplication(id);

            return await app;
        }

        [HttpGet("AllApplications/{id}")]
        public async Task<IQueryable<JobApplication>> ShowApplicationsForJob(int id)
        {
            //var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
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
    }
}
