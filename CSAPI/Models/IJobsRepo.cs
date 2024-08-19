namespace CSAPI.Models
{
    public interface IJobsRepo
    {
        List<Job> GetAll();
        Job FindById(int id);
        void AddJobs(Job j);
        void UpdateJobs(int id, Job j);
        void DeleteJobs(int id);
    }

    public class JobsRepo : IJobsRepo
    {
        CareerSolutionsDB _context;
        public JobsRepo(CareerSolutionsDB context)
        {
            _context = context;
        }
        public void AddJobs(Job j)
        {
            _context.Jobs.Add(j);
            _context.SaveChanges();
            //throw new NotImplementedException();
        }

        public void DeleteJobs(int id)
        {
            Job j = _context.Jobs.Find(id);
            _context.Jobs.Remove(j);
            _context.SaveChanges();
            //throw new NotImplementedException();
        }

        public Job FindById(int id)
        {
            return _context.Jobs.Find(id);
            //throw new NotImplementedException();
        }

        public List<Job> GetAll()
        {
            List<Job> jList = _context.Jobs.ToList();
            return jList;
            //throw new NotImplementedException();
        }

        public void UpdateJobs(int id, Job j)
        {
            Job updatedJobs = _context.Jobs.Find(id);

            updatedJobs.JobID = j.JobID;
            updatedJobs.EmployerID = j.EmployerID;
            updatedJobs.JobTitle = j.JobTitle;
            updatedJobs.JobDescription = j.JobDescription;
            updatedJobs.IndustryType = j.IndustryType;
            updatedJobs.Specialization = j.Specialization;
            updatedJobs.RequiredSkills = j.RequiredSkills;
            updatedJobs.ExperienceLevel = j.ExperienceLevel;
            updatedJobs.Location = j.Location;
            updatedJobs.SalaryRange = j.SalaryRange;
            updatedJobs.PostedDate = j.PostedDate;
            updatedJobs.ApplicationDeadline = j.ApplicationDeadline;
            updatedJobs.JobType = j.JobType;

            _context.SaveChanges();
            //throw new NotImplementedException();
        }

    }
}

//namespace CareerSolutionWebApiApp.Models
//{
//    public interface Interface
//    {
//    }
//}
