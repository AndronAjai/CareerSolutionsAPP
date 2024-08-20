using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using CSAPI.Models;
using DbCreationApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace CSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class BranchOfficeController : ControllerBase
    {
        private readonly IBranchOfficeRepo _Repo;

        public BranchOfficeController(IBranchOfficeRepo repo)
        {
            _Repo = repo;
        }

        // GET: api/BranchOffice
        [HttpGet]
        public async Task<ActionResult<List<BranchOffice>>> ShowAll()
        {
            return await _Repo.GetAllAsync();
        }

        // GET api/BranchOffice/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BranchOffice>> GetBranchOffice(int id)
        {
            var branchOffice = await _Repo.FindByIdAsync(id);
            if (branchOffice == null)
            {
                return NotFound();
            }
            return branchOffice;
        }

        // POST api/BranchOffice
        [HttpPost]
        public async Task<HttpStatusCode> Post([FromBody] BranchOffice branchOffice)
        {
            await _Repo.AddBranchOfficesAsync(branchOffice);
            return HttpStatusCode.Created;
        }

        // PUT api/BranchOffice/5
        [HttpPut("{id}")]
        public async Task<HttpStatusCode> Put(int id, [FromBody] BranchOffice branchOffice)
        {
            await _Repo.UpdateBranchOfficesAsync(id, branchOffice);
            return HttpStatusCode.NoContent;
        }

        // DELETE api/BranchOffice/5
        [HttpDelete("{id}")]
        public async Task<HttpStatusCode> Delete(int id)
        {
            await _Repo.DeleteBranchOfficesAsync(id);
            return HttpStatusCode.NoContent;
        }
    }
}
