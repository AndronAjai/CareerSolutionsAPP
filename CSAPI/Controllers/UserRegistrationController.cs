using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CSAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace CSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRegistrationController : ControllerBase
    {
        private readonly IUserRepo _repo;

        public UserRegistrationController(IUserRepo repo)
        {
            _repo = repo;
        }

        // POST: api/User/Register
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("Invalid user registration request!");
            }

            user.RegistrationDate = DateTime.Now;

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            await _repo.AddUserAsync(user);

            Response.Cookies.Append("UsID", user.UserID.ToString(), new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            return CreatedAtAction(nameof(Register), new { id = user.UserID }, user);
        }
    }
}
