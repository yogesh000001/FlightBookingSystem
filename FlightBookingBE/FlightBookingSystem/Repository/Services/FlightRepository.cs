using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightBookingSystem.Model.Flight;
using FlightBookingSystem.Repository.Context;
using FlightBookingSystem.Repository.Entity;
using FlightBookingSystem.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlightBookingSystem.Repository
{
    public class FlightRepository : IFlightRepository
    {
        private readonly FlightContext _context;
        private readonly ILogger<FlightRepository> _logger;

        public FlightRepository(FlightContext context, ILogger<FlightRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<FlightEntity>> GetAllFlights()
        {
            try
            {
                _logger.LogInformation("Fetching all flights.");
                var flights = await _context.Flights.ToListAsync();
                if (flights == null || !flights.Any())
                {
                    _logger.LogWarning("No flights found.");
                    return new List<FlightEntity>();
                }
                return flights;
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
                var flight = await _context.Flights.FindAsync(flightId);
                if (flight == null)
                {
                    _logger.LogWarning($"Flight with ID {flightId} not found.");
                    return null;
                }
                return flight;
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

                // Check if the flight number already exists in the database
                var flightExists = await _context.Flights.AnyAsync(f =>
                    f.FlightNumber == flight.FlightNumber
                );
                if (flightExists)
                {
                    _logger.LogError($"Flight number {flight.FlightNumber} is already present.");
                    throw new InvalidOperationException(
                        $"Flight number {flight.FlightNumber} is already present."
                    );
                }

                if (flight.DepartureTime >= flight.ArrivalTime)
                {
                    _logger.LogError("Departure time must be earlier than arrival time.");
                    throw new ArgumentException(
                        "Departure time must be earlier than arrival time."
                    );
                }

                if (flight.Origin == flight.Destination)
                {
                    _logger.LogError("Origin and destination cannot be the same.");
                    throw new ArgumentException("Origin and destination cannot be the same.");
                }

                var availableSeats = Enumerable
                    .Range(1, flight.TotalSeats)
                    .Select(seat => seat.ToString())
                    .ToList();

                var flightEntity = new FlightEntity
                {
                    FlightNumber = flight.FlightNumber,
                    DepartureTime = flight.DepartureTime,
                    ArrivalTime = flight.ArrivalTime,
                    Origin = flight.Origin,
                    Destination = flight.Destination,
                    Status = flight.Status,
                    TotalSeats = flight.TotalSeats,
                    AvailableSeats = availableSeats,
                };

                _context.Flights.Add(flightEntity);
                await _context.SaveChangesAsync();
                _logger.LogInformation("New flight added successfully.");
                return flightEntity;
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
                var existingFlight = await _context.Flights.FindAsync(id);
                if (existingFlight == null)
                {
                    _logger.LogWarning($"Flight with ID {id} not found.");
                    return false;
                }

                if (flight.DepartureTime > flight.ArrivalTime)
                {
                    _logger.LogError("Departure time must be earlier than arrival time.");
                    throw new ArgumentException(
                        "Departure time must be earlier than arrival time."
                    );
                }

                existingFlight.DepartureTime = flight.DepartureTime;
                existingFlight.ArrivalTime = flight.ArrivalTime;
                existingFlight.Origin = flight.Origin;
                existingFlight.Destination = flight.Destination;
                existingFlight.Status = flight.Status;
                existingFlight.TotalSeats = flight.TotalSeats;

                _context.Flights.Update(existingFlight);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Flight updated successfully.");
                return true;
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
                var flight = await _context.Flights.FindAsync(flightId);
                if (flight == null)
                {
                    _logger.LogWarning($"Flight with ID {flightId} not found.");
                    return false;
                }

                _context.Flights.Remove(flight);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Flight deleted successfully.");
                return true;
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
