using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightBookingSystem.Model.Flight;
using FlightBookingSystem.Repository.Entity;

namespace FlightBookingSystem.Interface
{
    public interface IFlightService
    {

        Task<IEnumerable<FlightEntity>> GetAllFlights();
        Task<FlightEntity> GetFlightById(int flightId);
        Task<FlightEntity> AddFlight(FlightDTO flight);
        Task<bool> UpdateFlight(int id, UpdateFlightDTO flight);
        Task<bool> DeleteFlight(int flightId);

    }
}