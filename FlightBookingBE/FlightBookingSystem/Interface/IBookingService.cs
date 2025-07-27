using System.Collections.Generic;
using System.Threading.Tasks;
using FlightBookingSystem.Model.Booking;
using FlightBookingSystem.Repository.Entity;

namespace FlightBookingSystem.Service.Interface
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingSendDTO>> GetAllBookingsAsync();
        Task<BookingSendDTO> GetBookingByIdAsync(int bookingId);
        Task<BookingSendDTO> AddBookingAsync(BookingDTO booking,string id);
        Task<bool> ConfirmBooking(int bookingId);
    }
}
