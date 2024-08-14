using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using usingLinq.Context;
using usingLinq.Dtos;
using usingLinq.Models;
using usingLinq.Service;

namespace usingLinq.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly TokenService _tokenService;

        public AuthController(ApplicationDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> UserLogin([FromBody] LoginDto signInDTO)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == signInDTO.Username);

                if (user == null)
                {
                    return BadRequest("Username does not exist.");
                }

                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(signInDTO.Password, user.Password);
                if (!isPasswordValid)
                {
                    return BadRequest("Password is incorrect.");
                }

                // Generate JWT token
                var token = _tokenService.GenerateToken(user, isPasswordValid);

                // Store token and user data in the session
                    HttpContext.Session.SetString("AuthToken", token);
                    HttpContext.Session.SetString("Id", user.Id.ToString());
                    HttpContext.Session.SetString("UserName", user.Name);
                    HttpContext.Session.SetString("UserRole", user.Role);

                return Ok(new { Message = "User signed in successfully!", Token = token });

                // return Ok("User signed in successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }



        }

        // [HttpPost("login")]
        // public IActionResult Login([FromBody] LoginDto model)
        // {
        //     var user = _context.Users.SingleOrDefault(u => u.Username == model.Username && u.Password == model.Password);



        //     if (user == null)
        //     {
        //         return Unauthorized();
        //     }

        //     var token = _tokenService.GenerateToken(user);

        //     // Store the token and user data in the session
        //     HttpContext.Session.SetString("AuthToken", token);
        //     HttpContext.Session.SetString("Id", user.Id.ToString());
        //     HttpContext.Session.SetString("UserName", user.Name);
        //     HttpContext.Session.SetString("UserRole", user.Role);

        //     return Ok(new { token });
        // }
    }
}
