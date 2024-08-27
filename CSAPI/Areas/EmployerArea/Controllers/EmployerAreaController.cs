
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
    public class EmployerAreaController : ControllerBase
    {
        private readonly IEmployerRepo _repo;
        private readonly IApplicationRepo _applicationRepo;
        private readonly IJobsRepo _jobsRepo;
        private readonly IUserRepo _userRepo;
        private readonly IEmployerAreaRepo _eRepo;
        private readonly IJobSeekerRepo _JobSeekerRepo;
        private readonly IBranchOfficeRepo _BranchOfficeRepo;


        public EmployerAreaController(IEmployerRepo repo, IApplicationRepo applicationRepo, IJobsRepo jobsRepo, IUserRepo userRepo, IEmployerAreaRepo empRepo, IJobSeekerRepo JobSeekerRepo, IBranchOfficeRepo BranchOfficeRepo)
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
            //var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
            var userIdClaim = User.FindFirst("UserId")?.Value;
            bool x = int.TryParse(userIdClaim, out var userIdCookie);
            int Empid = _eRepo.GetEmpID(userIdCookie);
            Employer e = await _repo.FindByIdAsync(Empid);

            if (e == null)
            {
                return NotFound("Employer not found.");
            }
            return Ok(e);
        }

        [HttpGet("view-js-resume/{id}")]
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

        [HttpPut("UpdateInfo")]
        public async Task<bool> UpdateInfo([FromBody] Employer updatedEmployer)
        {
            //var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
            var userIdClaim = User.FindFirst("UserId")?.Value;
            bool x = int.TryParse(userIdClaim, out var userIdCookie);
            int Empid = _eRepo.GetEmpID(userIdCookie);
            Employer e = await _repo.FindByIdAsync(Empid);
            return await _repo.UpdateEmployerAsync(Empid, e);
        }

        [HttpDelete("DeleteAccount")]
        public async Task<bool> DeleteAccount()
        {
            //var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
            var userIdClaim = User.FindFirst("UserId")?.Value;
            bool x = int.TryParse(userIdClaim, out var userIdCookie);
            int Empid = _eRepo.GetEmpID(userIdCookie);
            Employer e = await _repo.FindByIdAsync(Empid);
            return await _repo.DeleteEmployerAsync(Empid);
        }

        //ApplicationTable---------------------------------------------------------------------------------------------------------------

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

        //JobTable-----------------------------------------------------------------------------------------------------------------------

        [HttpGet("AllJobs")]
        public async Task<IQueryable<Job>> ShowAllJobs()
        {
            //var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
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
            //var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
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
            //var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
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
            //var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
            var userIdClaim = User.FindFirst("UserId")?.Value;
            bool x = int.TryParse(userIdClaim, out var userIdCookie);
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
            //var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
            var userIdClaim = User.FindFirst("UserId")?.Value;
            bool x = int.TryParse(userIdClaim, out var userIdCookie);
            //int Empid = _eRepo.GetEmpID(userIdCookie);
            var user = _userRepo.FindByIdAsync(userIdCookie);

            return await user;
        }
       
        //Reccomendation------------------------------------------------------------------------------------------------------------

        [HttpGet("RecommendationForEachJob")]
        public async Task<List<JobSeeker>> GetRecommendations(int jobid)
        {
            List<JobSeeker> recommendedList = new List<JobSeeker>();
            Tuple<JobSeeker, int> recommendedrow;
            List<Tuple<JobSeeker, int>> recommendedListCondition = new List<Tuple<JobSeeker, int>>();
            List<JobSeeker> IndustryRec = new List<JobSeeker>();
            List<JobSeeker> SpecialRec = new List<JobSeeker>();
            List<JobSeeker> SkillRec = new List<JobSeeker>();
            List<Tuple<JobSeeker, int>> PrefSkillRec = new List<Tuple<JobSeeker, int>>();
            int pref = 0;

            //var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
            var userIdClaim = User.FindFirst("UserId")?.Value;
            bool x = int.TryParse(userIdClaim, out var userIdCookie);
            int Empid = _eRepo.GetEmpID(userIdCookie);

            var job = await _jobsRepo.FindByIdAsync(jobid);
            if (job != null && job.EmployerID == Empid)
            {
                IndustryRec = await _eRepo.GetRecommendationByIndustry(jobid);
                SpecialRec = await _eRepo.GetRecommendationBySpecialization(jobid);
                PrefSkillRec = await _eRepo.GetRecommendationBySkills(jobid);
                SkillRec = PrefSkillRec.Select(x => x.Item1).ToList();

                foreach (var i in SkillRec)
                {
                    pref = 0;
                    if (IndustryRec.Contains(i) && SpecialRec.Contains(i))
                        pref = 1;
                    else if (IndustryRec.Contains(i) || SpecialRec.Contains(i))
                        pref = 2;
                    else pref = 3;
                    recommendedrow = new Tuple<JobSeeker, int>(i, pref);
                    recommendedListCondition.Add(recommendedrow);
                }
                foreach (var i in SpecialRec)
                {
                    if (IndustryRec.Contains(i))
                        pref = 2;
                    else pref = 3;
                    if(!SkillRec.Contains(i))
                    {
                        recommendedrow = new Tuple<JobSeeker, int>(i, pref);
                        recommendedListCondition.Add(recommendedrow);
                    }
                }
                foreach (var i in IndustryRec)
                {
                    if(!SkillRec.Contains(i) && !SpecialRec.Contains(i))
                    {
                        pref = 3;
                        recommendedrow = new Tuple<JobSeeker, int>(i, pref);
                        recommendedListCondition.Add(recommendedrow);
                    }
                }

                recommendedList = recommendedListCondition.Select(x => x.Item1).ToList();
                return await Task.FromResult(recommendedList);
            }
            else
                return null;
        }

    }
}