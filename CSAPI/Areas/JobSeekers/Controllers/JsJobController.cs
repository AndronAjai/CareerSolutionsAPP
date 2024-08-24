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

        [HttpGet("goat")]
        public async Task<ActionResult<IEnumerable<Job>>> DisplayRecommendedJobs()
            {
            var useridCookie = Convert.ToInt32(Request.Cookies["UserId"]);
            var x = await _AjbRepo.GetJobs(useridCookie);
            return Ok(x);
            }
        }
    }
