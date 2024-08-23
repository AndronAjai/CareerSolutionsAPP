using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CSAPI.Models;
using CSAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace CareerSolutionWebApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationRepo _repo;

        public ApplicationController(IApplicationRepo repo)
        {
            _repo = repo;
        }

        // GET: api/Application
        [HttpGet]
        public async Task<List<JobApplication>> ShowAll()
        {
            return await _repo.GetAllAsync();
        }

        // GET api/Application/5
        [HttpGet("{id}")]
        public async Task<JobApplication> FindApplication(int id)
        {
            return await _repo.FindByIdAsync(id);
        }

        // POST api/Application
        [HttpPost]
        public async Task<HttpStatusCode> Post([FromBody] JobApplication app)
        {
            await _repo.AddApplicationAsync(app);
            return HttpStatusCode.Created;
        }

        // PUT api/Application/5
        [HttpPut("{id}")]
        public async Task<HttpStatusCode> Put(int id, [FromBody] JobApplication app)
        {
            await _repo.UpdateApplicationAsync(id, app);
            return HttpStatusCode.NoContent;
        }

        // DELETE api/Application/5
        [HttpDelete("{id}")]
        public async Task<HttpStatusCode> Delete(int id)
        {
            await _repo.DeleteApplicationAsync(id);
            return HttpStatusCode.NoContent;
        }
    }
}
