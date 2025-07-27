using System.Collections.Generic;
using System.Threading.Tasks;
using FlightBookingSystem.Interface;
using FlightBookingSystem.Model.Flight;
using FlightBookingSystem.Repository.Entity;
using FlightBookingSystem.Repository.Interface;
using Microsoft.Extensions.Logging;

namespace FlightBookingSystem.Business
{
    public class FlightService : IFlightService
    {
        private readonly IFlightRepository _flightRepository;
        private readonly ILogger<FlightService> _logger;

        public FlightService(IFlightRepository flightRepository, ILogger<FlightService> logger)
        {
            _flightRepository = flightRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<FlightEntity>> GetAllFlights()
        {
            try
            {
                _logger.LogInformation("Fetching all flights.");
                return await _flightRepository.GetAllFlights();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all flights.");
                throw;
            }
        }

        public async Task<FlightEntity> GetFlightById(int flightId)
        {
            try
            {
                _logger.LogInformation($"Fetching flight with ID {flightId}.");
                return await _flightRepository.GetFlightById(flightId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    $"An error occurred while fetching flight with ID {flightId}."
                );
                throw;
            }
        }

        public async Task<FlightEntity> AddFlight(FlightDTO flight)
        {
            try
            {
                _logger.LogInformation($"Adding new flight with number {flight.FlightNumber}.");
                return await _flightRepository.AddFlight(flight);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new flight.");
                throw;
            }
        }

        public async Task<bool> UpdateFlight(int id, UpdateFlightDTO flight)
        {
            try
            {
                _logger.LogInformation($"Updating flight with ID {id}.");
                return await _flightRepository.UpdateFlight(id, flight);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating flight with ID {id}.");
                throw;
            }
        }

        public async Task<bool> DeleteFlight(int flightId)
        {
            try
            {
                _logger.LogInformation($"Deleting flight with ID {flightId}.");
                return await _flightRepository.DeleteFlight(flightId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    $"An error occurred while deleting flight with ID {flightId}."
                );
                throw;
            }
        }
    }
}
