using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CSAPI.Areas.EmployerArea.Models;
using CSAPI.Models;

namespace CSAPI.Areas.EmployerArea.Controllers
{
    [Area("EmployerArea")]
    [Route("api/[controller]/")]
    [ApiController]
    [Authorize(Roles = "Employer")]
    public class Employer_AccountController : ControllerBase
    {
        private readonly IEmployerRepo _repo;
        private readonly IEmployerAreaRepo _eRepo;

        public Employer_AccountController(IEmployerRepo repo, IEmployerAreaRepo empRepo)
        {
            _repo = repo; 
            _eRepo = empRepo;
        }

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
    }
}
