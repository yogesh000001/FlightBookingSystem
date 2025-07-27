using FlightBookingSystem.Model.User;
using FlightBookingSystem.Service;
using FlightBookingSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;

namespace FlightBookingSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;
        private readonly EmailService _emailService;

        static string globalToken;

        public UsersController(
            IUserService userService,
            ILogger<UsersController> logger,
            EmailService emailService
        )
        {
            _userService = userService;
            _logger = logger;
            _emailService = emailService;
        }

        /// <summary>
        /// Fetches all users.
        /// Authorized for: Admin, Passenger
        /// </summary>
        /// <returns>List of all users.</returns>
        [HttpGet]
        [Authorize(Roles = "Admin, Passenger")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                _logger.LogInformation("Fetching all users.");
                var users = await _userService.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all users.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Fetches a user by ID.
        /// Authorized for: Admin, Passenger
        /// </summary>
        /// <param name="id">User ID.</param>
        /// <returns>User details.</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Passenger")]
        public async Task<IActionResult> GetUserByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching user with ID {id}.");
                var user = await _userService.GetUserById(id);
                if (user == null)
                {
                    _logger.LogWarning($"User with ID {id} not found.");
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching user with ID {id}.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="user">User details.</param>
        /// <returns>Confirmation message.</returns>
        [HttpPost("Register")]
        public async Task<IActionResult> CreateUser(UserDTO user)
        {
            try
            {
                if (user == null)
                {
                    _logger.LogError("UserDTO is null.");
                    return BadRequest("User data is null.");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("ModelState is invalid.");
                    return BadRequest(ModelState);
                }

                var validRoles = new[] { "Admin", "Passenger" };
                if (!validRoles.Contains(user.Role))
                {
                    _logger.LogError("Invalid role: {Role}", user.Role);
                    return BadRequest("Invalid role. Role must be either 'Admin' or 'Passenger'.");
                }

                _logger.LogInformation($"Creating new user with email {user.Email}.");
                await _emailService.SendEmailAsync(
                    user.Email,
                    "User Registration",
                    "User Register Successfully"
                );
                await _userService.CreateUser(user); 
                return Ok($"User created successfully with email: {user.Email}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new user.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates a user by ID.
        /// Authorized for: Admin, Passenger
        /// </summary>
        /// <param name="id">User ID.</param>
        /// <param name="user">Updated user details.</param>
        /// <returns>No content.</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Passenger")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateDTO user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest();
                }
                _logger.LogInformation($"Updating user with ID {id}.");
                var existingUser = await _userService.GetUserById(id);
                if (existingUser == null)
                {
                    _logger.LogWarning($"User with ID {id} not found.");
                    return NotFound();
                }
                await _userService.UpdateUser(id, user);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating user with ID {id}.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a user by ID.
        /// Authorized for: Admin
        /// </summary>
        /// <param name="id">User ID.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting user with ID {id}.");
                var user = await _userService.GetUserById(id);
                if (user == null)
                {
                    _logger.LogWarning($"User with ID {id} not found.");
                    return NotFound();
                }
                await _userService.DeleteUser(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting user with ID {id}.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="userLogin">Login details.</param>
        /// <returns>JWT token.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> LogIn(LogInDTO userLogin)
        {
            try
            {
                if (
                    userLogin == null
                    || string.IsNullOrEmpty(userLogin.Email)
                    || string.IsNullOrEmpty(userLogin.Password)
                )
                {
                    return BadRequest("Invalid login request.");
                }
                _logger.LogInformation($"User login attempt with email {userLogin.Email}.");
                var result = await _userService.LogIn(userLogin);
                if (result == null)
                {
                    _logger.LogWarning("Login failed. Invalid email or password.");
                    return Unauthorized("Invalid email or password.");
                }
                var message = $"Login successful for user: {result.Email}";
                var user = await _userService.GetUserByEmail(userLogin.Email);
                var tokenService = HttpContext.RequestServices.GetRequiredService<TokenService>();
                string token = tokenService.GenerateToken(user.Role, user.Id);

                return Ok(new {Success=true, Message = message, Token = token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during user login.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Initiates the password reset process for a user.
        /// </summary>
        /// <param name="request">Forgot password request details.</param>
        /// <returns>Confirmation message.</returns>
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel request)
        {
            _logger.LogInformation(
                "ForgotPassword request received for email: {Email}",
                request.Email
            );
            try
            {
                if (string.IsNullOrEmpty(request.Email))
                {
                    _logger.LogWarning("ForgotPassword failed: Email is null or empty");
                    return BadRequest("Email is required.");
                }

                bool validEmail = await _userService.ValidateEmail(request.Email);
                if(!validEmail){
                    _logger.LogInformation("ForgotPassword failed: Invalid email - {Email}",request.Email);
                    return BadRequest("Invalid email.");
                }

                var tokenService = HttpContext.RequestServices.GetRequiredService<TokenService>();
                string resetToken = tokenService.GenerateResetToken(request.Email);
                globalToken=resetToken;

                var message = new
                {
                    To = request.Email,
                    Subject = "Reset Your Password",
                    Body = $"Click the link to reset your password: https://yourdomain.com/reset-password?token={resetToken}",
                };

                await _emailService.SendEmailAsync(message.To, message.Subject, message.Body);
                return Ok("Password reset email has been sent.");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while processing the password reset request."
                );
                return BadRequest(
                    new
                    {
                        message = "Error occurred while processing the password reset",
                        error = ex.Message,
                    }
                );
            }
        }

        /// <summary>
        /// Resets the user's password.
        /// </summary>
        /// <param name="model">Reset password details.</param>
        /// <returns>Confirmation message.</returns>
        [HttpPut("ResetPassword")]
#pragma warning restore ASP0018 // Unused route parameter
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            try
            {

                var token = HttpContext.Request.Query["token"].ToString();

                token = globalToken;
                // globalToken="";
                if(globalToken == ""){
                    return BadRequest("your token is expired, recreate it");
                }
                if(token == null){
                    return BadRequest("enter a valid token");
                }


                var tokenService = HttpContext.RequestServices.GetRequiredService<TokenService>();
                var tokenEmail=tokenService.ValidateResetToken(token);
                if (model.NewPassword != model.ConfirmPassword)
                {
                    _logger.LogWarning(
                        "Reset Password failed: Passwords do not match for email: {Email}",
                        model.Email
                    );
                    return BadRequest("Passwords do not match.");
                }

                bool result = await _userService.ResetPassword(tokenEmail, model.NewPassword);
                if (result)
                {
                    var message = new
                    {
                        To = model.Email,
                        Subject = "FlightBookingSystem Security Alert",
                        Body = $"Your password has been changed recently. If this wasn't you, please change it immediately.",
                    };
                    await _emailService.SendEmailAsync(message.To, message.Subject, message.Body);
                    _logger.LogInformation(
                        "Password reset successful for email: {Email}",
                        model.Email
                    );
                    return Ok("Password reset successful.");
                }

                _logger.LogWarning(
                    "ResetPassword failed: Could not reset password for email: {Email}",
                    model.Email
                );
                return BadRequest("Error occurred during password reset.");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Exception occurred during ResetPassword for email: {Email}",
                    model.Email
                );
                return BadRequest("An error occurred while resetting the password.");
            }
        }
    }
}
