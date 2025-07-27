using FlightBookingSystem.Model.Booking;
using FlightBookingSystem.Repository.Entity;

namespace FlightBookingSystem.Repository.Interface
{
    public interface IBookingRepository
    {
        Task<IEnumerable<BookingSendDTO>> GetAllBookingsAsync();
        Task<BookingSendDTO> GetBookingByIdAsync(int bookingId);
        Task<BookingSendDTO> AddBookingAsync(BookingDTO booking,string id);
        Task<bool> ConfirmBooking(int bookingId);
    }
}