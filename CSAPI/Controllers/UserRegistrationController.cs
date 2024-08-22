using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        // POST: api/User
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Post([FromBody] User us)
        {
            var success = await _repo.AddUserAsync(us);
            if (!success)
            {
                return BadRequest("Invalid BranchOfficeID.");
            }
            return StatusCode((int)HttpStatusCode.Created);
        }

        // GET: api/User
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<User>>> ShowAll()
        {
            var users = await _repo.GetAllAsync();
            return Ok(users);
        }


        //// GET: api/User/5
        //[HttpGet("{id}")]
        //[AllowAnonymous]
        //public async Task<ActionResult<User>> FindUser(int id)
        //{
        //    var user = await _repo.FindByIdAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(user);
        //}

        //// PUT: api/User/5
        //[HttpPut("{id}")]
        //[AllowAnonymous]
        //public async Task<ActionResult> Put(int id, [FromBody] User us)
        //{
        //    var user = await _repo.FindByIdAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    var success = await _repo.UpdateUserAsync(id, us);
        //    if (!success)
        //    {
        //        return BadRequest("Invalid BranchOfficeID.");
        //    }

        //    return NoContent();
        //}

        //// DELETE: api/User/5
        //[HttpDelete("{id}")]
        //[AllowAnonymous]
        //public async Task<ActionResult> Delete(int id)
        //{
        //    var user = await _repo.FindByIdAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    var success = await _repo.DeleteUserAsync(id);
        //    if (!success)
        //    {
        //        return BadRequest("Could not delete the user.");
        //    }

        //    return NoContent();
        //}
    }
}
