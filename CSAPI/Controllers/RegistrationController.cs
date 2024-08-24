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

        [HttpPost("RegisterJobSeeker")]
        [AllowAnonymous]
        public async Task<ActionResult> Post([FromBody] JobSeeker js)
        {
            // Retrieve the UserId from the cookie
            if (Request.Cookies.TryGetValue("UId", out var userIdString))
            {
                // Parse the string to an integer
                if (int.TryParse(userIdString, out int userId))
                {
                    // Use the UserId as needed, for example, associating it with the JobSeeker
                    js.UserID = userId;

                    var success = await _JSrepo.AddJobSeekerAsync(js);
                    if (!success)
                    {
                        return BadRequest("Invalid data or user does not exist.");
                    }

                    return StatusCode((int)HttpStatusCode.Created);
                }
                else
                {
                    return BadRequest("Invalid UserId in cookie.");
                }
            }

            return BadRequest("User is not authenticated.");
        }

        [HttpPost("RegisterEmployer")]
        [AllowAnonymous]
        public async Task<ActionResult> Post([FromBody] Employer emp)
        {
            // Retrieve the UserId from the cookie
            if (Request.Cookies.TryGetValue("UId", out var userIdString))
            {
                // Parse the string to an integer
                if (int.TryParse(userIdString, out int userId))
                {
                    // Use the UserId as needed, for example, associating it with the Employer
                    emp.UserID = userId;

                    await _IEmployerRepo.AddEmployerAsync(emp);
                    return StatusCode((int)HttpStatusCode.Created);
                }
                else
                {
                    return BadRequest("Invalid UserId in cookie.");
                }
            }

            return BadRequest("User is not authenticated.");
        }


    }
} 
