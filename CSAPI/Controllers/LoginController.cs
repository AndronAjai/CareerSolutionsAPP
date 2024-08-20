using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CSAPI.Models;

namespace CSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogin _login;

        public LoginController(ILogin login)
        {
            _login = login;
        }

        [HttpPost("login")]
        public IActionResult LoginUser([FromBody] Login user)
        {
            if (user is null)
            {
                return BadRequest("Invalid user request!!!");
            }

            if (_login.ValidateUser(user.UserName, user.Password, user.Role))
            {
                // Create the security key and credentials
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSetting["JWT:Secret"]));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                // Add user-specific claims, including the role
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                // Create the token
                var tokenOptions = new JwtSecurityToken(
                    issuer: ConfigurationManager.AppSetting["JWT:ValidIssuer"],
                    audience: ConfigurationManager.AppSetting["JWT:ValidAudience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(60),  // You can adjust the expiration as needed
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

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
}
