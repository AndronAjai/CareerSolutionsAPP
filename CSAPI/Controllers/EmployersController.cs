using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CSAPI.Models;
using DbCreationApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace CSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class EmployersController : ControllerBase
    {
        private readonly IEmployerRepo _repo;

        public EmployersController(IEmployerRepo repo)
        {
            _repo = repo;
        }

        // GET: api/Employers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employer>>> ShowAll()
        {
            var employers = await _repo.GetAllAsync();
            return Ok(employers);
        }

        // GET: api/Employers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employer>> FindEmployer(int id)
        {
            var employer = await _repo.FindByIdAsync(id);
            if (employer == null)
            {
                return NotFound();
            }
            return Ok(employer);
        }

        // POST: api/Employers
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Employer emp)
        {
            var success = await _repo.AddEmployerAsync(emp);
            if (!success)
            {
                return BadRequest("Invalid data or user does not exist.");
            }
            return StatusCode((int)HttpStatusCode.Created);
        }

        // PUT: api/Employers/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Employer emp)
        {
            var success = await _repo.UpdateEmployerAsync(id, emp);
            if (!success)
            {
                return NotFound("Employer not found or user does not exist.");
            }
            return NoContent();
        }

        // DELETE: api/Employers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _repo.DeleteEmployerAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
