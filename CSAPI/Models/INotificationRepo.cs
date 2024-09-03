using Microsoft.EntityFrameworkCore;

namespace CSAPI.Models
    {
    public interface INotificationRepo
        {
        Task<bool> AddNotificationAsync(Notification user);
        Task<bool> IsApplnExistsAsync(int appid);
        Task<bool> AddDeleteNotificationAsync(int applnid,Notification noti2);
        }

    public class NotificationRepo : INotificationRepo
        {
        private readonly CareerSolutionsDB _context;
        public NotificationRepo(CareerSolutionsDB context)
            {
             _context = context;
            }
        public async Task<bool> AddNotificationAsync(Notification notification)
            {
            if (!await IsApplnExistsAsync(notification.ApplicationID))
                {
                return false; 
                }
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return true;
            
            }
        public async Task<bool> AddDeleteNotificationAsync(int applnid, Notification noti2)
            {
            if (noti2 == null)
                {
                noti2 = new Notification();
                }

            var applnrow = await _context.Applications
                .Where(a => a.ApplicationID == applnid)
                .FirstOrDefaultAsync();

            var jobid = await _context.Applications
                .Where(b => b.ApplicationID == applnid)
                .Select(b => b.JobID)
                .FirstOrDefaultAsync();


            var empid = await _context.Jobs
                .Where(a => a.JobID == jobid)
                .Select(a => a.EmployerID)
                .FirstOrDefaultAsync();



            noti2.ApplicationID = applnid;
            noti2.Message = "Have Successfully deleted the job";
            noti2.EmployerID = empid;

            if (await AddNotificationAsync(noti2))
                {
                return true;
                }
            return false;
            }

        public async Task<bool> IsApplnExistsAsync(int appid)
            {
            return await _context.Applications.AnyAsync(u => u.ApplicationID == appid);
            }
        }


    }
