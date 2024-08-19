using Microsoft.AspNetCore.Mvc;
using System.Net;
using CSAPI.Models;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CareerSolutionWebApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        IJobsRepo _Repo;

        public JobController(IJobsRepo repo)
        {
            _Repo = repo;
        }

        // GET: api/<JobController>
        [HttpGet]
        public List<Job> ShowAll()
        {
            return _Repo.GetAll();
        }

        // GET api/<JobController>/5
        [HttpGet("{id}")]
        public Job FindJob(int id)
        {
            return _Repo.FindById(id);
        }

        // POST api/<JobController>
        [HttpPost]
        public HttpStatusCode Post([FromBody] Job job)
        {
            _Repo.AddJobs(job);
            return HttpStatusCode.Created;
        }

        // PUT api/<JobController>/5
        [HttpPut("{id}")]
        public HttpStatusCode Put(int id, [FromBody] Job job)
        {
            _Repo.UpdateJobs(id, job);
            return HttpStatusCode.NoContent;
        }

        // DELETE api/<JobController>/5
        [HttpDelete("{id}")]
        public HttpStatusCode Delete(int id)
        {
            _Repo.DeleteJobs(id);
            return HttpStatusCode.NoContent;
        }
    }
}
