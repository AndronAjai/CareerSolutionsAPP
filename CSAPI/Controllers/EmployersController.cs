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
    public class EmployersController : ControllerBase
    {
        IEmployerRepo _Repo;

        public EmployersController(IEmployerRepo repo)
        {
            _Repo = repo;
        }
        // GET: api/<ApplicationController>
        [HttpGet]
        public List<Employer> ShowAll()
        {
            return _Repo.GetAll();
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public Employer FindEmployer(int id)
        {
            return _Repo.FindById(id);
        }

        // POST api/<JobSeekerController>
        [HttpPost]
        public HttpStatusCode Post([FromBody] Employer emp)
        {
            _Repo.AddEmployer(emp);
            return HttpStatusCode.Created;
        }

        // PUT api/<JobSeekerController>/5
        [HttpPut("{id}")]
        public HttpStatusCode Put(int id, [FromBody] Employer emp)
        {
            _Repo.UpdateEmployer(id, emp);
            return HttpStatusCode.NoContent;
        }

        // DELETE api/<JobSeekerController>/5
        [HttpDelete("{id}")]
        public HttpStatusCode Delete(int id)
        {
            _Repo.DeleteEmployer(id);
            return HttpStatusCode.NoContent;
        }
        //private bool EmployersExists(int id)
        //{
        //    return _context.Employers.Any(user => user.EmployerID == id);
        //}
    }
}