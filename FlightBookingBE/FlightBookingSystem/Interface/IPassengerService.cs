using System.Collections.Generic;
using System.Threading.Tasks;
using FlightBookingSystem.Repository.Entity;
using FlightBookingSystem.Model.Passenger;

namespace FlightBookingSystem.Service
{
    public interface IPassengerService
    {
        Task<IEnumerable<PassengerDTO>> GetAllPassengersAsync();
        Task<PassengerDTO> GetPassengerByIdAsync(int passengerId);
        Task AddPassengerAsync(PassengerDTO passenger);
        Task<bool> UpdatePassengerAsync(int id, UpdatePassengerDTO passenger);
        Task<bool> DeletePassengerAsync(int passengerId);
    }
}
