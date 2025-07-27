using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightBookingSystem.Model.Flight;
using FlightBookingSystem.Repository.Entity;

namespace FlightBookingSystem.Repository.Interface
{
    public interface IFlightRepository
    {
        Task<IEnumerable<FlightEntity>> GetAllFlights();
        Task<FlightEntity> GetFlightById(int passengerId);
        Task<FlightEntity> AddFlight(FlightDTO passenger);

        Task<bool> UpdateFlight(int id, UpdateFlightDTO flight);

        Task<bool> DeleteFlight(int flightId);
    }
}