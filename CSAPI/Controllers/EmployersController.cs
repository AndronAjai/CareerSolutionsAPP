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
    public class EmployersController : Controller
    {
        private readonly CareerSolutionsDB _context;

        public Employer EmployerID { get; private set; }

        public EmployersController(CareerSolutionsDB context)
        {
            _context = context;
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public Employer AddEmployer(int id)
        {
            return _context.AddEmployer(id);
        }

        // POST api/<JobSeekerController>
        [HttpPost]
        public HttpStatusCode Post([FromBody] Employer emp)
        {
            _context.Add(emp);
            return HttpStatusCode.Created;
        }

        // PUT api/<JobSeekerController>/5
        [HttpPut("{id}")]
        public HttpStatusCode Put(int id, [FromBody] Employer emp)
        {
            _context.UpdateRange(id, emp);
            return HttpStatusCode.NoContent;
        }

        // DELETE api/<JobSeekerController>/5
        [HttpDelete("{id}")]
        public HttpStatusCode Delete(int id)
        {
            _context.DeleteEmployer(id);
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            _context.Employers.Remove(EmployerID);
            return HttpStatusCode.NoContent;
        }
        private bool EmployersExists(int id)
        {
            return _context.Employers.Any(user => user.EmployerID == id);
        }
    }
}