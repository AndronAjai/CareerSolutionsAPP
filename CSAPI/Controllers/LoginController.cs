using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CSAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        ILogin login;
        public AuthenticationController(ILogin _login)
        {
            login = _login;
        }
        [HttpPost("login")]
        public IActionResult LoginUser([FromBody] Login user)
        {

            if (user is null)
            {
                return BadRequest("Invalid user request!!!");
            }

            if (login.ValidateUser(user.UserName, user.Password,user.Role))
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSetting["JWT:Secret"]));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: ConfigurationManager.AppSetting["JWT:ValidIssuer"],
                    audience: ConfigurationManager.AppSetting["JWT:ValidAudience"],
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(6),
                    signingCredentials: signinCredentials
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new JWTTokenResponse { Token = tokenString });
            }
            return Unauthorized();
        }
    }

    public class JWTTokenResponse
    {
        public string? Token { get; set; }
    }

    public class Login
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }

        public string? Role { get; set; }
    }

    public interface ILogin
    {
        bool ValidateUser(string uname, string pwd,string role);
    }
    public class LoginRepo : ILogin
    {
        CareerSolutionsDB context;
        public LoginRepo(CareerSolutionsDB _context)
        {
            context = _context;
        }
        public bool ValidateUser(string uname, string pwd,string role)
        {
            //User user = context.Users.Find(uname);
            User user = context.Users.SingleOrDefault(u => u.Username == uname);

            if ((user.Password == pwd) && (user.Role == role))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
