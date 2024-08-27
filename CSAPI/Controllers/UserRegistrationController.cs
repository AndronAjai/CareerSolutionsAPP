using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CSAPI.Models;
using System.Net;
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
        public async Task<ActionResult> Register([FromBody] User User)
        {
            if (User == null)
            {
                return BadRequest("Invalid user registration request!");
            }
            

            // Hashing before storing
            User.Password = BCrypt.Net.BCrypt.HashPassword(User.Password);
            await _repo.AddUserAsync(User);
            Response.Cookies.Append("UsID", User.UserID.ToString(), new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });
            return CreatedAtAction(nameof(User), new { id = User.UserID }, User);

        }

        
    }
}