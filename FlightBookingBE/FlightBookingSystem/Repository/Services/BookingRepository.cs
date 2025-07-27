using FlightBookingSystem.Model.Booking;
using FlightBookingSystem.Repository.Context;
using FlightBookingSystem.Repository.Entity;
using FlightBookingSystem.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlightBookingSystem.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly FlightContext _context;
        private readonly ILogger<BookingRepository> _logger;

        public BookingRepository(FlightContext context, ILogger<BookingRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<BookingSendDTO>> GetAllBookingsAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all bookings.");
                var bookings = await _context.Bookings.ToListAsync();
                if (!bookings.Any())
                {
                    _logger.LogWarning("No bookings found.");
                    return Enumerable.Empty<BookingSendDTO>();
                }

                var bookingSendDTOs = bookings
                    .Select(booking => new BookingSendDTO
                    {
                        BookingID = booking.BookingID,
                        FlightID = booking.FlightID,
                        PassengerID = booking.PassengerID,
                        BookingDate = booking.BookingDate,
                        Status = booking.Status,
                        PaymentStatus = booking.PaymentStatus,
                        SeatNumber = booking.SeatNumber,
                        UserId = booking.UserId
                    })
                    .ToList();

                return bookingSendDTOs;
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
                var booking = await _context.Bookings.FindAsync(bookingId);
                if (booking == null)
                {
                    _logger.LogWarning($"Booking with ID {bookingId} not found.");
                    return null;
                }

                var bookingSendDTO = new BookingSendDTO
                {
                    BookingID = booking.BookingID,
                    FlightID = booking.FlightID,
                    PassengerID = booking.PassengerID,
                    BookingDate = booking.BookingDate,
                    Status = booking.Status,
                    PaymentStatus = booking.PaymentStatus,
                    SeatNumber = booking.SeatNumber,
                };

                return bookingSendDTO;
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
                var flight = await _context.Flights.FindAsync(booking.FlightID);
                if (flight == null)
                {
                    _logger.LogError("Flight not found.");
                    throw new Exception("Flight not found.");
                }

                if (booking.BookingDate >= flight.DepartureTime.AddDays(-1))
                {
                    _logger.LogError(
                        "Booking date must be at least one day less than departure time of flight."
                    );
                    throw new Exception(
                        "Booking date must be at least one day less than departure time of flight."
                    );
                }

                if (flight.AvailableSeats.Count == 0)
                {
                    _logger.LogError("No seats available.");
                    throw new Exception("No seats available.");
                }

                var passenger = await _context.Passengers.FindAsync(booking.PassengerID);
                if (passenger == null)
                {
                    _logger.LogError("Passenger not found.");
                    throw new Exception("Passenger not found.");
                }

                var existingBooking = await _context.Bookings.FirstOrDefaultAsync(b =>
                    b.PassengerID == booking.PassengerID && b.FlightID == booking.FlightID
                );
                if (existingBooking != null)
                {
                    _logger.LogError("Booking already exists for this passenger.");
                    throw new Exception("Booking already exists for this passenger.");
                }

                var newBooking = new BookingEntity
                {
                    FlightID = booking.FlightID,
                    PassengerID = booking.PassengerID,
                    BookingDate = booking.BookingDate,
                    Status = "Pending",
                    PaymentStatus = "Paid by Customer - pending Confirmation",
                    SeatNumber = "Not Confirmed",
                    UserId=id
                };

                await _context.Bookings.AddAsync(newBooking);
                flight.TotalSeats -= 1;
                _context.Flights.Update(flight);
                await _context.SaveChangesAsync();

                _logger.LogInformation("New booking added successfully.");

                return new BookingSendDTO
                {
                    BookingID = newBooking.BookingID,
                    FlightID = newBooking.FlightID,
                    PassengerID = newBooking.PassengerID,
                    BookingDate = newBooking.BookingDate,
                    Status = newBooking.Status,
                    PaymentStatus = newBooking.PaymentStatus,
                    SeatNumber = newBooking.SeatNumber,
                };
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
                var booking = await _context.Bookings.FindAsync(bookingId);
                if (booking == null)
                {
                    _logger.LogWarning($"Booking with ID {bookingId} not found.");
                    return false;
                }

                var flight = await _context.Flights.FindAsync(booking.FlightID);
                if (flight == null || !flight.AvailableSeats.Any())
                {
                    _logger.LogWarning("Flight not found or no available seats.");
                    return false;
                }

                booking.Status = "Confirmed";
                booking.PaymentStatus = "Payment recieved and verified";
                booking.SeatNumber = flight.AvailableSeats.First();
                flight.AvailableSeats.Remove(booking.SeatNumber);

                await _context.SaveChangesAsync();

                _logger.LogInformation("Booking confirmed successfully.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    $"An error occurred while confirming booking with ID {bookingId}."
                );
                throw;
            }
        }
    }
}
