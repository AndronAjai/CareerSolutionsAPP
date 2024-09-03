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
        public Task<IQueryable<JobApplication>> GetApplicationsOfJob(int id);
        public Task<List<Tuple<JobSeeker, int>>> GetRecommendationBySkills(int id);
        public Task<List<JobSeeker>> GetRecommendationByIndustry(int id);
        public Task<List<JobSeeker>> GetRecommendationBySpecialization(int id);
        public Task<List<Tuple<Notification,string,int,string>>> GetNotificationAsync(int empid);
        public Task SelectApplicationsForJobAsync(int jobId, int selectedApplicationId);
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

        public async Task<IQueryable<JobApplication>> GetApplicationsOfJob(int id)
        {
            var applications = from app in _context.Applications
                               where app.JobID == id
                               select app;

            return applications;
        }


        private List<string> ConvertSkillsToKeySkills(List<string> skills)
        {
            List<string> keySkills = new List<string>();

            foreach (var skill in skills)
            {
                var SkillRel = _context.SkillRelations;
                foreach (var item in SkillRel)
                {
                    var subskill = item.SubSkill.Split(", ").ToList();
                    string keySkill = null;

                    if (subskill.Contains(skill, StringComparer.OrdinalIgnoreCase))
                    {
                        keySkill = item.KeySkill;
                    }

                    if (!string.IsNullOrEmpty(keySkill) && !keySkills.Contains(keySkill))
                    {
                        keySkills.Add(keySkill);
                    }
                }

            }
            return keySkills;
        }


        public async Task<List<Tuple<JobSeeker, int>>> GetRecommendationBySkills(int id)
        {
            List<Tuple<JobSeeker, int>> recommendedJobSeeker = new List<Tuple<JobSeeker, int>>();
            Tuple<JobSeeker, int> recommendedrow;
            List<JobSeeker> jobSeekerList;

            var jobrequired = (from job in _context.Jobs
                              where job.JobID == id
                              select job).FirstOrDefault();


            if (jobrequired == null)
            {
                return recommendedJobSeeker; 
            }

            string[] jobSkillsArray = jobrequired.RequiredSkills.Split(",");
            List<string> jobSkillsList = jobSkillsArray.ToList();
            List<string> jobKeySkills = ConvertSkillsToKeySkills(jobSkillsList);

            jobSeekerList = _context.JobSeekers.ToList();

            foreach (JobSeeker jobseeker in jobSeekerList)
            {
                int noOfSkills = 0;

                string[] jobSeekerSkillsArray = jobseeker.KeySkills.Split(",");
                List<string> jobSeekerSkillsList = jobSeekerSkillsArray.ToList();
                List<string> jobSeekerKeySkills = ConvertSkillsToKeySkills(jobSeekerSkillsList);

                foreach (var seekerSkill in jobSeekerKeySkills)
                {
                    foreach (var item in jobKeySkills)
                    {
                        if(item.ToLower() == seekerSkill.ToLower())
                        {
                            noOfSkills++;
                        }
                    }
                }


                if (noOfSkills > 0)
                {
                    recommendedrow = new Tuple<JobSeeker, int>(jobseeker, noOfSkills);
                    recommendedJobSeeker.Add(recommendedrow);
                }
            }

            recommendedJobSeeker.Sort((x, y) => y.Item2.CompareTo(x.Item2)); //descending order

            return await Task.FromResult(recommendedJobSeeker);
        }


        public async Task<List<JobSeeker>> GetRecommendationByIndustry(int id)
        {
            List<JobSeeker> recommendedJobSeeker = new List<JobSeeker>();

            var Industry = (from job in _context.Jobs
                            where job.JobID == id
                            select job.IndustryType).SingleOrDefault();
            string jobInd = Industry.ToString();

            foreach (JobSeeker jobseeker in _context.JobSeekers)
            {
                if (jobInd == jobseeker.PreferredIndustry)
                    recommendedJobSeeker.Add(jobseeker);                
            }
            return await Task.FromResult(recommendedJobSeeker);
        }

        public async Task<List<JobSeeker>> GetRecommendationBySpecialization(int id)
        {
            List<JobSeeker> recommendedJobSeeker = new List<JobSeeker>();

            var Specialization = (from job in _context.Jobs
                                 where job.JobID == id
                                 select job.Specialization).SingleOrDefault();
            string jobSpecial = Specialization.ToString();

            foreach (JobSeeker jobseeker in _context.JobSeekers)
            { 
                if (jobSpecial == jobseeker.PreferredSpecialization)
                    recommendedJobSeeker.Add(jobseeker);
            }
            return await Task.FromResult(recommendedJobSeeker);
        }

        public async Task<List<Tuple<Notification,string,int,string>>> GetNotificationAsync(int empid)
        {
            var notification = from n in _context.Notifications
                               where empid == n.EmployerID
                               select n;
            var NotiAppl = notification.Join(_context.Applications, n => n.ApplicationID, a => a.ApplicationID, (n, a) => new {n,a}).Join
                (
                _context.Jobs,
                na => na.a.JobID,
                j => j.JobID,
                (na, j) => new { na, j }).Join
                (_context.JobSeekers, naj => naj.na.a.JobSeekerID , s => s.JobSeekerID, (naj,s) => new 
                {
                noti=naj.na.n,
                jsname = s.FirstName + " " + s.LastName,
                job = naj.na.a.JobID,
                jobtitle = naj.j.JobTitle                
                }).ToList();

            List<Tuple<Notification, string,int,string>> list = new List<Tuple<Notification,string,int, string>>();

            foreach (var item in NotiAppl)
            {
                Tuple<Notification, string, int, string>  tuple = new Tuple<Notification,string, int,string>(item.noti,item.jsname, item.job, item.jobtitle);
                list.Add(tuple);
            }

            return await Task.FromResult(list);
           
        }

        public async Task SelectApplicationsForJobAsync(int jobId, int selectedApplicationId)
        {
            var appln = await _context.Applications.FindAsync(selectedApplicationId);
            appln.Status = "Accepted";

            JobStatusNotification jsnotify = new JobStatusNotification
            {
                Msg = "Congrats, You are selected for this Job",
                ApplicationID = appln.ApplicationID
            };
            await _context.JobStatusNotifications.AddAsync(jsnotify);

            var otherApplications = _context.Applications
                .Where(a => a.JobID == jobId && a.ApplicationID != selectedApplicationId)
                .ToList();

            foreach (var application in otherApplications)
            {
                application.Status = "Rejected";

                JobStatusNotification rejectionNotification = new JobStatusNotification
                {
                    Msg = "Sorry, you were not selected for this Job",
                    ApplicationID = application.ApplicationID
                };

                await _context.JobStatusNotifications.AddAsync(rejectionNotification);
            }

            await _context.SaveChangesAsync();
        }


    }


}
  

