using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CSAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace CSAPI.Controllers
{
    [Area("Employer")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Employer")]
    public class EmployersController : ControllerBase
    {
        private readonly IEmployerRepo _employerRepo;
        private readonly IApplicationRepo _applicationRepo;

        public EmployersController(IEmployerRepo employerRepo, IApplicationRepo applicationRepo)
        {
            _employerRepo = employerRepo;
            _applicationRepo = applicationRepo;
        }

        // GET: api/Employers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployersController>>> ShowAll()
        {
            var employers = await _employerRepo.GetAllAsync();
            return Ok(employers);
        }

        // GET: api/Employers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployersController>> FindEmployer(int id)
        {
            var employer = await _employerRepo.FindByIdAsync(id);
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
            await _employerRepo.AddEmployerAsync(emp);
            return StatusCode((int)HttpStatusCode.Created);
        }

        // PUT: api/Employers/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Employer emp)
        {
            var success = await _employerRepo.UpdateEmployerAsync(id, emp);
            if (!success)
            {
                return NotFound("Employer not found or user does not exist.");
            }
            return NoContent();
        }

        // DELETE: api/Employers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployer(int id)
        {
            var success = await _employerRepo.DeleteEmployerAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        // DELETE: api/Employers/Applications/5
        [HttpDelete("DeleteApplication/{id}")]
        public async Task<ActionResult> DeleteApplication(int id)
        {
            await _applicationRepo.DeleteApplicationAsync(id);
            return NoContent();
        }
    }
}
