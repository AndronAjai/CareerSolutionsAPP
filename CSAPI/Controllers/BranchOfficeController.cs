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
    public class BranchOfficeController : ControllerBase
    {
        private readonly CareerSolutionsDB _context;

        public BranchOfficeController(CareerSolutionsDB context)
        {
            _context = context;
        }

        // GET api/BranchOffice/5
        [HttpGet("{id}")]
        public BranchOffice GetBranchOffice(int id)
        {
            return _context.BranchOffices.Find(id);
        }

        // POST api/BranchOffice
        [HttpPost]
        public HttpStatusCode Post([FromBody] BranchOffice branchOffice)
        {
            _context.BranchOffices.Add(branchOffice);
            _context.SaveChanges();
            return HttpStatusCode.Created;
        }

        // PUT api/BranchOffice/5
        [HttpPut("{id}")]
        public HttpStatusCode Put(int id, [FromBody] BranchOffice branchOffice)
        {
            if (id != branchOffice.BranchOfficeID)
            {
                return HttpStatusCode.BadRequest;
            }

            _context.Entry(branchOffice).State = EntityState.Modified;
            _context.SaveChanges();
            return HttpStatusCode.NoContent;
        }

        // DELETE api/BranchOffice/5
        [HttpDelete("{id}")]
        public HttpStatusCode Delete(int id)
        {
            var branchOffice = _context.BranchOffices.Find(id);
            if (branchOffice == null)
            {
                return HttpStatusCode.NotFound;
            }

            _context.BranchOffices.Remove(branchOffice);
            _context.SaveChanges();
            return HttpStatusCode.NoContent;
        }

        private bool BranchOfficeExists(int id)
        {
            return _context.BranchOffices.Any(bo => bo.BranchOfficeID == id);
        }
    }
}