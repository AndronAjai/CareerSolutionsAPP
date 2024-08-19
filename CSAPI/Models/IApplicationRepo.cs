
namespace CSAPI.Models
{
    public interface IApplicationRepo
    {
        List<Application> GetAll();
        Application FindById(int id);
        void AddApplication(Application app);
        void UpdateApplication(int id, Application app);
        void DeleteApplication(int id);
    }

    public class ApplicationRepo : IApplicationRepo
    {
        CareerSolutionsDB _context;
        public ApplicationRepo(CareerSolutionsDB context)
        {
            _context = context;
        }
        public void AddApplication(Application app)
        {
            _context.Applications.Add(app);
            _context.SaveChanges();
            //throw new NotImplementedException();
        }

        public void DeleteApplication(int id)
        {
            Application app = _context.Applications.Find(id);
            _context.Applications.Remove(app);
            _context.SaveChanges();
            //throw new NotImplementedException();
        }

        public Application FindById(int id)
        {
            return _context.Applications.Find(id);
            //throw new NotImplementedException();
        }

        public List<Application> GetAll()
        {
            List<Application> appList = _context.Applications.ToList();
            return appList;
            //throw new NotImplementedException();
        }

        public void UpdateApplication(int id, Application app)
        {
            Application updatedApp = _context.Applications.Find(id);

            updatedApp.JobID = app.JobID;
            updatedApp.JobSeekerID = app.JobSeekerID;
            updatedApp.ApplicationDate = app.ApplicationDate;
            updatedApp.Status = app.Status;

            _context.SaveChanges();

            //throw new NotImplementedException();
        }
    }
}
