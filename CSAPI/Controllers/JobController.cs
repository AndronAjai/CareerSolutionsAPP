using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CSAPI.Models;
using Microsoft.AspNetCore.Authorization;
using DbCreationApp.Models;

namespace CareerSolutionWebApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class JobController : ControllerBase
    {
        private readonly IJobsRepo _repo;

        public JobController(IJobsRepo repo)
        {
            _repo = repo;
        }

        // GET: api/Job
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Job>>> ShowAll()
        {
            var jobs = await _repo.GetAllAsync();
            return Ok(jobs);
        }

        // GET: api/Job/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Job>> FindJob(int id)
        {
            var job = await _repo.FindByIdAsync(id);
            if (job == null)
            {
                return NotFound();
            }
            return Ok(job);
        }

        // POST: api/Job
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Job job)
        {
            var success = await _repo.AddJobAsync(job);
            if (!success)
            {
                return BadRequest("Invalid data or employer does not exist.");
            }
            return StatusCode((int)HttpStatusCode.Created);
        }

        // PUT: api/Job/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Job job)
        {
            var success = await _repo.UpdateJobAsync(id, job);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        // DELETE: api/Job/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _repo.DeleteJobAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
