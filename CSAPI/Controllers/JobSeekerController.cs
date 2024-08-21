using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CSAPI.Models;
using CSAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace CareerSolutionWebApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class JobSeekerController : ControllerBase
    {
        private readonly IJobSeekerRepo _repo;

        public JobSeekerController(IJobSeekerRepo repo)
        {
            _repo = repo;
        }

        // GET: api/JobSeeker
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobSeeker>>> ShowAll()
        {
            var jobSeekers = await _repo.GetAllAsync();
            return Ok(jobSeekers);
        }

        // GET: api/JobSeeker/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JobSeeker>> FindJobSeeker(int id)
        {
            var jobSeeker = await _repo.FindByIdAsync(id);
            if (jobSeeker == null)
            {
                return NotFound();
            }
            return Ok(jobSeeker);
        }

        // POST: api/JobSeeker
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] JobSeeker js)
        {
            var success = await _repo.AddJobSeekerAsync(js);
            if (!success)
            {
                return BadRequest("Invalid data or user does not exist.");
            }
            return StatusCode((int)HttpStatusCode.Created);
        }

        // PUT: api/JobSeeker/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] JobSeeker js)
        {
            var success = await _repo.UpdateJobSeekerAsync(id, js);
            if (!success)
            {
                return NotFound("JobSeeker not found or user does not exist.");
            }
            return NoContent();
        }

        // DELETE: api/JobSeeker/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _repo.DeleteJobSeekerAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
