using FlightBookingSystem.Model.User;
using FlightBookingSystem.Repository.Entity;

namespace FlightBookingSystem.Repository.Services
{
    public interface IUserRepository
    {
        Task<IEnumerable<GetUserDTO>> GetAllUsers();
        Task<GetUserDTO> GetUserById(int id);
        Task<GetUserByEmailDTO> GetUserByEmail(string email);
        Task CreateUser(UserDTO user);
        Task UpdateUser(int id, UpdateDTO user);
        Task DeleteUser(int id);
        Task<UserEntity> LogIn(LogInDTO user);
        Task<bool> ResetPassword(string email,string newPassword);
        Task<bool> ValidateEmail(string email);
    }
}
