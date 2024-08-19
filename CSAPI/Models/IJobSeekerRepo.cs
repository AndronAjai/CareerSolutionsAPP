namespace CSAPI.Models
{
    public interface IJobSeekerRepo
    {
        List<JobSeeker> GetAll();
        JobSeeker FindById(int id);
        void AddJobSeeker(JobSeeker js);
        void UpdateJobSeeker(int id, JobSeeker js);
        void DeleteJobSeeker(int id);
    }

    public class JobSeekerRepo : IJobSeekerRepo
    {
        CareerSolutionsDB _context;
        public JobSeekerRepo(CareerSolutionsDB context)
        {
            _context = context;
        }
        public void AddJobSeeker(JobSeeker js)
        {
            _context.JobSeekers.Add(js);
            _context.SaveChanges();
            //throw new NotImplementedException();
        }

        public void DeleteJobSeeker(int id)
        {
            JobSeeker js = _context.JobSeekers.Find(id);
            _context.JobSeekers.Remove(js);
            _context.SaveChanges();
            //throw new NotImplementedException();
        }

        public JobSeeker FindById(int id)
        {
            return _context.JobSeekers.Find(id);
            //throw new NotImplementedException();
        }

        public List<JobSeeker> GetAll()
        {
            List<JobSeeker> jsList = _context.JobSeekers.ToList();
            return jsList;
            //throw new NotImplementedException();
        }

        public void UpdateJobSeeker(int id, JobSeeker js)
        {
            JobSeeker updatedJs = _context.JobSeekers.Find(id);

            updatedJs.FirstName = js.FirstName;
            updatedJs.LastName = js.LastName;
            updatedJs.PhoneNumber = js.PhoneNumber;
            updatedJs.Address = js.Address;
            updatedJs.ProfileSummary = js.ProfileSummary;
            updatedJs.KeySkills = js.KeySkills;
            updatedJs.ExpertField = js.ExpertField;
            updatedJs.ResumePath = js.ResumePath;
            updatedJs.AcademicDetails = js.AcademicDetails;
            updatedJs.ProfessionalDetails = js.ProfessionalDetails;
            updatedJs.PreferredIndustry = js.PreferredIndustry;
            updatedJs.PreferredSpecialization = js.PreferredSpecialization;

            _context.SaveChanges();
            //throw new NotImplementedException();
        }

    }
}
