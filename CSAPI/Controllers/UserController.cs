using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CSAPI.Models;
using System.Net;

namespace CSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly CareerSolutionsDB _context;

        public UsersController(CareerSolutionsDB context)
        {
            _context = context;
        }

        // GET api/Users/5
        [HttpGet("{id}")]
        public User GetUser(int id)
        {
            return _context.Users.Find(id);
        }

        // POST api/Users
        [HttpPost]
        public HttpStatusCode Post([FromBody] User user)
        {
            if (_context.Users.Any(u => u.Username == user.Username || u.Email == user.Email))
            {
                return HttpStatusCode.Conflict;
            }

            _context.Users.Add(user);
            _context.SaveChanges();
            return HttpStatusCode.Created;
        }

        // PUT api/Users/5
        [HttpPut("{id}")]
        public HttpStatusCode Put(int id, [FromBody] User user)
        {
            if (id != user.UserID)
            {
                return HttpStatusCode.BadRequest;
            }

            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
            return HttpStatusCode.NoContent;
        }

        // DELETE api/Users/5
        [HttpDelete("{id}")]
        public HttpStatusCode Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return HttpStatusCode.NotFound;
            }

            _context.Users.Remove(user);
            _context.SaveChanges();
            return HttpStatusCode.NoContent;
        }

        private bool UsersExists(int id)
        {
            return _context.Users.Any(user => user.UserID == id);
        }
    }
}