using CSAPI.Areas.JobSeekers.Models;
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
    public class JsBranchOfficeController : ControllerBase
        {
        // Constructor creation mapping Irepository (Function Connecting Backend)
        IBranchOfficeRepo _AbrRepo;

        public JsBranchOfficeController(IBranchOfficeRepo Arepo)
            {
            _AbrRepo = Arepo;
            }

        // Job Seeker Can View all the Branch Office Relations 
        [HttpGet("viewBOR")]
        public async Task<ActionResult<IEnumerable<BranchOffice>>> ShowAll()
            {
            List<BranchOffice>? jsviewbor = await _AbrRepo.GetAllAsync();
            return Ok(jsviewbor);
            }


        }
    }
