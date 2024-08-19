namespace CSAPI.Models
{
    public interface IEmployerRepo
    {
        List<Employer> GetAll();
        Employer FindById(int id);
        void AddEmployer(Employer emp);
        void UpdateEmployer(int id, Employer emp);
        void DeleteEmployer(int id);
    }

    public class EmployerRepo : IEmployerRepo
    {
        CareerSolutionsDB _context;
        public EmployerRepo(CareerSolutionsDB context)
        {
            _context = context;
        }
        public void AddEmployer(Employer emp)
        {
            _context.Employers.Add(emp);
            _context.SaveChanges();
            //throw new NotImplementedException();
        }

        public void DeleteEmployer(int id)
        {
            Employer emp = _context.Employers.Find(id);
            _context.Employers.Remove(emp);
            _context.SaveChanges();
            //throw new NotImplementedException();
        }

        public Employer FindById(int id)
        {
            return _context.Employers.Find(id);
            //throw new NotImplementedException();
        }

        public List<Employer> GetAll()
        {
            List<Employer> empList = _context.Employers.ToList();
            return empList;
            throw new NotImplementedException();
        }

        public void UpdateEmployer(int id, Employer emp)
        {
            Employer updatedEmp = _context.Employers.Find(id);

            updatedEmp.CompanyName = emp.CompanyName;
            updatedEmp.ContactPerson = emp.ContactPerson;
            updatedEmp.PhoneNumber = emp.PhoneNumber;
            updatedEmp.CompanyAddress = emp.CompanyAddress;
            updatedEmp.IndustryType = emp.IndustryType;
            updatedEmp.WebsiteURL = emp.WebsiteURL;
           
            _context.SaveChanges();
            //throw new NotImplementedException();
        }
    }
}
