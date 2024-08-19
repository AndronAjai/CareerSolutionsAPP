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
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _Repo;

        public UserController(IUserRepo repo)
        {
            _Repo = repo;
        }

        [HttpGet]
        [AllowAnonymous]
        public List<User> ShowAll()
        {
            return _Repo.GetAll();
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public User FindUser(int id)
        {
            return _Repo.FindById(id);
        }

        // POST api/<UserController>
        [HttpPost]
        [AllowAnonymous]
        public HttpStatusCode Post([FromBody] User us)
        {
            _Repo.AddUser(us);
            return HttpStatusCode.Created;
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        [AllowAnonymous]
        public HttpStatusCode Put(int id, [FromBody] User us)
        {
            _Repo.UpdateUser(id, us);
            return HttpStatusCode.NoContent;
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        [AllowAnonymous]
        public HttpStatusCode Delete(int id)
        {
            _Repo.DeleteUser(id);
            return HttpStatusCode.NoContent;
        }
    }
}