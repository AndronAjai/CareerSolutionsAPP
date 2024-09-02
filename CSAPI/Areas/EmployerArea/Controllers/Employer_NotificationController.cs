using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CSAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using CSAPI.Areas.EmployerArea.Models;

namespace CSAPI.Areas.EmployerArea.Controllers
{
    [Area("EmployerArea")]
    [Route("api/[controller]/")]
    [ApiController]
    [Authorize(Roles = "Employer")]
    public class Employer_NotificationController : ControllerBase
    {
        private readonly IEmployerAreaRepo _eRepo;       

        public Employer_NotificationController(IEmployerAreaRepo empRepo)
        { 
            _eRepo = empRepo;
        }

        [HttpGet("AllNotifications")]
        public async Task<List<Tuple<Notification,string,int,string>>> ShowAllNotifications()
        {
            //var userIdCookie = Convert.ToInt32(Request.Cookies["UserId"]);
            var userIdClaim = User.FindFirst("UserId")?.Value;
            bool x = int.TryParse(userIdClaim, out var userIdCookie);
            int Empid = _eRepo.GetEmpID(userIdCookie);
            return await _eRepo.GetNotificationAsync(Empid);
        }
    }
}
