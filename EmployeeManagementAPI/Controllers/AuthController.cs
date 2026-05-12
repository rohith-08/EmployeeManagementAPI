using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagementAPI.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;
namespace EmployeeManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;

        private readonly List<(string Username, string Password, string Role)> _users = new()
        {
            ("admin", "admin123", "Admin"),
            ("hrmanager", "hr123", "HR"),
            ("viewer", "view123", "Viewer")

        };
        public AuthController(IConfiguration configuration,ILogger<AuthController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            var user = _users.FirstOrDefault(u =>
                u.Username == dto.Username &&
                u.Password == dto.Password);

            if (user == default)
            {
                _logger.LogWarning("Failed login attempt for username: {Username}", dto.Username);
                return Unauthorized(new { message = "Invalid username or password" });
            }

                _logger.LogInformation(
               "Successful login — User: {Username} Role: {Role}",
                  user.Username,
                     user.Role);
            var token = GenerateToken(user.Username, user.Role);
            return Ok(new { token, role = user.Role });
        }

            private string GenerateToken(string username,string role)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"]!;
            var issuer = jwtSettings["Issuer"]!;
            var audience = jwtSettings["Audience"]!;
            var expiry = int.Parse(jwtSettings["ExpiryMinutes"]!);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiry),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }


    }
    }
