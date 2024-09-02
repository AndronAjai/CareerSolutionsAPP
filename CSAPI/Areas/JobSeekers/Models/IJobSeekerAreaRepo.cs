
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CSAPI.Models;

namespace CSAPI.Areas.JobSeekers.Models
{
    public interface IJobSeekerAreaRepo
    {
        public int GetJSID(int userID);
        public Task<List<Tuple<JobStatusNotification, int, int, string>>> GetNotificationAsync(int jsid);
    }
        
    public class JobSeekerAreaRepo : IJobSeekerAreaRepo
    {
        CareerSolutionsDB _context;

        public JobSeekerAreaRepo(CareerSolutionsDB context)
        {
            _context = context;
        }

        public int GetJSID(int userID)
        {
            var jsId = from e in _context.Employers
                        where e.UserID == userID
                        select e.EmployerID;

            var jsID = jsId.FirstOrDefault();

            return jsID;
        }

        public async Task<List<Tuple<JobStatusNotification, int, int, string>>> GetNotificationAsync(int jsid)
        {

            var NotiAppl = _context.JobStatusNotifications.Join(_context.Applications, n => n.ApplicationID, a => a.ApplicationID, (n, a) => new { n, a }).Join
                (
                _context.Jobs,
                na => na.a.JobID,
                j => j.JobID,
                (na, j) => new { na, j }).Join
                (_context.JobSeekers, naj => naj.na.a.JobSeekerID, s => s.JobSeekerID, (naj, s) => new
                {
                    noti = naj.na.n,
                    sid = s.JobSeekerID,
                    job = naj.na.a.JobID,
                    jobtitle = naj.j.JobTitle
                }).ToList();


            List<Tuple<JobStatusNotification, int, int, string>> list = new List<Tuple<JobStatusNotification, int, int, string>>();
            foreach (var item in NotiAppl)
            {
                if(item.sid == jsid)
                {
                    Tuple<JobStatusNotification, int, int, string> tuple = new Tuple<JobStatusNotification, int, int, string>(item.noti, item.sid, item.job, item.jobtitle);
                    list.Add(tuple);
                }
            }

            return await Task.FromResult(list);

        }

    }

}