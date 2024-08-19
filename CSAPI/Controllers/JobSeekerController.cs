using Microsoft.AspNetCore.Mvc;
using System.Net;
using CSAPI.Models;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CareerSolutionWebApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobSeekerController : ControllerBase
    {
        IJobSeekerRepo _Repo;

        public JobSeekerController(IJobSeekerRepo repo)
        {
            _Repo = repo;
        }

        // GET: api/<JobSeekerController>
        [HttpGet]
        public List<JobSeeker> ShowAll()
        {
            return _Repo.GetAll();
        }

        // GET api/<JobSeekerController>/5
        [HttpGet("{id}")]
        public JobSeeker FindJobSeeker(int id)
        {
            return _Repo.FindById(id);
        }

        // POST api/<JobSeekerController>
        [HttpPost]
        public HttpStatusCode Post([FromBody] JobSeeker js)
        {
            _Repo.AddJobSeeker(js);
            return HttpStatusCode.Created;
        }

        // PUT api/<JobSeekerController>/5
        [HttpPut("{id}")]
        public HttpStatusCode Put(int id, [FromBody] JobSeeker js)
        {
            _Repo.UpdateJobSeeker(id, js);
            return HttpStatusCode.NoContent;
        }

        // DELETE api/<JobSeekerController>/5
        [HttpDelete("{id}")]
        public HttpStatusCode Delete(int id)
        {
            _Repo.DeleteJobSeeker(id);
            return HttpStatusCode.NoContent;
        }
    }
}
