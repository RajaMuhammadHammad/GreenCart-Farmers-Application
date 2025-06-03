    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using GreenCart.API.Models;
    using Microsoft.EntityFrameworkCore;

    namespace GreenCart.API.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class AuthController : ControllerBase
        {
            private readonly GreenCartDbContext _context;
            private readonly IConfiguration _config;

            public AuthController(GreenCartDbContext context, IConfiguration config)
            {
                _context = context;
                _config = config;
            }

            [HttpPost("register")]
            public async Task<IActionResult> Register(RegisterModel model)
            {
                // Check if username already exists
                if (_context.Users.Any(u => u.UserName == model.UserName))
                    return BadRequest("Username already exists");

                // Create a new user object with all fields including Email
                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,                     // ✅ Email field added
                    PasswordHash = model.PasswordHash,       // 🔐 You should hash this in production
                    Role = model.Role,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Ok("Registered successfully");
            }

            [HttpPost("login")]
            public async Task<IActionResult> Login(LoginModel model)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserName == model.UserName && u.PasswordHash == model.PasswordHash);

                if (user == null)
                    return Unauthorized();

                var token = GenerateJwtToken(user);

                // Return token AND role at the root level of the JSON response
                return Ok(new
                {
                    token = token,
                    role = user.Role
                });
            }

            private string GenerateJwtToken(User user)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(Convert.ToDouble(_config["Jwt:DurationInMinutes"])),
                    signingCredentials: creds);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }
    }
