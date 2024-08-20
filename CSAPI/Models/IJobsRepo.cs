using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbCreationApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CSAPI.Models
{
    public interface IJobsRepo
    {
        Task<List<Job>> GetAllAsync();
        Task<Job> FindByIdAsync(int id);
        Task<bool> AddJobAsync(Job job);
        Task<bool> UpdateJobAsync(int id, Job job);
        Task<bool> DeleteJobAsync(int id);
        Task<bool> IsEmployerExistsAsync(int employerId); // New method to check if Employer exists
    }

    public class JobsRepo : IJobsRepo
    {
        private readonly CareerSolutionsDB _context;

        public JobsRepo(CareerSolutionsDB context)
        {
            _context = context;
        }

        public async Task<List<Job>> GetAllAsync()
        {
            return await _context.Jobs.ToListAsync();
        }

        public async Task<Job> FindByIdAsync(int id)
        {
            return await _context.Jobs.FindAsync(id);
        }

        public async Task<bool> AddJobAsync(Job job)
        {
            if (!await IsEmployerExistsAsync(job.EmployerID))
            {
                return false; // Employer does not exist, cannot create the job
            }

            try
            {
                _context.Jobs.Add(job);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false; // Handle exceptions as needed
            }
        }

        public async Task<bool> UpdateJobAsync(int id, Job job)
        {
            var existingJob = await _context.Jobs.FindAsync(id);
            if (existingJob == null || !await IsEmployerExistsAsync(job.EmployerID))
            {
                return false;
            }

            existingJob.EmployerID = job.EmployerID;
            existingJob.JobTitle = job.JobTitle;
            existingJob.JobDescription = job.JobDescription;
            existingJob.IndustryType = job.IndustryType;
            existingJob.Specialization = job.Specialization;
            existingJob.RequiredSkills = job.RequiredSkills;
            existingJob.ExperienceLevel = job.ExperienceLevel;
            existingJob.Location = job.Location;
            existingJob.SalaryRange = job.SalaryRange;
            existingJob.PostedDate = job.PostedDate;
            existingJob.ApplicationDeadline = job.ApplicationDeadline;
            existingJob.JobType = job.JobType;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false; // Handle exceptions as needed
            }
        }

        public async Task<bool> DeleteJobAsync(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
            {
                return false;
            }

            _context.Jobs.Remove(job);

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false; // Handle exceptions as needed
            }
        }

        public async Task<bool> IsEmployerExistsAsync(int employerId)
        {
            return await _context.Employers.AnyAsync(e => e.EmployerID == employerId);
        }
    }
}
