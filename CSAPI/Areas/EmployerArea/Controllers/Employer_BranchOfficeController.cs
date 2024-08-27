using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using CSAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using CSAPI.Areas.EmployerArea.Models;

namespace CSAPI.Areas.EmployerArea.Controllers
{
    [Area("EmployerArea")]
    [Route("api/[controller]/")]
    [ApiController]
    [Authorize(Roles = "Employer")]
    public class Employer_BranchOfficeController : ControllerBase
    {
        private readonly IEmployerRepo _repo;
        private readonly IApplicationRepo _applicationRepo;
        private readonly IJobsRepo _jobsRepo;
        private readonly IUserRepo _userRepo;
        private readonly IEmployerAreaRepo _eRepo;
        private readonly IJobSeekerRepo _JobSeekerRepo;
        private readonly IBranchOfficeRepo _BranchOfficeRepo;


        public Employer_BranchOfficeController(IEmployerRepo repo, IApplicationRepo applicationRepo, IJobsRepo jobsRepo, IUserRepo userRepo, IEmployerAreaRepo empRepo, IJobSeekerRepo JobSeekerRepo, IBranchOfficeRepo BranchOfficeRepo)
        {
            _repo = repo;
            _applicationRepo = applicationRepo;
            _jobsRepo = jobsRepo;
            _userRepo = userRepo;
            _eRepo = empRepo;
            _JobSeekerRepo = JobSeekerRepo;
            _BranchOfficeRepo = BranchOfficeRepo;
        }

        [HttpGet("AllBranch")]
        public async Task<ActionResult<IEnumerable<BranchOffice>>> ShowAll()
        {
            var branchoffice = await _BranchOfficeRepo.GetAllAsync();

            return Ok(branchoffice);
        }

        [HttpGet("BranchDetails/{id}")]
        public async Task<BranchOffice> GetBranch(int id)
        {
            var branch = await _BranchOfficeRepo.FindByIdAsync(id);
            return branch;
        }
    }
}
