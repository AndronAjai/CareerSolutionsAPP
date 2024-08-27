using CSAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CSAPI.Areas.JobSeekers.Controllers
    {
    [Area("JobSeekers")]
    [Route("api/jobseekers/[controller]")]
    [ApiController]
    [Authorize(Roles = "JobSeeker")]
    public class JsJobController : ControllerBase
        {
        IJobsRepo _AjbRepo;

        public JsJobController(IJobsRepo AjbRepo)
            {
            _AjbRepo = AjbRepo;
            }

        [HttpGet("ViewJobs")]
        public async Task<ActionResult<IEnumerable<Job>>> DisplayJobs()
            {
            List<Job> jsviewjobs = await _AjbRepo.GetAllAsync();
            return Ok(jsviewjobs);
            }

        [HttpGet("ClickedJobs/{jobid}")]
        public async Task<IActionResult> ClickedJob(int jobid)
            {
            return RedirectToAction("EmpInfo", "JsEmployer", new { paramname = jobid });
            }
        
        [HttpGet("RecommendJobs")]
        public async Task<ActionResult<IEnumerable<Job>>> DisplayRecommendedJobs()
            {
            //var useridCookie = Convert.ToInt32(Request.Cookies["UserId"]);
            var userIdClaim = User.FindFirst("UserId")?.Value;
            bool y = int.TryParse(userIdClaim, out var userIdCookie);
            var x = await _AjbRepo.GetJobs(userIdCookie);
            return Ok(x);
            }

        [HttpPost("Filterjobs/industry/{industry}")]
        public async Task<ActionResult<IEnumerable<Job>>> FilterByIndustry(string industry)
            {
            var filtery = await _AjbRepo.filterbyind(industry);
            return filtery;
            }

        [HttpPost("Filterjobs/specialization/{specialization}")]
        public async Task<ActionResult<IEnumerable<Job>>> FilterByspecial(string specialization)
            {
            var filterz = await _AjbRepo.filterbyspec(specialization);
            return filterz;
            }
        }
    }
