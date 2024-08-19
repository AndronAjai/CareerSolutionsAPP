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
        IBranchOfficeRepo _Repo;

        public BranchOfficeController(IBranchOfficeRepo repo)
        {
            _Repo = repo;
        }

        // GET: api/<ApplicationController>
        [HttpGet]
        public List<BranchOffice> ShowAll()
        {
            return _Repo.GetAll();
        }

        // GET api/BranchOffice/5
        [HttpGet("{id}")]
        public BranchOffice GetBranchOffice(int id)
        {
            return _Repo.FindById(id);
        }

        // POST api/BranchOffice
        [HttpPost]
        public HttpStatusCode Post([FromBody] BranchOffice branchOffice)
        {
            _Repo.AddBranchOffices(branchOffice);
            return HttpStatusCode.Created;
        }

        // PUT api/BranchOffice/5
        [HttpPut("{id}")]
        public HttpStatusCode Put(int id, [FromBody] BranchOffice branchOffice)
        {
            _Repo.UpdateBranchOffices(id, branchOffice);
            return HttpStatusCode.NoContent;
        }

        // DELETE api/BranchOffice/5
        [HttpDelete("{id}")]
        public HttpStatusCode Delete(int id)
        {
            _Repo.DeleteBranchOffices(id);
            return HttpStatusCode.NoContent;
        }

        //private bool BranchOfficeExists(int id)
        //{
        //    return _context.BranchOffices.Any(bo => bo.BranchOfficeID == id);
        //}
    }
}