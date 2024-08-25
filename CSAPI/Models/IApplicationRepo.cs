using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CSAPI.Models
{
    public interface IApplicationRepo
    {
        Task<List<JobApplication>> GetAllAsync();
        Task<JobApplication> FindByIdAsync(int id);

        Task<bool> AddApplicationAsync(int usrid,JobApplication app);
        Task<bool> UpdateApplicationAsync(int id, JobApplication app);
        Task<bool> DeleteApplicationAsync(int id);

        Task<IEnumerable<JobApplication>> FindByJobSeekerIdAsync(int jobSeekerId);

        Task<bool> UpdateJobSeekerIdAsync(int id, IEnumerable<JobApplication> Apj);

    }

    public class ApplicationRepo : IApplicationRepo
    {
        private readonly CareerSolutionsDB _context;

        public ApplicationRepo(CareerSolutionsDB context)
        {
            _context = context;
        }


        // c2 in Calling IsJobIdExistsAsync function
        public async Task<bool> AddApplicationAsync(int usrid,JobApplication app)

        {


            // 112
            var jsid = await _context.JobSeekers
            .Where(a => a.UserID == usrid)
            .Select(a => a.JobSeekerID)
            .FirstOrDefaultAsync();

            // checking if already Applied or not to the Job [1,2,3]
            var InsertCheck = await _context.Applications
                .Where(g => g.JobSeekerID == jsid)
                .Select(x => x.JobID)
                .ToListAsync();


            // fetching overall job
            var listjobid = await _context.Jobs
                .Select(f => f.JobID)
                .ToListAsync();

            // Check if the Job ID exists and jobseekerid in their table before adding the JobApplication
            if (!await IsJobIdExistsAsync(app.JobID, app.JobSeekerID))
                {
                return false;
                }

            if (jsid != app.JobSeekerID)
                {
                return false;
                }

            // value should be part of jobid but not already present
            if (listjobid.Contains(app.JobID) && !(InsertCheck.Contains(app.JobID)))
                {
                // then add 
                _context.Applications.Add(app);
                await _context.SaveChangesAsync();
                return true;
                }
            else
                {
                return false;
                }



            
        }


        public async Task<bool> DeleteApplicationAsync(int usrid)

        {
            var id  = await _context.JobSeekers
                .Where(a => a.UserID == usrid)
               .Select(a => a.JobSeekerID)
                .FirstOrDefaultAsync();

            var app = await _context.Applications.FindAsync(id);
            if (app != null)
            {

                return false;
            }
            _context.Applications.Remove(app);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<JobApplication> FindByIdAsync(int id)
        {

            
            return await _context.Applications.FindAsync(id);

        }


        // change 4 
        public async Task<IEnumerable<JobApplication>> FindByJobSeekerIdAsync(int usrid)
            {

            var jobSeekerId = await _context.JobSeekers
            .Where(a => a.UserID == usrid)
            .Select(a => a.JobSeekerID)
            .FirstOrDefaultAsync();

            return await _context.Applications
                                 .Where(e => e.JobSeekerID == jobSeekerId)
                                 .ToListAsync();
            }




        public async Task<List<JobApplication>> GetAllAsync()
        {
            return await _context.Applications.ToListAsync();
        }


        public async Task<bool> UpdateApplicationAsync(int id, JobApplication app)
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





        public async Task<bool> UpdateJobSeekerIdAsync(int usrid, IEnumerable<JobApplication> apps)
            {
            // Check if any rows exist with the given jobSeekerId

            var jobSeekerId = await _context.JobSeekers
                    .Where(a => a.UserID == usrid)
                    .Select(a => a.JobSeekerID)
                    .FirstOrDefaultAsync();
            var existingApplications = await _context.Applications
                .Where(a => a.JobSeekerID == jobSeekerId)
                .ToListAsync();

            if (!existingApplications.Any())
                {
                // No rows found with the given jobSeekerId
                return false;
                }

            // conversion of FrontEnd values to List
            List<JobApplication> newData=apps.ToList();
 
            foreach (var applicationRow in existingApplications)
                {
                // comparision of each updated row(id) to frontendValues(id)
                for (int i = 0; i < newData.Count; i++)
                    {
                    
                    if (applicationRow.ApplicationID == newData[i].ApplicationID)
                        {
                        // Update the application details

                        applicationRow.JobID = newData[i].JobID;
                        applicationRow.JobSeekerID = newData[i].JobSeekerID;
                        applicationRow.ApplicationDate = newData[i].ApplicationDate;
                        applicationRow.Status = newData[i].Status;

                        } 
                    }
                }

            // Save changes to the database
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
