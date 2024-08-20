using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbCreationApp.Models;
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
        Task<bool> IsUserExistsAsync(int userId); // Check if UserID exists in Users table
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

        public async Task<JobSeeker> FindByIdAsync(int id)
        {
            return await _context.JobSeekers.FindAsync(id);
        }

        public async Task<bool> AddJobSeekerAsync(JobSeeker js)
        {
            if (!await IsUserExistsAsync(js.UserID))
            {
                return false; // UserID does not exist, cannot create job seeker
            }

            _context.JobSeekers.Add(js);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateJobSeekerAsync(int id, JobSeeker js)
        {
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
            existingJobSeeker.UserID = js.UserID; // Ensure UserID is updated

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteJobSeekerAsync(int id)
        {
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
    }
}
