﻿using Microsoft.AspNetCore.Http;
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


        [HttpPost("AddEmployerProfile")]
        public async Task<ActionResult> CreateEmployer([FromBody] Employer emp)
        {

            if (Request.Cookies.TryGetValue("UsID", out var userIdString) && int.TryParse(userIdString, out int userId))
            {
                emp.UserID = userId;

                await _IEmployerRepo.AddEmployerAsync(emp);
                return CreatedAtAction("CreateEmployer", new { id = emp.EmployerID }, emp);
            }

            return BadRequest("User not registered!!!.");
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


        [HttpPost("AddJobseekerProfile")]
        public async Task<IActionResult> CreateJobSeeker([FromForm] JobSeekerViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Request.Cookies.TryGetValue("UsID", out var userIdString) && int.TryParse(userIdString, out int userId))
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

                    _context.JobSeekers.Add(jobSeeker);
                    await _context.SaveChangesAsync();

                    var resumePath = await SaveResumeFile(model.Resume, jobSeeker.JobSeekerID);
                    jobSeeker.ResumePath = resumePath;
                    _context.JobSeekers.Update(jobSeeker);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction("CreateJobSeeker", new { id = jobSeeker.JobSeekerID }, jobSeeker);
                }
                else
                {
                    return BadRequest("User is not authenticated.");
                }
            }

            return BadRequest(ModelState);
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


    }
} 
