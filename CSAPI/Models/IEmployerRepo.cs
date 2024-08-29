using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CSAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CSAPI.Models
{
    public interface IEmployerRepo
    {
        Task<List<Employer>> GetAllAsync();
        Task<Employer> FindByIdAsync(int id);
        Task<bool> AddEmployerAsync(Employer emp);
        Task<bool> UpdateEmployerAsync(int id, Employer emp);
        Task<bool> DeleteEmployerAsync(int id);
        Task<bool> IsUserExistsAsync(int userId);
        Task<Employer> FindByJobId(int jobid);
    }

    public class EmployerRepo : IEmployerRepo
    {
        private readonly CareerSolutionsDB _context;

        public EmployerRepo(CareerSolutionsDB context)
        {
            _context = context;
        }

        public async Task<List<Employer>> GetAllAsync()
        {
            return await _context.Employers.ToListAsync();
        }

        public async Task<Employer> FindByIdAsync(int id)
        {
            return await _context.Employers.FindAsync(id);
        }

        public async Task<Employer> FindByJobId(int jobid)
            {
            var Employeeid = await _context.Jobs
           .Where(a => a.JobID == jobid)
           .Select(a => a.EmployerID)
           .FirstOrDefaultAsync();

            var EmployeeDet = await _context.Employers
                .Where(b => b.EmployerID == Employeeid)
                .FirstOrDefaultAsync();

            return EmployeeDet;
            }

        public async Task<bool> AddEmployerAsync(Employer emp)
        {
            if (!await IsUserExistsAsync(emp.UserID))
            {
                return false; // UserID does not exist, cannot create employer
            }

            _context.Employers.Add(emp);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateEmployerAsync(int id, Employer emp)
        {
            var existingEmployer = await _context.Employers.FindAsync(id);
            if (existingEmployer == null || !await IsUserExistsAsync(emp.UserID))
            {
                return false;
            }

            existingEmployer.CompanyName = emp.CompanyName;
            existingEmployer.ContactPerson = emp.ContactPerson;
            existingEmployer.PhoneNumber = emp.PhoneNumber;
            existingEmployer.CompanyAddress = emp.CompanyAddress;
            existingEmployer.IndustryType = emp.IndustryType;
            existingEmployer.WebsiteURL = emp.WebsiteURL;
            existingEmployer.UserID = emp.UserID; // Ensure UserID is updated

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEmployerAsync(int id)
        {
            var emp = await _context.Employers.FindAsync(id);
            if (emp == null)
            {
                return false;
            }

            _context.Employers.Remove(emp);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsUserExistsAsync(int userId)
        {
            return await _context.Users.AnyAsync(u => u.UserID == userId);
        }

      
    }
}
