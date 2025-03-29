using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using task_management_app.Data;
using task_management_app.Models;
using task_management_app.Services;

namespace task_management_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly MongoDbContext _dbContext;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;
        public UsersController(MongoDbContext dbContext, IPasswordService passwordService, ITokenService tokenService)
        {
            _dbContext = dbContext;
            _passwordService = passwordService;
            _tokenService = tokenService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDto signUpDto)
        {
            var existingUser = await _dbContext.Users.Find(u => u.Username == signUpDto.Username).FirstOrDefaultAsync();

            if (existingUser != null)
            {
                return BadRequest(new { error = "This user is already taken!" });
            }

            var hashedPassword = _passwordService.HashPassword(signUpDto.Password);

            var user = new User
            {
                Username = signUpDto.Username,
                PasswordHash = hashedPassword
            };

            await _dbContext.Users.InsertOneAsync(user);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {

            var user = await _dbContext.Users.Find(u => u.Username == loginDto.Username).FirstOrDefaultAsync();

            if (user == null)
            {
                return Unauthorized(new { error = "Invalid credentials!" });
            }

            var isValid = _passwordService.VerifyPassword(user.PasswordHash, loginDto.Password);
            if (!isValid)
            {
                return Unauthorized(new { error = "Invalid credentials!" });
            }

            var token = _tokenService.GenerateToken(user);

            return Ok(new { token });
        }
    }
}
