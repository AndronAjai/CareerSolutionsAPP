using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CSAPI.Models;
using Microsoft.AspNetCore.Authorization;



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

            if (_login.ValidateUser(user.UserName, user.Password, user.Role,out int uid))
            {

                //addednow
                var cookieOptions = new CookieOptions
                {
>
                    Expires = DateTimeOffset.UtcNow.AddMinutes(30), // Set cookie expiration time
                    HttpOnly = true, // Make the cookie inaccessible to JavaScript
                    Secure = true, // Only send cookie over HTTPS
                    SameSite = SameSiteMode.Strict // Enforce SameSite policy

                };

                // Add the UserId cookie
                Response.Cookies.Append("UserId", uid.ToString(), cookieOptions);
                //addednow--

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
        bool ValidateUser(string uname, string pwd, string role,out int uid);
    }

    public class LoginRepo : ILogin
    {
        private readonly CareerSolutionsDB _context;

        public LoginRepo(CareerSolutionsDB context)
        {
            _context = context;
        }

        public bool ValidateUser(string uname, string pwd, string role,out int uid)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == uname);

            uid = 0;

            if (user != null && BCrypt.Net.BCrypt.Verify(pwd, user.Password) && user.Role == role)
            {
                uid = user.UserID;
                return true;
            }
            return false;
        }
    }
}
