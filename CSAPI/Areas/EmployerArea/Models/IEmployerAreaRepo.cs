using CSAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CSAPI.Areas.EmployerArea.Models
{
    public interface IEmployerAreaRepo
    {
        public int GetEmpID(int userID);
        public Task<IQueryable<Job>> GetMyJobs(int empid);
        public Task<IQueryable<JobApplication>> GetMyApplications(int empid);
        public Task<JobApplication> GetApplication(int appid);
        public Task<IQueryable<JobApplication>> GetApplicationsOfJob(int jobid);
    }

    public class EmployerAreaRepo : IEmployerAreaRepo
    {
        CareerSolutionsDB _context;

        public EmployerAreaRepo(CareerSolutionsDB context)
        {
            _context = context;
        }
        public int GetEmpID(int userID)
        {
            var Empid = from e in _context.Employers
                        where e.UserID == userID
                        select e.EmployerID;

            var empId = Empid.FirstOrDefault();

            return empId;
        }

        public async Task<IQueryable<Job>> GetMyJobs(int empid)
        {
            var job = from jobs in _context.Jobs
                      where jobs.EmployerID == empid
                      select jobs;
            return job;
        }

        public async Task<IQueryable<JobApplication>> GetMyApplications(int empid)
        {
            var job = from jobs in _context.Jobs
                      where jobs.EmployerID == empid
                      select jobs.JobID;
            var applications = from app in _context.Applications
                               where job.Contains(app.JobID)
                               select app;

            return applications;
        }

        public async Task<JobApplication> GetApplication(int appid)
        {
            var application = from app in _context.Applications
                              where appid == app.ApplicationID
                              select app;
            var appl = application.FirstOrDefault();
            return appl;
        }

        public async Task<IQueryable<JobApplication>> GetApplicationsOfJob(int jobid)
        {
            var applications = from app in _context.Applications
                               where app.JobID == jobid
                               select app;

            return applications;
        }

    }
}