using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightBookingSystem.Model.User;
using FlightBookingSystem.Repository.Entity;
using FlightBookingSystem.Repository.Services;
using Microsoft.Extensions.Logging;

namespace FlightBookingSystem.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<GetUserDTO>> GetAllUsers()
        {
            try
            {
                _logger.LogInformation("Fetching all users.");
                var users = await _userRepository.GetAllUsers();
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
                var user = await _userRepository.GetUserById(id);
                if (user == null)
                {
                    _logger.LogWarning($"User with ID {id} not found.");
                    return null;
                }
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching user with ID {id}.");
                throw new Exception($"Error retrieving user with ID {id}", ex);
            }
        }
//token
        public async Task<GetUserByEmailDTO> GetUserByEmail(string email)
        {
            try
            {
                _logger.LogInformation($"Fetching user with Email {email}.");
                var user = await _userRepository.GetUserByEmail(email);
                if (user == null)
                {
                    _logger.LogWarning($"User with Email {email} not found.");
                    return null;
                }
                return user;
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
                await _userRepository.CreateUser(user);
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
                await _userRepository.UpdateUser(id, user);
                _logger.LogInformation("User updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating user with ID {id}.");
                throw new Exception("Error updating user", ex);
            }
        }

        public async Task DeleteUser(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting user with ID {id}.");
                await _userRepository.DeleteUser(id);
                _logger.LogInformation("User deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting user with ID {id}.");
                throw new Exception($"Error deleting user with ID {id}", ex);
            }
        }

        public async Task<UserDTO> LogIn(LogInDTO user)
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

                var existingUser = await _userRepository.LogIn(user);
                if (existingUser == null)
                {
                    _logger.LogWarning("Login failed. Invalid email or password.");
                    return null;
                }

                _logger.LogInformation("User logged in successfully.");
                return new UserDTO
                {
                    FirstName = existingUser.FirstName,
                    LastName = existingUser.LastName,
                    Email = existingUser.Email,
                    PhoneNo = existingUser.PhoneNo,
                    Role = existingUser.Role,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during user login.");
                throw;
            }
        }
        public async Task<bool> ResetPassword(string email,string newPassword){
            _logger.LogInformation("ResetPassword method called");
            return await _userRepository.ResetPassword(email,newPassword);
        }
        public async Task<bool> ValidateEmail(string email){
            return await _userRepository.ValidateEmail(email);
        }
    }
}
