﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CSAPI.Models
{
    public interface IJobSeekerRepo
    {
        Task<List<JobSeeker>> GetAllAsync();
        Task<JobSeeker> FindByIdAsync(int id);
        Task<bool> AddJobSeekerAsync(JobSeeker js);
        Task<bool> UpdateJobSeekerAsync(int id, JobSeeker js);
        Task<bool> DeleteJobSeekerAsync(int id);
        Task<bool> IsUserExistsAsync(int userId); 
        JobSeeker FindById(int id);
        public Task<JobSeeker> FindAsync(int id);
    }

    public class JobSeekerRepo : IJobSeekerRepo
    {
        private readonly CareerSolutionsDB _context;

        public JobSeekerRepo(CareerSolutionsDB context)
        {
            _context = context;
        }

        public async Task<List<JobSeeker>> GetAllAsync()
        {
            return await _context.JobSeekers.ToListAsync();
        }

        public async Task<JobSeeker> FindByIdAsync(int usrid)
        {
            var id = await _context.JobSeekers
            .Where(a => a.UserID == usrid)
            .Select(a => a.JobSeekerID)
            .FirstOrDefaultAsync();

            return await _context.JobSeekers.FindAsync(id);
        }

        public async Task<JobSeeker> FindAsync(int id)
        {
            return await _context.JobSeekers.FindAsync(id);
        }

        public async Task<bool> AddJobSeekerAsync(JobSeeker js)
        {
            if (!await IsUserExistsAsync(js.UserID))
            {
                return false;
            }

            _context.JobSeekers.Add(js);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateJobSeekerAsync(int usrid, JobSeeker js)

        {
            var id = await _context.JobSeekers
            .Where(a => a.UserID == usrid)
            .Select(a => a.JobSeekerID)
            .FirstOrDefaultAsync();
            var existingJobSeeker = await _context.JobSeekers.FindAsync(id);
            if (existingJobSeeker == null || !await IsUserExistsAsync(js.UserID))
            {
                return false;
            }

            existingJobSeeker.FirstName = js.FirstName;
            existingJobSeeker.LastName = js.LastName;
            existingJobSeeker.PhoneNumber = js.PhoneNumber;
            existingJobSeeker.Address = js.Address;
            existingJobSeeker.ProfileSummary = js.ProfileSummary;
            existingJobSeeker.KeySkills = js.KeySkills;
            existingJobSeeker.ExpertField = js.ExpertField;
            existingJobSeeker.ResumePath = js.ResumePath;
            existingJobSeeker.AcademicDetails = js.AcademicDetails;
            existingJobSeeker.ProfessionalDetails = js.ProfessionalDetails;
            existingJobSeeker.PreferredIndustry = js.PreferredIndustry;
            existingJobSeeker.PreferredSpecialization = js.PreferredSpecialization;
            existingJobSeeker.UserID = js.UserID; 

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteJobSeekerAsync(int usrid)
        {
            var id = await _context.JobSeekers
            .Where(a => a.UserID == usrid)
            .Select(a => a.JobSeekerID)
            .FirstOrDefaultAsync();

            var js = await _context.JobSeekers.FindAsync(id);
            if (js == null)
            {
                return false;
            }

            _context.JobSeekers.Remove(js);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsUserExistsAsync(int userId)
        {
            return await _context.Users.AnyAsync(u => u.UserID == userId);
        }

        public JobSeeker FindById(int id)
        {
            return _context.JobSeekers.Find(id);
        }
    }
}
