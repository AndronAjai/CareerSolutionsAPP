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
    [Route("api/[controller]")]
    [ApiController]
    public class Employer_JobSeekerController : ControllerBase
    {
        private readonly IEmployerRepo _repo;
        private readonly IApplicationRepo _applicationRepo;
        private readonly IJobsRepo _jobsRepo;
        private readonly IUserRepo _userRepo;
        private readonly IEmployerAreaRepo _eRepo;
        private readonly IJobSeekerRepo _JobSeekerRepo;
        private readonly IBranchOfficeRepo _BranchOfficeRepo;


        public Employer_JobSeekerController(IEmployerRepo repo, IApplicationRepo applicationRepo, IJobsRepo jobsRepo, IUserRepo userRepo, IEmployerAreaRepo empRepo, IJobSeekerRepo JobSeekerRepo, IBranchOfficeRepo BranchOfficeRepo)
        {
            _repo = repo;
            _applicationRepo = applicationRepo;
            _jobsRepo = jobsRepo;
            _userRepo = userRepo;
            _eRepo = empRepo;
            _JobSeekerRepo = JobSeekerRepo;
            _BranchOfficeRepo = BranchOfficeRepo;
        }

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

        [HttpGet("View-Js-resume/{id}")]
        public IActionResult ViewResume(int id)
        {
            var jobSeeker = _JobSeekerRepo.FindById(id);

            if (jobSeeker == null || string.IsNullOrEmpty(jobSeeker.ResumePath))
            {
                return NotFound();
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), jobSeeker.ResumePath);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var fileExtension = Path.GetExtension(filePath);

            var mimeType = fileExtension switch
            {
                ".pdf" => "application/pdf",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                _ => "application/octet-stream",
            };

            return File(fileBytes, mimeType, $"Resume_{id}{fileExtension}");
        }

    }
}
