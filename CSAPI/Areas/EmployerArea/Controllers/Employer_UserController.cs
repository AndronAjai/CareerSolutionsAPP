using Microsoft.AspNetCore.Http;
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
    public class Employer_UserController : ControllerBase
    {
        private readonly IUserRepo _userRepo;

        public Employer_UserController( IUserRepo userRepo)
        {
            _userRepo = userRepo;           
        }

        [HttpGet("UserDetails")]
        public async Task<User> GetUser()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            bool x = int.TryParse(userIdClaim, out var userIdCookie);
            
            var user = _userRepo.FindByIdAsync(userIdCookie);

            return await user;
        }
    }
}
