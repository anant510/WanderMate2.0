using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using usingLinq.Context;
using usingLinq.Dtos;
using usingLinq.Models;

namespace usingLinq.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Get() 
        {
            var users = _context.Users.ToList();
            return Ok(users);
        }

         [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            // Retrieve user data from the session
            var userName = HttpContext.Session.GetString("UserName");
            var userRole = HttpContext.Session.GetString("UserRole");

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userRole))
            {
                return Unauthorized("User data not found in session");
            }

            return Ok(new { UserName = userName, UserRole = userRole });
        }

        [HttpPost]
        public IActionResult Create([FromBody] UserDto model)
        {
            var user = new User
            {
                Name = model.Name,
                Role = model.Role,
                Username = model.Username,
                Password = model.Password,
            };
            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok("User created successfully");

        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Clear the session data
            HttpContext.Session.Clear();
            return Ok(new { message = "Logged out successfully" });
        }
        
    }
}
