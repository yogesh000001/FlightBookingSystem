using FlightBookingSystem.Model.Booking;
using FlightBookingSystem.Repository.Entity;
using FlightBookingSystem.Service.Interface;
using FlightBookingSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightBookingSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly ILogger<BookingController> _logger;

        public BookingController(IBookingService bookingService, ILogger<BookingController> logger)
        {
            _bookingService = bookingService;
            _logger = logger;
        }

        /// <summary>
        /// Fetches all bookings.
        /// Authorized for: Admin
        /// </summary>
        /// <returns>List of all bookings.</returns>
        [HttpGet]
        [Authorize(Roles = "Admin , Passenger")]
        public async Task<ActionResult<IEnumerable<BookingSendDTO>>> GetAllBookings()
        {
            try
            {
                _logger.LogInformation("Fetching all bookings.");
                var bookings = await _bookingService.GetAllBookingsAsync();
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all bookings.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Fetches a booking by ID.
        /// Authorized for: Admin, Passenger
        /// </summary>
        /// <param name="id">Booking ID.</param>
        /// <returns>Booking details.</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Passenger")]
        public async Task<ActionResult<BookingSendDTO>> GetBookingById(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching booking with ID {id}.");
                var booking = await _bookingService.GetBookingByIdAsync(id);
                if (booking == null)
                {
                    _logger.LogWarning($"Booking with ID {id} not found.");
                    return NotFound();
                }
                return Ok(booking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching booking with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Adds a new booking.
        /// Authorized for: Passenger
        /// </summary>
        /// <param name="booking">Booking details.</param>
        /// <returns>Confirmation message.</returns>
        [HttpPost]
        [Authorize(Roles = "Passenger")]
        public async Task<ActionResult<BookingSendDTO>> AddBooking(BookingDTO booking)
        {
            try
            {
                _logger.LogInformation(
                    $"Adding new booking for flight ID {booking.FlightID} and passenger ID {booking.PassengerID}."
                );
                
                string userId = User.FindFirst("UserId")?.Value??"0";
                var newBooking = await _bookingService.AddBookingAsync(booking,userId);
                return Ok(newBooking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new booking.");
                
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Confirms a booking by ID.
        /// Authorized for: Admin
        /// </summary>
        /// <param name="bookingId">Booking ID.</param>
        /// <returns>Confirmation message.</returns>
        [HttpPut("ConfirmBooking/{bookingId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ConfirmBooking(int bookingId)
        {
            try
            {
                _logger.LogInformation($"Confirming booking with ID {bookingId}.");
                bool isConfirm = await _bookingService.ConfirmBooking(bookingId);
                if (isConfirm)
                {
                    return Ok($"Booking is confirmed with ID {bookingId}");
                }
                _logger.LogWarning($"Booking confirmation failed for ID {bookingId}.");
                return BadRequest("Booking confirmation failed");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    $"An error occurred while confirming booking with ID {bookingId}."
                );
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
