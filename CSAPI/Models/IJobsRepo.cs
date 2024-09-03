using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSAPI.Models;
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

        Task<List<Job>> filterbyind(string industry);
        Task<List<Job>> filterbyspec(string specialization);
        Task<List<Ranking>> GetJobs(int usrid);
        List<string> ConvertSkillsToKeySkills(List<string> skills);
        }
    public class JobSeekerDTO
        {
        public string PreferredIndustry { get; set; }
        public string PreferredSpecialization { get; set; }
        public string KeySkills { get; set; }

        public  bool AreFieldsEmpty(JobSeekerDTO preferences)
            {
            bool  status= string.IsNullOrWhiteSpace(preferences.PreferredIndustry) &&
                   string.IsNullOrWhiteSpace(preferences.PreferredSpecialization) &&
                   string.IsNullOrWhiteSpace(preferences.KeySkills);
            return  status;
            }


        public int CheckPreferences(JobSeekerDTO preferences)
            {
            if (AreFieldsEmpty(preferences))
                {
                // Handle the case where all fields are empty
                Console.WriteLine("All fields are empty.");
                // recommend all jobs
                return 1;
                }
            return 0;
          
            }


        }



    public class Ranking:Job
        {
        public int rank { get; set; }

        }

    public class JobsRepo : IJobsRepo
    {
        private readonly CareerSolutionsDB _context;

        public JobsRepo(CareerSolutionsDB context)
        {
            _context = context;
        }

        public List<string> ConvertSkillsToKeySkills(List<string> skills)
        {
            HashSet<string> keySkills = new HashSet<string>();

            var skillRelations = _context.SkillRelations.ToList();

            foreach (var skill in skills)
            {
                var keySkill = skillRelations
                    .Where(ksr => ksr.SubSkill.Split(',').Select(s => s.Trim()).Contains(skill, StringComparer.OrdinalIgnoreCase))
                    .Select(ksr => ksr.KeySkill)
                    .FirstOrDefault();

                if (!string.IsNullOrEmpty(keySkill))
                {
                    keySkills.Add(keySkill);
                }
            }

            return keySkills.ToList();
        }


        public bool MatchingSkills(Job jobrow, JobSeekerDTO jspreferences)
        {
            string[] jobSeekerSkillsArray = jspreferences.KeySkills.Trim().ToLower().Split(',');
            List<string> jobSeekerSkillsList = jobSeekerSkillsArray.ToList();
            List<string> jobSeekerKeySkills = ConvertSkillsToKeySkills(jobSeekerSkillsList);

            string[] jobSkillsArray = jobrow.RequiredSkills.Trim().ToLower().Split(',');
            List<string> jobSkillsList = jobSkillsArray.ToList();
            List<string> jobKeySkills = ConvertSkillsToKeySkills(jobSkillsList);

            bool hasMatchingSkill = jobKeySkills.Any(skill => jobSeekerKeySkills.Contains(skill));
            return hasMatchingSkill;
        }



        public bool MatchingSpecialisation(Job jobrow, JobSeekerDTO jspreferences)
            {
            string[] cs = jspreferences.PreferredSpecialization.Trim().ToLower().Split(',');


            string[] ns = jobrow.Specialization.Trim().ToLower().Split(',');

            bool hasMatchingSpecialisation = ns.Any(skill => cs.Contains(skill));
            return hasMatchingSpecialisation;
            }

        public bool MatchingPrefferedIndustry(Job jobrow, JobSeekerDTO jspreferences)
            {
            string[] cs = jspreferences.PreferredIndustry.Trim().ToLower().Split(',');


            string[] ns = jobrow.IndustryType.Trim().ToLower().Split(',');

            bool hasMatchingPrefferedIndustry = ns.Any(skill => cs.Contains(skill));
            return hasMatchingPrefferedIndustry;
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


        public async Task<List<Job>> filterbyind(string industry)
            {

            var queryind = await _context.Jobs
                .Where(d => d.IndustryType == industry)
                .ToListAsync();

            return queryind;
            }


        public async Task<List<Job>> filterbyspec(string specialization)
            {

            var queryspec = await _context.Jobs
                .Where(d => d.Specialization == specialization)
                .ToListAsync();

            return queryspec;
            }



        public async Task<List<Ranking>> GetJobs(int usrid)
            {

            var jspreferences = await _context.JobSeekers
                .Where(a => a.UserID == usrid)
            .Select(a => new JobSeekerDTO
            {
                PreferredIndustry = a.PreferredIndustry,
                PreferredSpecialization = a.PreferredSpecialization,
                 KeySkills = a.KeySkills
            })
        .FirstOrDefaultAsync();

            int value_recommend = jspreferences.CheckPreferences(jspreferences);
            List<Job> Alljobs = await GetAllAsync();
          
                    List<Ranking> newjobsformat = new List<Ranking>();
                    int rankgiven;
                    foreach (var jobrow in Alljobs)
                        {
                        rankgiven = 0;

                        // first priority Key skills exist with matching 

                        if (  (!string.IsNullOrWhiteSpace(jspreferences.KeySkills)) && MatchingSkills(jobrow, jspreferences))
                            {
                            rankgiven += 3;
                            // then second priority is specialisation
                            if (!string.IsNullOrWhiteSpace(jspreferences.PreferredSpecialization) && MatchingSpecialisation(jobrow, jspreferences))
                                {
                                rankgiven += 2;
                                }

                            if (!string.IsNullOrWhiteSpace(jspreferences.PreferredIndustry) && MatchingPrefferedIndustry(jobrow,jspreferences))
                                {
                                rankgiven += 1;
                                }
                            }

                        // case when first priority is absent 
                        if (!string.IsNullOrWhiteSpace(jspreferences.PreferredSpecialization) && MatchingSpecialisation(jobrow, jspreferences))
                            {
                            rankgiven += 2;
                            if (!string.IsNullOrWhiteSpace(jspreferences.PreferredIndustry) && MatchingPrefferedIndustry(jobrow, jspreferences))
                                {
                                rankgiven += 1;
                                }
                            }
                        // case when first second priority is absent
                        if (!string.IsNullOrWhiteSpace(jspreferences.PreferredIndustry) && MatchingPrefferedIndustry(jobrow, jspreferences))
                            {
                            rankgiven += 1;
                            }
                          if (rankgiven > 0)
                        {
                        Ranking job1 = new Ranking
                            {
                            JobID = jobrow.JobID,
                            EmployerID = jobrow.EmployerID,
                            JobTitle = jobrow.JobTitle,
                            JobDescription = jobrow.JobDescription,
                            IndustryType = jobrow.IndustryType,
                            Specialization = jobrow.Specialization,
                            RequiredSkills = jobrow.RequiredSkills,
                            ExperienceLevel = jobrow.ExperienceLevel,
                            Location = jobrow.Location,
                            SalaryRange = jobrow.SalaryRange,
                            PostedDate = jobrow.PostedDate,
                            ApplicationDeadline = jobrow.ApplicationDeadline,
                            JobType = jobrow.JobType,
                            rank = rankgiven
                            };

                        newjobsformat.Add(job1);
                        }

                        }
                    newjobsformat = newjobsformat.OrderByDescending(job => job.rank).ToList();
                    return newjobsformat;

                


            }






        }
}
