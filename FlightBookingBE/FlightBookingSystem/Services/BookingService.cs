using FlightBookingSystem.Model.Booking;
using FlightBookingSystem.Repository.Entity;
using FlightBookingSystem.Repository.Interface;
using FlightBookingSystem.Service.Interface;

namespace FlightBookingSystem.Service
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ILogger<BookingService> _logger;

        public BookingService(IBookingRepository bookingRepository, ILogger<BookingService> logger)
        {
            _bookingRepository = bookingRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<BookingSendDTO>> GetAllBookingsAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all bookings.");
                return await _bookingRepository.GetAllBookingsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all bookings.");
                throw;
            }
        }

        public async Task<BookingSendDTO> GetBookingByIdAsync(int bookingId)
        {
            try
            {
                _logger.LogInformation($"Fetching booking with ID {bookingId}.");
                return await _bookingRepository.GetBookingByIdAsync(bookingId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    $"An error occurred while fetching booking with ID {bookingId}."
                );
                throw;
            }
        }

        public async Task<BookingSendDTO> AddBookingAsync(BookingDTO booking,string id)
        {
            try
            {
                _logger.LogInformation(
                    $"Adding new booking for flight ID {booking.FlightID} and passenger ID {booking.PassengerID}."
                );
                return await _bookingRepository.AddBookingAsync(booking,id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new booking.");
                throw;
            }
        }

        public async Task<bool> ConfirmBooking(int bookingId)
        {
            try
            {
                _logger.LogInformation($"Confirming booking with ID {bookingId}.");
                return await _bookingRepository.ConfirmBooking(bookingId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,$"An error occurred while confirming booking with ID {bookingId}.");
                throw;
            }
        }
    }
}
