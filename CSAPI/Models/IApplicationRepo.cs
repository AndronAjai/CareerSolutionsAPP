using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbCreationApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CSAPI.Models
{
    public interface IApplicationRepo
    {
        Task<List<Application>> GetAllAsync();
        Task<Application> FindByIdAsync(int id);
        Task<bool> AddApplicationAsync(Application app);
        Task<bool> UpdateApplicationAsync(int id, Application app);
        Task<bool> DeleteApplicationAsync(int id);

        Task<IEnumerable<Application>> FindByJobSeekerIdAsync(int jobSeekerId);

        Task<bool> UpdateJobSeekerIdAsync(int id, Application app);
    }

    public class ApplicationRepo : IApplicationRepo
    {
        private readonly CareerSolutionsDB _context;

        public ApplicationRepo(CareerSolutionsDB context)
        {
            _context = context;
        }

        // c2 in Calling IsJobIdExistsAsync function
        public async Task<bool> AddApplicationAsync(Application app)
        {
            
            // Check if the Application ID exists before adding the Application
            if (!await IsJobIdExistsAsync(app.JobID,app.JobSeekerID))
                return false;

            _context.Applications.Add(app);
            await _context.SaveChangesAsync();
            return true;
            
        }

        public async Task<bool> DeleteApplicationAsync(int id)
        {
            var app = await _context.Applications.FindAsync(id);
            if (app != null)
            {

                return false;
            }
            _context.Applications.Remove(app);
            await _context.SaveChangesAsync();
            return true;




        }

        public async Task<Application> FindByIdAsync(int id)
        {

            
            return await _context.Applications.FindAsync(id);

        }

        // change 4 
        public async Task<IEnumerable<Application>> FindByJobSeekerIdAsync(int jobSeekerId)
            {
            return await _context.Applications
                                 .Where(e => e.JobSeekerID == jobSeekerId)
                                 .ToListAsync();
            }



        public async Task<List<Application>> GetAllAsync()
        {
            return await _context.Applications.ToListAsync();
        }

        public async Task<bool> UpdateApplicationAsync(int id, Application app)
        {
            var updatedApp = await _context.Applications.FindAsync(id);
            if (updatedApp == null)
            {
                return false;
            }



            // Check if the Jobseekerid,jobid exists before updating the Jobseeker
            if (!await IsJobIdExistsAsync(app.JobID,app.JobSeekerID))
                return false;

            var updatedUser = await _context.Applications.FindAsync(id);
            if (updatedUser == null)
                return false;


            updatedApp.JobID = app.JobID;
            updatedApp.JobSeekerID = app.JobSeekerID;
            updatedApp.ApplicationDate = app.ApplicationDate;
            updatedApp.Status = app.Status;

            await _context.SaveChangesAsync();
            return true;
            }

        public  async Task<bool> UpdateJobSeekerIdAsync(int jobSeekerId, Application app)
            {

            // Check if the Jobseekerid,jobid exists before updating the Jobseeker
            if (!await IsJobIdExistsAsync(app.JobID, app.JobSeekerID))
                return false;

            var  update_rows =  _context.Applications
                                 .Where(e => e.JobSeekerID == jobSeekerId)
                                 .ToListAsync();

            foreach (var eachrows in await update_rows)
                {

                eachrows.JobID = app.JobID;
                eachrows.JobSeekerID = app.JobSeekerID;
                eachrows.ApplicationDate = app.ApplicationDate;
                eachrows.Status = app.Status;
                }

            await _context.SaveChangesAsync();
            return true;

            }


        // c1 added a method for Insert check
        private async Task<bool> IsJobIdExistsAsync(int? id,int? jsid)
            {
            return id.HasValue && await _context.Jobs.AnyAsync(b => b.JobID == id.Value)
                && jsid.HasValue && await _context.JobSeekers.AnyAsync(c => c.JobSeekerID == jsid.Value);
            }


        }
}
