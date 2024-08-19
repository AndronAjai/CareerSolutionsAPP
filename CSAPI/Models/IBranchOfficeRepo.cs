
namespace CSAPI.Models
{
    public interface IBranchOfficeRepo
    {
        List<BranchOffice> GetAll();
        BranchOffice FindById(int id);
        void AddBranchOffices(BranchOffice j);
        void UpdateBranchOffices(int id, BranchOffice js);
        void DeleteBranchOffices(int id);
    }

    public class BranchOfficesRepo : IBranchOfficeRepo
    {
        CareerSolutionsDB _context;
        public BranchOfficesRepo(CareerSolutionsDB context)
        {
            _context = context;
        }
        public void AddBranchOffices(BranchOffice b)
        {
            _context.BranchOffices.Add(b);
            _context.SaveChanges();
            //throw new NotImplementedException();
        }

        public void DeleteBranchOffices(int id)
        {
            BranchOffice b = _context.BranchOffices.Find(id);
            _context.BranchOffices.Remove(b);
            _context.SaveChanges();
            //throw new NotImplementedException();
        }

        public BranchOffice FindById(int id)
        {
            return _context.BranchOffices.Find(id);
            //throw new NotImplementedException();
        }

        public List<BranchOffice> GetAll()
        {
            List<BranchOffice> bList = _context.BranchOffices.ToList();
            return bList;
            //throw new NotImplementedException();
        }

        public void UpdateBranchOffices(int id, BranchOffice b)
        {
            BranchOffice updatedBranchOffices = _context.BranchOffices.Find(id);

            updatedBranchOffices.BranchOfficeID = b.BranchOfficeID;
            updatedBranchOffices.BranchName = b.BranchName;
            updatedBranchOffices.BranchAddress = b.BranchAddress;
            updatedBranchOffices.PhoneNumber = b.PhoneNumber;
            _context.SaveChanges();
            //throw new NotImplementedException();
        }

    }
}

