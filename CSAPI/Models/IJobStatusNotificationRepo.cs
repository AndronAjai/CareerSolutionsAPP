using Microsoft.EntityFrameworkCore;

namespace CSAPI.Models
    {
    public interface IJobStatusNotificationRepo
    {
        Task<bool> AddJSNotificationAsync(JobStatusNotification jsnotification);

     }

    public class JobStatusNotificationRepo : IJobStatusNotificationRepo
    {
        private readonly CareerSolutionsDB _context;
        public JobStatusNotificationRepo(CareerSolutionsDB context)
            {
             _context = context;
            }
        public async Task<bool> AddJSNotificationAsync(JobStatusNotification jsnotification)
            {
            
            _context.JobStatusNotifications.Add(jsnotification);
            await _context.SaveChangesAsync();
            return true;
            
            }


        
        }


    }
