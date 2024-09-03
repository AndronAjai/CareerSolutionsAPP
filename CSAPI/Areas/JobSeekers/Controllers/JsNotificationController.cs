using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CSAPI.Areas.JobSeekers.Models;
using Microsoft.AspNetCore.Authorization;
using CSAPI.Models;
using CSAPI.Areas.JobSeekers.Models;

namespace CSAPI.Areas.JobSeekers.Controllers
{
    [Area("JobSeekers")]
    [Route("api/jobseekers/[controller]")]
    [ApiController]
    [Authorize(Roles = "JobSeeker")]
    public class JsNotificationController : ControllerBase
    {
        private readonly IJobSeekerAreaRepo _jRepo;

        public JsNotificationController(IJobSeekerAreaRepo jRepo)
        {
            _jRepo = jRepo;
        }

        [HttpGet("AllNotifications")]
        public async Task<List<Tuple<JobStatusNotification, int, string>>> ShowAllNotifications()
        {
            //var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
            var userIdClaim = User.FindFirst("UserId")?.Value;
            bool x = int.TryParse(userIdClaim, out var userIdCookie);
            int jid = _jRepo.GetJSID(userIdCookie);
            return await _jRepo.GetNotificationAsync(jid);
        }
    }
}
