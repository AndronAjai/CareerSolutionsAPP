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

        Task<bool> AddApplicationAsync(int usrid,int jobid);
        Task<bool> UpdateApplicationAsync(int id, JobApplication app);
        Task<bool> DeleteApplicationAsync(int id);

        Task<IEnumerable<JobApplication>> FindByJobSeekerIdAsync(int jobSeekerId);

        Task<bool> UpdateJobSeekerIdAsync(int id, IEnumerable<JobApplication> Apj);

        Task<int> FindEmpID(int jobid);

        Task<int> FetchApplnID(int usrid, int jobid);

    }

    public class ApplicationRepo : IApplicationRepo
    {
        private readonly CareerSolutionsDB _context;

        public ApplicationRepo(CareerSolutionsDB context)
        {
            _context = context;
        }


        
        public async Task<bool> AddApplicationAsync(int usrid,int jobid)

        {
            var datecheck = await _context.Jobs
            .Where(a => a.JobID== jobid)
            .Select(a => a.ApplicationDeadline)
            .FirstOrDefaultAsync();

            if (datecheck < DateTime.Now)
                {
                return false;
                }

            // 112
            var jsid = await _context.JobSeekers
            .Where(a => a.UserID == usrid)
            .Select(a => a.JobSeekerID)
            .FirstOrDefaultAsync();

            var InsertCheck = await _context.Applications
                .Where(g => g.JobSeekerID == jsid)
                .Select(x => x.JobID)
                .ToListAsync();


            var listjobid = await _context.Jobs
                .Select(f => f.JobID)
                .ToListAsync();

            if (!await IsJobIdExistsAsync(jobid, jsid))
                {
                return false;
                }

          
            if (listjobid.Contains(jobid) && !(InsertCheck.Contains(jobid)))
                {

                JobApplication app = new JobApplication();
                app.JobID = jobid;
                app.JobSeekerID = jsid;
                app.ApplicationDate = DateTime.Now;
                app.Status = "Applied";

                _context.Applications.Add(app);
                await _context.SaveChangesAsync();
                return true;
                }
            else
                {
                return false;
                }



            
        }

        public async Task<int> FetchApplnID(int usrid, int jobid)
            {
            var jsid = await _context.JobSeekers
           .Where(a => a.UserID == usrid)
           .Select(a => a.JobSeekerID)
           .FirstOrDefaultAsync();

            var applnid = await _context.Applications
                .Where(a => a.JobID == jobid && a.JobSeekerID == jsid)
                .Select(c => c.ApplicationID)
                .FirstOrDefaultAsync();

            if (applnid != null)
                {
                return applnid;
                }
            return -1;
            }

        public async Task<bool> DeleteApplicationAsync(int applnid)

            {


            var app = await _context.Applications.FindAsync(applnid);
            if (app == null)
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

            var jobSeekerId = await _context.JobSeekers
                    .Where(a => a.UserID == usrid)
                    .Select(a => a.JobSeekerID)
                    .FirstOrDefaultAsync();
            var existingApplications = await _context.Applications
                .Where(a => a.JobSeekerID == jobSeekerId)
                .ToListAsync();

            if (!existingApplications.Any())
                {
                return false;
                }

            List<JobApplication> newData=apps.ToList();
 
            foreach (var applicationRow in existingApplications)
                {
                for (int i = 0; i < newData.Count; i++)
                    {
                    
                    if (applicationRow.ApplicationID == newData[i].ApplicationID)
                        {

                        applicationRow.JobID = newData[i].JobID;
                        applicationRow.JobSeekerID = newData[i].JobSeekerID;
                        applicationRow.ApplicationDate = newData[i].ApplicationDate;
                        applicationRow.Status = newData[i].Status;

                        } 
                    }
                }

            await _context.SaveChangesAsync();

            return true;
            }


        private async Task<bool> IsJobIdExistsAsync(int? id,int? jsid)
            {
            return id.HasValue && await _context.Jobs.AnyAsync(b => b.JobID == id.Value)
                && jsid.HasValue && await _context.JobSeekers.AnyAsync(c => c.JobSeekerID == jsid.Value);
            }

        public async Task<int> FindEmpID(int jobid)
            {
            var empid = await _context.Jobs
        .Where(a => a.JobID == jobid)
        .Select(a => a.EmployerID)
        .FirstOrDefaultAsync();
            return empid;
            }


        }
}
