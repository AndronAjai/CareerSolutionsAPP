using CSAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace CSAPI.Areas.EmployerArea.Models
{
    public interface IEmployerAreaRepo
    {
        public int GetEmpID(int userID);
        public Task<IQueryable<Job>> GetMyJobs(int empid);
        public Task<IQueryable<JobApplication>> GetMyApplications(int empid);
        public Task<JobApplication> GetApplication(int appid);
        public Task<IQueryable<JobApplication>> GetApplicationsOfJob(int jobid);
        public Task<List<Tuple<JobSeeker, int>>> GetRecommendationBySkills(int jobid);
        public Task<List<JobSeeker>> GetRecommendationByIndustry(int jobid);
        public Task<List<JobSeeker>> GetRecommendationBySpecialization(int jobid);
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

        public async Task<List<Tuple<JobSeeker, int>>> GetRecommendationBySkills(int jobid)
        {
            List<Tuple<JobSeeker,int>> recommendedJobSeeker = new List<Tuple<JobSeeker, int>>();
            Tuple<JobSeeker, int> recommendedrow;
            List<JobSeeker> jobSeekerList;

            var jobSkills = from job in _context.Jobs
                            where job.JobID == jobid
                            select job.RequiredSkills;

            List<string> skills =  jobSkills.ToList();

            var jslist = _context.JobSeekers.ToList();
            //var List = from js in _context.JobSeekers
            //           select js;

            var SkillArray = new string[jslist.Count];
            foreach (JobSeeker jobseeker in jslist)
            {
                bool condition = false;
                int noOfSkills = 0;
               
                //var Skill = jobseeker.KeySkills;
                SkillArray.Append(jobseeker.KeySkills);

                //List<string> JobSeekerSkills = await Task.FromResult(Skill.ToList());
                List<string> JobSeekerSkills = SkillArray.ToList();

                foreach (var i in JobSeekerSkills)
                {
                    noOfSkills = 0;
                    foreach (var j in skills)
                    {
                        if (i == j)
                            noOfSkills++;
                        condition = true;
                    }
                    if (condition)
                    {
                        recommendedrow = new Tuple<JobSeeker, int>(jobseeker, noOfSkills);
                        recommendedJobSeeker.Add(recommendedrow);
                    }
                }
            }
            recommendedJobSeeker.Sort((x, y) => x.Item2.CompareTo(y.Item2));
            return await Task.FromResult(recommendedJobSeeker);
        }

        public async Task<List<JobSeeker>> GetRecommendationByIndustry(int jobid)
        {
            List<JobSeeker> recommendedJobSeeker = new List<JobSeeker>();

            var Industry = from job in _context.Jobs
                            where job.JobID == jobid
                            select job.IndustryType;
            string jobInd = Industry.ToString();

            foreach (JobSeeker jobseeker in _context.JobSeekers)
            {
                if (jobInd == jobseeker.PreferredIndustry)
                    recommendedJobSeeker.Add(jobseeker);                
            }
            return await Task.FromResult(recommendedJobSeeker);
        }

        public async Task<List<JobSeeker>> GetRecommendationBySpecialization(int jobid)
        {
            List<JobSeeker> recommendedJobSeeker = new List<JobSeeker>();

            var Specialization = from job in _context.Jobs
                                 where job.JobID == jobid
                                 select job.Specialization;
            string jobSpecial = Specialization.ToString();

            foreach (JobSeeker jobseeker in _context.JobSeekers)
            { 
                if (jobSpecial == jobseeker.PreferredSpecialization)
                    recommendedJobSeeker.Add(jobseeker);
            }
            return await Task.FromResult(recommendedJobSeeker);
        }
    }

   
}
  

