using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CSAPI.Models;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace CSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        IJobSeekerRepo _JSrepo;
        IEmployerRepo _IEmployerRepo;

        public RegistrationController(IJobSeekerRepo js, IEmployerRepo emp)
        {
            _IEmployerRepo = emp;
            _JSrepo = js;
        }

        // POST: api/JobSeeker
        [HttpPost("RegisterJobSeeker")]
        [AllowAnonymous]
        public async Task<ActionResult> Post([FromBody] JobSeeker js)
        {
            var success = await _JSrepo.AddJobSeekerAsync(js);
            if (!success)
            {
                return BadRequest("Invalid data or user does not exist.");
            }
            return StatusCode((int)HttpStatusCode.Created);
        }

        // POST: api/Employers
        [HttpPost("RegisterEmployer")]
        public async Task<ActionResult> Post([FromBody] Employer emp)
        {
            await _IEmployerRepo.AddEmployerAsync(emp);
            return StatusCode((int)HttpStatusCode.Created);
        }
    }
}
