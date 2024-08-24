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

            var userId = us.UserID;

            // Store the UserId in a cookie
            Response.Cookies.Append("UId", userId.ToString(), new CookieOptions
            {
                HttpOnly = true,
                Secure = true,   
                SameSite = SameSiteMode.Strict 
            });

            return StatusCode((int)HttpStatusCode.Created);
        }

        // GET: api/User
        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<ActionResult<IEnumerable<User>>> ShowAll()
        //{
        //    var users = await _repo.GetAllAsync();
        //    return Ok(users);
        //}


            // Check if the user already exists in the database
            //if (await _repo.UserExistsAsync(newUser.Username))
            //{
            //    return Conflict("User already exists!");
            //}


            // Create a new user object
            //var user = new User
            //{
            //    Username = newUser.Username,
            //    Password = newUser.Password, // Store hashed password
            //    Email = newUser.Email,
            //    BranchOfficeID = newUser.BranchOfficeID,
            //    Role = newUser.Role
            //};

            // Save the new user to the database
            var success = await _repo.AddUserAsync(User);
            if (!success)
            {
                return BadRequest("Invalid BranchOfficeID.");
            }
            
            


            return StatusCode((int)HttpStatusCode.Created, "User registered successfully!");
        }

        // GET: api/User
        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<ActionResult<IEnumerable<User>>> ShowAll()
        //{
        //    var users = await _repo.GetAllAsync();
        //    return Ok(users);
        //}
    }
}