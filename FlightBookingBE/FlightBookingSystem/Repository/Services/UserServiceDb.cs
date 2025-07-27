using FlightBookingSystem.Model.User;
using FlightBookingSystem.Repository.Context;
using FlightBookingSystem.Repository.Entity;
using Microsoft.EntityFrameworkCore;

namespace FlightBookingSystem.Repository.Services
{
    public class UserServiceDb : IUserRepository
    {
        private readonly FlightContext _context;
        private readonly ILogger<UserServiceDb> _logger;

        public UserServiceDb(FlightContext context, ILogger<UserServiceDb> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<GetUserDTO>> GetAllUsers()
        {
            try
            {
                _logger.LogInformation("Fetching all users.");
                var users = await _context.Users.ToListAsync();
                return users
                    .Select(u => new GetUserDTO
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Email = u.Email,
                        PhoneNo = u.PhoneNo,
                        Role = u.Role,
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all users.");
                throw new Exception("Error retrieving users", ex);
            }
        }

        public async Task<GetUserDTO> GetUserById(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching user with ID {id}.");
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    _logger.LogWarning($"User with ID {id} not found.");
                    return null;
                }
                return new GetUserDTO
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNo = user.PhoneNo,
                    Role = user.Role,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching user with ID {id}.");
                throw new Exception($"Error retrieving user with ID {id}", ex);
            }
        }

        public async Task<GetUserByEmailDTO> GetUserByEmail(string email)
        {
            try
            {
                _logger.LogInformation($"Fetching user with Email {email}.");
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    _logger.LogWarning($"User with Email {email} not found.");
                    return null;
                }
                return new GetUserByEmailDTO
                {
                    Id = user.UserId,
                    Name = user.FirstName + " " + user.LastName,
                    PhoneNo = user.PhoneNo,
                    Role = user.Role,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching user with Email {email}.");
                throw new Exception($"Error retrieving user with Email {email}", ex);
            }
        }

        public async Task CreateUser(UserDTO user)
        {
            try
            {
                _logger.LogInformation($"Creating new user with email {user.Email}.");
                var existingUser = await _context.Users.FirstOrDefaultAsync(u =>
                    u.Email == user.Email
                );
                if (existingUser != null)
                {
                    _logger.LogError("A user with this email already exists.");
                    throw new Exception("A user with this email already exists.");
                }
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
                var newUser = new UserEntity
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Password = hashedPassword,
                    PhoneNo = user.PhoneNo,
                    Role = user.Role,
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
                _logger.LogInformation("New user created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new user.");
                throw;
            }
        }

        public async Task UpdateUser(int id, UpdateDTO user)
        {
            try
            {
                _logger.LogInformation($"Updating user with ID {id}.");
                var existingUser = await _context.Users.FindAsync(id);
                if (existingUser != null)
                {
                    existingUser.FirstName = user.FirstName;
                    existingUser.LastName = user.LastName;
                    existingUser.Email = user.Email;
                    existingUser.PhoneNo = user.PhoneNo;
                    _context.Users.Update(existingUser);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("User updated successfully.");
                }
                else
                {
                    _logger.LogWarning($"User with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating user.");
                throw new Exception("Error updating user", ex);
            }
        }

        public async Task DeleteUser(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting user with ID {id}.");
                var user = await _context.Users.FindAsync(id);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("User deleted successfully.");
                }
                else
                {
                    _logger.LogWarning($"User with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting user with ID {id}.");
                throw new Exception($"Error deleting user with ID {id}", ex);
            }
        }

        public async Task<UserEntity> LogIn(LogInDTO user)
        {
            try
            {
                _logger.LogInformation($"User login attempt with email {user.Email}.");
                if (
                    user == null
                    || string.IsNullOrEmpty(user.Email)
                    || string.IsNullOrEmpty(user.Password)
                )
                {
                    _logger.LogWarning("Invalid login attempt.");
                    return null;
                }

                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (
                    existingUser == null
                    || !BCrypt.Net.BCrypt.Verify(user.Password, existingUser.Password)
                )
                {
                    _logger.LogWarning("Login failed. Invalid email or password.");
                    return null;
                }

                _logger.LogInformation("User logged in successfully.");
                return existingUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during user login.");
                throw;
            }
        }
        public async Task<bool> ResetPassword(string email, string newPassword)
        {
            _logger.LogInformation($"Resetting password for user with email: {email}");
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null)
            {
                _logger.LogInformation($"Password reset successfully for user with email: {email}");
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
                user.Password = hashedPassword;
                await _context.SaveChangesAsync();
                return true;
            }
            _logger.LogWarning($"Invalid email received for password reset: {email}");
            return false;
        }

        public async Task<bool> ValidateEmail(string email){
            var result = await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
            Console.WriteLine(result);
            if(result == null){
                return false;
            }
            return true;
        }
    }
}
