using System;
using System.Collections.Generic;
using System.Linq;
using FlightBookingSystem.Repository.Entity;
using System.Threading.Tasks;
using FlightBookingSystem.Model.Passenger;

namespace FlightBookingSystem.Repository.Interface
{
    public interface IPassengerRepository
    {


        Task<IEnumerable<PassengerEntity>> GetAllPassengers();
        Task<PassengerDTO> GetPassengerById(int passengerId);
        Task AddPassenger(PassengerDTO passenger);

        Task<bool> UpdatePassenger(int id, UpdatePassengerDTO passenger);

        Task<bool> DeletePassenger(int passengerId);

    }

}