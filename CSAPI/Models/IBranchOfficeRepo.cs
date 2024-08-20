using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbCreationApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CSAPI.Models
{
    public interface IBranchOfficeRepo
    {
        Task<List<BranchOffice>> GetAllAsync();
        Task<BranchOffice> FindByIdAsync(int id);
        Task AddBranchOfficesAsync(BranchOffice branchOffice);
        Task UpdateBranchOfficesAsync(int id, BranchOffice branchOffice);
        Task DeleteBranchOfficesAsync(int id);
    }

    public class BranchOfficesRepo : IBranchOfficeRepo
    {
        private readonly CareerSolutionsDB _context;

        public BranchOfficesRepo(CareerSolutionsDB context)
        {
            _context = context;
        }

        public async Task<List<BranchOffice>> GetAllAsync()
        {
            return await _context.BranchOffices.ToListAsync();
        }

        public async Task<BranchOffice> FindByIdAsync(int id)
        {
            return await _context.BranchOffices.FindAsync(id);
        }

        public async Task AddBranchOfficesAsync(BranchOffice branchOffice)
        {
            await _context.BranchOffices.AddAsync(branchOffice);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBranchOfficesAsync(int id, BranchOffice branchOffice)
        {
            BranchOffice updatedBranchOffice = await _context.BranchOffices.FindAsync(id);

            if (updatedBranchOffice != null)
            {
                updatedBranchOffice.BranchName = branchOffice.BranchName;
                updatedBranchOffice.BranchAddress = branchOffice.BranchAddress;
                updatedBranchOffice.PhoneNumber = branchOffice.PhoneNumber;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteBranchOfficesAsync(int id)
        {
            BranchOffice branchOffice = await _context.BranchOffices.FindAsync(id);

            if (branchOffice != null)
            {
                _context.BranchOffices.Remove(branchOffice);
                await _context.SaveChangesAsync();
            }
        }
    }
}
