using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CSAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CareerSolutionWebApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        IApplicationRepo _Repo;

        public ApplicationController(IApplicationRepo repo)
        {
            _Repo = repo;
        }

        // GET: api/<ApplicationController>
        [HttpGet]
        public List<Application> ShowAll()
        {
            return _Repo.GetAll();
        }

        // GET api/<ApplicationController>/5
        [HttpGet("{id}")]
        public Application FindApplication(int id)
        {
            return _Repo.FindById(id);
        }

        // POST api/<ApplicationController>
        [HttpPost]
        public HttpStatusCode Post([FromBody] Application app)
        {
            _Repo.AddApplication(app);
            return HttpStatusCode.Created;
        }
    

        // PUT api/<ApplicationController>/5
        [HttpPut("{id}")]
        public HttpStatusCode Put(int id, [FromBody] Application app)
        {
            _Repo.UpdateApplication(id, app);
            return HttpStatusCode.NoContent;
        }

        // DELETE api/<ApplicationController>/5
        [HttpDelete("{id}")]
        public HttpStatusCode Delete(int id)
        {
            _Repo.DeleteApplication(id);
            return HttpStatusCode.NoContent;
        }
    }
}
