using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CSAPI.Models
{
    public interface IUserRepo
    {
        Task<List<User>> GetAllAsync();
        Task<User> FindByIdAsync(int id);
        Task<bool> AddUserAsync(User user);
        Task<bool> UpdateUserAsync(int id, User user);
        Task<bool> DeleteUserAsync(int id);
    }

    public class UserRepo : IUserRepo
    {
        private readonly CareerSolutionsDB _context;

        public UserRepo(CareerSolutionsDB context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> FindByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<bool> AddUserAsync(User user)
        {
            // Check if the BranchOffice exists before adding the user
            if (!await IsBranchOfficeExistsAsync(user.BranchOfficeID))
                return false;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateUserAsync(int id, User user)
        {
            var updatedUser = await _context.Users.FindAsync(id);
            if (updatedUser == null)
                return false;

            // Check if the BranchOffice exists before updating the user
            if (!await IsBranchOfficeExistsAsync(user.BranchOfficeID))
                return false;

            updatedUser.Username = user.Username;
            updatedUser.Password = user.Password;
            updatedUser.Email = user.Email;
            updatedUser.BranchOfficeID = user.BranchOfficeID;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var userToDelete = await _context.Users.FindAsync(id);
            if (userToDelete == null)
                return false;

            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<bool> IsBranchOfficeExistsAsync(int? branchOfficeID)
        {
            return branchOfficeID.HasValue && await _context.BranchOffices.AnyAsync(b => b.BranchOfficeID == branchOfficeID.Value);
        }
    }
}
