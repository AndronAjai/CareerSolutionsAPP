using CSAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CSAPI.Areas.JobSeekers.Controllers
    {
    [Area("JobSeekers")]
    [Route("api/jobseekers/[controller]")]
    [ApiController]
    [Authorize(Roles = "JobSeeker")]
    public class JsEmployerController : ControllerBase
        {
        IEmployerRepo _AemRepo;
        public JsEmployerController(IEmployerRepo AemRepo)
            {
            _AemRepo = AemRepo;
            }


        // GET api/<JsEmployer>/5
        [HttpGet("EmpInfo")]
        public async Task<IActionResult> EmpInfo(int paramname)
            {
            var jsempinfo = await _AemRepo.FindByJobId(paramname);
            return Ok(new { jsempinfo.CompanyName, jsempinfo.ContactPerson, jsempinfo.PhoneNumber, jsempinfo.CompanyAddress, jsempinfo.WebsiteURL});
            
            }

      
        }
    }
