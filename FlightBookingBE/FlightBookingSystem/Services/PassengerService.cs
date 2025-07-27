using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightBookingSystem.Model.Passenger;
using FlightBookingSystem.Repository;
using FlightBookingSystem.Repository.Entity;
using FlightBookingSystem.Repository.Interface;
using Microsoft.Extensions.Logging;

namespace FlightBookingSystem.Service
{
    public class PassengerService : IPassengerService
    {
        private readonly IPassengerRepository _passengerRepository;
        private readonly ILogger<PassengerService> _logger;

        public PassengerService(
            IPassengerRepository passengerRepository,
            ILogger<PassengerService> logger
        )
        {
            _passengerRepository = passengerRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<PassengerDTO>> GetAllPassengersAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all passengers.");
                var passengers = await _passengerRepository.GetAllPassengers();
                return passengers
                    .Select(p => new PassengerDTO
                    {
                        Name = p.Name,
                        UserID = p.BookedByUser,
                        ContactDetails = p.ContactDetails,
                        Code = p.Code,
                        Gender = p.Gender,
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all passengers.");
                throw;
            }
        }

        public async Task<PassengerDTO> GetPassengerByIdAsync(int passengerId)
        {
            try
            {
                _logger.LogInformation($"Fetching passenger with ID {passengerId}.");
                var passenger = await _passengerRepository.GetPassengerById(passengerId);
                if (passenger == null)
                {
                    _logger.LogWarning($"Passenger with ID {passengerId} not found.");
                    return null;
                }
                return passenger;
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

        public async Task AddPassengerAsync(PassengerDTO passenger)
        {
            try
            {
                _logger.LogInformation($"Adding new passenger with user ID {passenger.UserID}.");
                await _passengerRepository.AddPassenger(passenger);
                _logger.LogInformation("New passenger added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new passenger.");
                throw;
            }
        }

        public async Task<bool> UpdatePassengerAsync(int id, UpdatePassengerDTO passenger)
        {
            try
            {
                _logger.LogInformation($"Updating passenger with ID {id}.");
                return await _passengerRepository.UpdatePassenger(id, passenger);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating passenger with ID {id}.");
                throw;
            }
        }

        public async Task<bool> DeletePassengerAsync(int passengerId)
        {
            try
            {
                _logger.LogInformation($"Deleting passenger with ID {passengerId}.");
                return await _passengerRepository.DeletePassenger(passengerId);
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