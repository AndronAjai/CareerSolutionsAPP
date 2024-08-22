using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CSAPI.Models;
using Microsoft.AspNetCore.Authorization;
using DbCreationApp.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace CSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly ILogin _login;

        public LoginController(ILogin login)
        {
            _login = login;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult LoginUser([FromBody] Login user)
        {
            if (user == null)
            {
                return BadRequest("Invalid user request!!!");
            }

            if (_login.ValidateUser(user.UserName, user.Password, user.Role))
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSetting["JWT:Secret"]));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var tokenOptions = new JwtSecurityToken(
                    issuer: ConfigurationManager.AppSetting["JWT:ValidIssuer"],
                    audience: ConfigurationManager.AppSetting["JWT:ValidAudience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(60),
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                // Return token and role in response
                return Ok(new { Token = tokenString, Role = user.Role });
            }

            return Unauthorized();
        }
    }

    public class Login
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
    }

    public interface ILogin
    {
        bool ValidateUser(string uname, string pwd, string role);
    }

    public class LoginRepo : ILogin
    {
        private readonly CareerSolutionsDB _context;

        public LoginRepo(CareerSolutionsDB context)
        {
            _context = context;
        }

        public bool ValidateUser(string uname, string pwd, string role)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == uname);

            if (user != null && user.Password == pwd && user.Role == role)
            {
                return true;
            }

            return false;
        }
    
}
