﻿
using CSAPI.Areas.JobSeekers.Models;
using CSAPI.Models;
using DbCreationApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CSAPI.Areas.JobSeekers.Controllers
{
    [Area("JobSeekers")]
    [Route("api/jobseekers/[controller]")]
    [ApiController]
    [Authorize(Roles = "JobSeeker")]
    public class AJobSeekersController : ControllerBase
    {
        // Constructor creation mapping Irepository (Function Connecting Backend)
        IBranchOfficeRepo _AbrRepo;
        IJobsRepo _AjbRepo;
        IApplicationRepo _AapnRepo;
        AIApplicationRepo _AzapnRepo;
        IUserRepo _AusRepo;

        public AJobSeekersController(IBranchOfficeRepo Arepo, IJobsRepo AjbRepo, IApplicationRepo AapnRepo, IUserRepo AusRepo)
            {
            _AbrRepo = Arepo;
            _AjbRepo = AjbRepo;
            _AapnRepo = AapnRepo;
            _AusRepo = AusRepo;
            }



        // Job Seeker Can View all the Branch Office Relations 
        [HttpGet("BOR")]
        public async Task<ActionResult<IEnumerable<JobSeeker>>> ShowAll()
            {
            List<BranchOffice>? jsviewbor = await _AbrRepo.GetAllAsync();
            return Ok(jsviewbor);
            }

        // Job Seeker Can see the jobs available 
        [HttpGet("ViewJobs")]
        public async Task<ActionResult<IEnumerable<BranchOffice>>> DisplayJobs()
            {
            List<Job> jsviewjobs = await _AjbRepo.GetAllAsync();
            return Ok(jsviewjobs);
            }


        // Job Seeker Can View His own Application

        [HttpGet("ViewjsApplication")]
        public async Task<ActionResult<IEnumerable<Application>>> jsviewAppln()
            {

            var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
            
            
            var viewjsappln = await _AapnRepo.FindByJobSeekerIdAsync(userIdCookie);
            if (viewjsappln == null)
                {
                return NotFound();
                }
            return Ok(viewjsappln);
            }

        // Job Seeker Can Update His own Application
        [HttpPut("UpdatejsApplication")]

        public async Task<ActionResult<IEnumerable<Application>>> jsupdateAppln([FromBody] IEnumerable<Application>  Apj)
            {
            // Retrieve the 'UserId' cookie from the request
            var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
            if (userIdCookie == null)
                {
                return NotFound();
                }

            var success = await _AapnRepo.UpdateJobSeekerIdAsync(userIdCookie, Apj);
            return Ok(success);
            }

        // Job Seeker Can Delete His own Application(need to implement(few confusions)

        [HttpDelete("DeletejsApplication")]
        public async Task<ActionResult<IEnumerable<Application>>> jsdeleteAppln(int id)
            {
            // Retrieve the 'UserId' cookie from the request
            var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
            if (userIdCookie == null)
                {
                return NotFound();
                }

            var success = await _AapnRepo.DeleteApplicationAsync(userIdCookie);
            if (!success)
                {
                return BadRequest("Could not delete the Application.");
                }

            return NoContent();
            }


        // Job Seeker Can View His own User profile

        [HttpGet("ViewjsUser")]
        public async Task<ActionResult<IEnumerable<Application>>> jsviewUser()
            {

            var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);


            var viewjsappln = await _AusRepo.FindByIdAsync(userIdCookie);
            if (viewjsappln == null)
                {
                return NotFound();
                }
            return Ok(viewjsappln);
            }







        }
}
