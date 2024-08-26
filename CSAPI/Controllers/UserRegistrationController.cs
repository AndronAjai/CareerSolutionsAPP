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
            return CreatedAtAction(nameof(User), new { id = User.UserID }, User);

            if (User == null)
            {
                return BadRequest("Invalid user registration request!");
            }

            var userId = User.UserID;

            // Store the UserId in a cookie
            Response.Cookies.Append("UId", userId.ToString(), new CookieOptions
            {
                HttpOnly = true,
                Secure = true,   
                SameSite = SameSiteMode.Strict 
            });

            return StatusCode((int)HttpStatusCode.Created);
        }

        
    }
}