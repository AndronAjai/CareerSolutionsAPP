using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CSAPI.Models;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace CSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class RegistrationController : ControllerBase
    {
        IJobSeekerRepo _JSrepo;
        IEmployerRepo _IEmployerRepo;
        CareerSolutionsDB _context;

        public RegistrationController(IJobSeekerRepo js, IEmployerRepo emp, CareerSolutionsDB context)
        {
            _IEmployerRepo = emp;
            _JSrepo = js;
            _context = context;
        }



        public class JobSeekerViewModel
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string? PhoneNumber { get; set; }
            public string? Address { get; set; }
            public string? ProfileSummary { get; set; }
            public string KeySkills { get; set; }
            public string ExpertField { get; set; }
            public IFormFile Resume { get; set; }
            public string? AcademicDetails { get; set; }
            public string? ProfessionalDetails { get; set; }
            public string? PreferredIndustry { get; set; }
            public string? PreferredSpecialization { get; set; }
            public int UserID { get; set; }
        }


        [HttpPost("Addjsprofile")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateJobSeeker([FromForm] JobSeekerViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Request.Cookies.TryGetValue("UserID", out var userIdString) && int.TryParse(userIdString, out int userId))
                {
                    var jobSeeker = new JobSeeker
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhoneNumber = model.PhoneNumber,
                        Address = model.Address,
                        ProfileSummary = model.ProfileSummary,
                        KeySkills = model.KeySkills,
                        ExpertField = model.ExpertField,
                        AcademicDetails = model.AcademicDetails,
                        ProfessionalDetails = model.ProfessionalDetails,
                        PreferredIndustry = model.PreferredIndustry,
                        PreferredSpecialization = model.PreferredSpecialization,
                        UserID = userId
                    };
                    //var IdCookie = Convert.ToInt32(Request.Cookies["Id"]);
                    ////model.UserID = IdCookie;
                    ////var resumePath = await SaveResumeFile(model.Resume, 0);
                    //var jobSeeker = new JobSeeker
                    //{
                    //    FirstName = model.FirstName,
                    //    LastName = model.LastName,
                    //    PhoneNumber = model.PhoneNumber,
                    //    Address = model.Address,
                    //    ProfileSummary = model.ProfileSummary,
                    //    KeySkills = model.KeySkills,
                    //    ExpertField = model.ExpertField,
                    //    AcademicDetails = model.AcademicDetails,
                    //    ProfessionalDetails = model.ProfessionalDetails,
                    //    PreferredIndustry = model.PreferredIndustry,
                    //    PreferredSpecialization = model.PreferredSpecialization,
                    //    //UserID = model.UserID
                    //};

                    //    _context.JobSeekers.Add(jobSeeker);
                    //    await _context.SaveChangesAsync();

                    //    var resumePath = await SaveResumeFile(model.Resume, jobSeeker.JobSeekerID);

                    //    jobSeeker.ResumePath = resumePath;
                    //    //jobSeeker.UserID = IdCookie;
                    //    _context.JobSeekers.Update(jobSeeker);
                    //    await _context.SaveChangesAsync();

                    //    return CreatedAtAction(nameof(GetJobSeeker), new { id = jobSeeker.JobSeekerID }, jobSeeker);
                    //}

                    //return BadRequest(ModelState);

                    _context.JobSeekers.Add(jobSeeker);
                    await _context.SaveChangesAsync();

                    var resumePath = await SaveResumeFile(model.Resume, jobSeeker.JobSeekerID);
                    jobSeeker.ResumePath = resumePath;
                    _context.JobSeekers.Update(jobSeeker);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction(nameof(GetJobSeeker), new { id = jobSeeker.JobSeekerID }, jobSeeker);
                }
                else
                {
                    return BadRequest("User is not authenticated.");
                }
            }

            return BadRequest(ModelState);
        }


        [HttpGet("{id}")]
        public IActionResult GetJobSeeker(int id)
        {
            var jobSeeker = _context.JobSeekers.Find(id);

            if (jobSeeker == null)
            {
                return NotFound();
            }

            return Ok(jobSeeker);
        }

        [HttpGet("view-resume/{id}")]
        public IActionResult ViewResume(int id)
        {
            var jobSeeker = _context.JobSeekers.Find(id);

            if (jobSeeker == null || string.IsNullOrEmpty(jobSeeker.ResumePath))
            {
                return NotFound();
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), jobSeeker.ResumePath);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var fileExtension = Path.GetExtension(filePath);

            var mimeType = fileExtension switch
            {
                ".pdf" => "application/pdf",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                _ => "application/octet-stream",
            };

            return File(fileBytes, mimeType, $"Resume_{id}{fileExtension}");
        }

        private async Task<string> SaveResumeFile(IFormFile resume, int jobSeekerId)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Resumes");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileName = $"{jobSeekerId}{Path.GetExtension(resume.FileName)}";
            var filePath = Path.Combine(folderPath, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await resume.CopyToAsync(fileStream);
            }

            return Path.Combine("Resumes", fileName);
        }



        [HttpPost("RegisterEmployer")]
        [AllowAnonymous]
        public async Task<ActionResult> Post([FromBody] Employer emp)
        {
            //// Retrieve the UserId from the cookie
            //if (Request.Cookies.TryGetValue("UId", out var userIdString))
            //{
            //    // Parse the string to an integer
            //    if (int.TryParse(userIdString, out int userId))
            //    {
            //        // Use the UserId as needed, for example, associating it with the Employer
            //        emp.UserID = userId;

            //        await _IEmployerRepo.AddEmployerAsync(emp);
            //        return StatusCode((int)HttpStatusCode.Created);
            //    }
            //    else
            //    {
            //        return BadRequest("Invalid UserId in cookie.");
            //    }
            //}

            //return BadRequest("User is not authenticated.");

            if (Request.Cookies.TryGetValue("UserID", out var userIdString) && int.TryParse(userIdString, out int userId))
            {
                emp.UserID = userId;

                await _IEmployerRepo.AddEmployerAsync(emp);
                return StatusCode((int)HttpStatusCode.Created);
            }

            return BadRequest("User is not authenticated.");
        }


    }
} 
