using FlightBookingSystem.Repository.Context;
using FlightBookingSystem.Repository.Entity;
using FlightBookingSystem.Repository.Interface;

namespace FlightBookingSystem.Repository
{
    public class CancellationRepository : ICancellationRepository
    {
        private readonly FlightContext _context;
        private readonly ILogger<CancellationRepository> _logger;

        public CancellationRepository(FlightContext context, ILogger<CancellationRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<CancellationEntity> CancelBookingAsync (int bookingId)
        {
            try
            {
                _logger.LogInformation($"Attempting to cancel booking with ID {bookingId}.");

                var booking = await _context.Bookings.FindAsync(bookingId);
                if (booking == null)
                {
                    _logger.LogWarning($"Booking with ID {bookingId} not found.");
                    throw new InvalidOperationException("Booking not found. Please provide a valid booking ID.");
                }

                booking.Status = "Requested Cancellation";
                _context.Bookings.Update(booking);

                var cancellationEntity = new CancellationEntity
                {
                    BookingID = bookingId,
                    RefundStatus = "Pending",
                    CancellationStatus = "Requested"
                };
                // booking.BookingID = cancellationEntity.CancellationID;
                // _context.Bookings.Update(booking);
                _context.Cancellations.Add(cancellationEntity);

                await _context.SaveChangesAsync();
                _logger.LogInformation($"Booking with ID {bookingId} cancelled successfully.");
                return cancellationEntity;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    $"An error occurred while cancelling booking with ID {bookingId}."
                );
                throw;
            }
        }

        public async Task<bool> ApproveCancellationAsync(int cancellationID)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation(
                    $"Attempting to approve cancellation with ID {cancellationID}."
                );

                // var cancellation = await _context.Cancellations.FindAsync(cancellationID);
                // if (cancellation == null)
                // {
                //     _logger.LogWarning($"Cancellation with ID {cancellationID} not found.");
                //     return false;
                // }

                var booking = await _context.Bookings.FindAsync(cancellationID);
                System.Console.WriteLine(booking+"          -----------------------------------------------        ");
                if (booking == null)
                {
                    _logger.LogWarning($"Booking with ID {cancellationID} not found.");
                    return false;
                }

                var flight = await _context.Flights.FindAsync(booking.FlightID);
                if (flight == null)
                {
                    _logger.LogWarning($"Flight with ID {booking.FlightID} not found.");
                    return false;
                }

                // cancellation.CancellationStatus = "Accepted";
                // cancellation.RefundStatus = "Successfully Refunded";
                booking.Status = "Cancelled";
                booking.PaymentStatus = "Refunded";

                flight.TotalSeats += 1;
                flight.AvailableSeats.Add(booking.SeatNumber);

                // _context.Cancellations.Update(cancellation);
                _context.Bookings.Update(booking);
                _context.Flights.Update(flight);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation(
                    $"Cancellation with ID {cancellationID} approved successfully."
                );
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex,$"An error occurred while approving cancellation with ID {cancellationID}.");
                return false;
            }
        }
    }
}
