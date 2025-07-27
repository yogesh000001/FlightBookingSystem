using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightBookingSystem.Model.Passenger;
using FlightBookingSystem.Repository.Context;
using FlightBookingSystem.Repository.Entity;
using FlightBookingSystem.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlightBookingSystem.Repository
{
    public class PassengerRepository : IPassengerRepository
    {
        private readonly FlightContext _context;
        private readonly ILogger<PassengerRepository> _logger;

        public PassengerRepository(FlightContext context, ILogger<PassengerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<PassengerEntity>> GetAllPassengers()
        {
            try
            {
                _logger.LogInformation("Fetching all passengers.");
                return await _context.Passengers.Include(p => p.User).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all passengers.");
                throw;
            }
        }

        public async Task<PassengerDTO> GetPassengerById(int passengerId)
        {
            try
            {
                _logger.LogInformation($"Fetching passenger with ID {passengerId}.");
                var passenger = await _context
                    .Passengers.Include(p => p.User)
                    .FirstOrDefaultAsync(p => p.PassengerID == passengerId);
                if (passenger == null)
                {
                    _logger.LogWarning($"Passenger with ID {passengerId} not found.");
                    return null;
                }

                var passengerDTO = new PassengerDTO
                {
                    Name = passenger.Name,
                    UserID = passenger.User.UserId,
                    ContactDetails = passenger.ContactDetails,
                    Code = passenger.Code,
                    Gender = passenger.Gender,
                };

                return passengerDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    $"An error occurred while fetching passenger with ID {passengerId}."
                );
                throw;
            }
        }

        public async Task AddPassenger(PassengerDTO passenger)
        {
            try
            {
                _logger.LogInformation($"Adding new passenger with user ID {passenger.UserID}.");
                var userExists = await _context.Users.FindAsync(passenger.UserID);
                if (userExists == null)
                {
                    _logger.LogError($"User with ID {passenger.UserID} not found.");
                    throw new KeyNotFoundException($"User with ID {passenger.UserID} not found.");
                }

                if (userExists.Role == "Admin")
                {
                    _logger.LogError($"User with ID {passenger.UserID} is Admin");
                    throw new InvalidOperationException("Operation not allowed for Admin users.");
                }

                var codeExists = await _context.Passengers.AnyAsync(p => p.Code == passenger.Code);
                if (codeExists)
                {
                    _logger.LogError($"Code {passenger.Code} is already present.");
                    throw new InvalidOperationException(
                        $"Code {passenger.Code} is already present."
                    );
                }

                var newPassenger = new PassengerEntity
                {
                    Name = passenger.Name,
                    BookedByUser = passenger.UserID,
                    ContactDetails = passenger.ContactDetails,
                    Code = passenger.Code,
                    Gender = passenger.Gender,
                };
                await _context.Passengers.AddAsync(newPassenger);
                await _context.SaveChangesAsync();
                _logger.LogInformation("New passenger added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new passenger.");
                throw;
            }
        }

        public async Task<bool> UpdatePassenger(int id, UpdatePassengerDTO passenger)
        {
            try
            {
                _logger.LogInformation($"Updating passenger with ID {id}.");
                var item = await _context.Passengers.FindAsync(id);
                if (item == null)
                {
                    _logger.LogWarning($"Passenger with ID {id} not found.");
                    return false;
                }

                item.ContactDetails = passenger.ContactDetails;
                item.Gender = passenger.Gender;
                await _context.SaveChangesAsync();
                _logger.LogInformation("Passenger updated successfully.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating passenger with ID {id}.");
                throw;
            }
        }

        public async Task<bool> DeletePassenger(int passengerId)
        {
            try
            {
                _logger.LogInformation($"Deleting passenger with ID {passengerId}.");
                var passenger = await _context.Passengers.FindAsync(passengerId);
                if (passenger == null)
                {
                    _logger.LogWarning($"Passenger with ID {passengerId} not found.");
                    return false;
                }

                _context.Passengers.Remove(passenger);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Passenger deleted successfully.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    $"An error occurred while deleting passenger with ID {passengerId}."
                );
                throw;
            }
        }
    }
}
