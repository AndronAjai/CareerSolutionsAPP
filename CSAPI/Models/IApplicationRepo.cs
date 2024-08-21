using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CSAPI.Models
{
    public interface IApplicationRepo
    {
        Task<List<Application>> GetAllAsync();
        Task<Application> FindByIdAsync(int id);
        Task AddApplicationAsync(Application app);
        Task UpdateApplicationAsync(int id, Application app);
        Task<bool> DeleteApplicationAsync(int id);
    }

    public class ApplicationRepo : IApplicationRepo
    {
        private readonly CareerSolutionsDB _context;

        public ApplicationRepo(CareerSolutionsDB context)
        {
            _context = context;
        }

        public async Task AddApplicationAsync(Application app)
        {
            _context.Applications.Add(app);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteApplicationAsync(int id)
        {
            var app = await _context.Applications.FindAsync(id);
            if (app != null)
            {
                _context.Applications.Remove(app);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Application> FindByIdAsync(int id)
        {
            return await _context.Applications.FindAsync(id);
        }

        public async Task<List<Application>> GetAllAsync()
        {
            return await _context.Applications.ToListAsync();
        }

        public async Task UpdateApplicationAsync(int id, Application app)
        {
            var updatedApp = await _context.Applications.FindAsync(id);
            if (updatedApp != null)
            {
                updatedApp.JobID = app.JobID;
                updatedApp.JobSeekerID = app.JobSeekerID;
                updatedApp.ApplicationDate = app.ApplicationDate;
                updatedApp.Status = app.Status;

                await _context.SaveChangesAsync();
            }
        }
    }
}
