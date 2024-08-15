using System.Security.Principal;
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
        private readonly EmailService _emailService;

        public UserController(ApplicationDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }
        // [Authorize(Roles = "Admin")]

        [HttpGet]
        public IActionResult Get() 
        {
            var users = _context.Users.ToList();
            return Ok(users);
        }

         [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            // Retrieve user data from the session
            var Id = HttpContext.Session.GetString("Id");
            var userName = HttpContext.Session.GetString("UserName");
            var userRole = HttpContext.Session.GetString("UserRole");

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userRole) || string.IsNullOrEmpty(Id) )
            {
                return Unauthorized("User data not found in session");
            }

            return Ok(new { UserName = userName, UserRole = userRole, Id = Id });
        }

        [HttpPost]
        public IActionResult Create([FromBody] UserDto model)
        {

             var HashPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
            

            var user = new User
            {
                Name = model.Name,
                Role = model.Role,
                Username = model.Username,
                Password = HashPassword,
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

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO model)
        {
            // Find the user by email (which is used as the Username)
            var user = _context.Users.FirstOrDefault(u => u.Username == model.Username);
            if (user == null)
                return BadRequest("User not found.");

            // Simulate generating a password reset token (In reality, you'd generate a secure, unique token)
            var resetToken = Guid.NewGuid().ToString();   // Generate a secure token

            // Store the reset token with the user's information
            _context.PasswordResets.Add(new PasswordReset { Token = resetToken });
            await _context.SaveChangesAsync();

            var emailBody = $"This is your Reset Token: {resetToken}";
            await _emailService.SendEmailAsync(user.Username, "Password Reset", emailBody);
    
            // Construct reset URL
            // var resetLink = Url.Action("ResetPassword",
            //     "Account",  // Controller name
            //     new { token = resetToken, email = user.Username },  // Query parameters
            //     Request.Scheme);  // Scheme (http or https)

            // var emailBody = $"This is your Reset Token: {resetToken}";

            // // Store the reset token with the user's information in a secure way (e.g., in a database)
            // _context.PasswordResets.Add(new PasswordReset { Token = resetToken });
            // await _context.SaveChangesAsync();

             // Construct the email body
            // var emailBody = $"This is your Reset Token: {resetToken}";
             // Send the email
            // await _emailService.SendEmailAsync(user.Username, "Password Reset", emailBody);

            // Send the reset link via email
            // var emailBody = $"Please reset your password by clicking here: <a href='{resetLink}'>Reset Password</a>";
            // await _emailService.SendEmailAsync(user.Username, "Password Reset", emailBody);

            return Ok("If an account with that email exists, a password reset link has been sent.");
        }


        [HttpPost("update-password")]

        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePassword model)
        {
            var verifiedToken = _context.PasswordResets.FirstOrDefault(u => u.Token == model.Token);

            if (verifiedToken == null)
            {
                return BadRequest("Invalid or expired token.");
            }

             // Find the user associated with the reset token
            var user = _context.Users.FirstOrDefault(u => u.Username == model.Username);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            // Hash the new password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Update_Password);

            // Update the user's password
            user.Password = hashedPassword;
            _context.Users.Update(user);

             // Save changes to the database
            await _context.SaveChangesAsync();

             return Ok("Updated Sucessfully");

        }



        
    }
}
