using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using TaskManagerApp.Data;
using TaskManagerApp.Models;
using TaskManagerApp.Services;

namespace TaskManagerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MongoDbContext _dbContext;
        private readonly JwtHelper _jwtHelper;

        public UserController(MongoDbContext dbContext, JwtHelper jwtHelper)
        {
            _dbContext = dbContext;
            _jwtHelper = jwtHelper;
        }

        // Register a new user
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            var existingUser = await _dbContext.Users.Find(u => u.Username == user.Username).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                return BadRequest("Username already exists");
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            await _dbContext.Users.InsertOneAsync(user);
            return Ok("User registered successfully");
        }

        // Login user and return JWT token
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            var existingUser = await _dbContext.Users.Find(u => u.Username == user.Username).FirstOrDefaultAsync();
            if (existingUser == null || !BCrypt.Net.BCrypt.Verify(user.PasswordHash, existingUser.PasswordHash))
            {
                return Unauthorized();
            }

            var token = _jwtHelper.GenerateToken(user.Username);
            return Ok(new { Token = token });
        }
    }
}
