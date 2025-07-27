using FlightBookingSystem.Repository.Entity;
using FlightBookingSystem.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightBookingSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CancellationController : ControllerBase
    {
        private readonly ICancellationService _cancellationService;
        private readonly ILogger<CancellationController> _logger;

        public CancellationController(
            ICancellationService cancellationService,
            ILogger<CancellationController> logger
        )
        {
            _cancellationService = cancellationService;
            _logger = logger;
        }

        /// <summary>
        /// Cancels a booking by ID.
        /// Authorized for: Passenger
        /// </summary>
        /// <param name="bookingId">Booking ID.</param>
        /// <returns>Cancellation details.</returns>
        [HttpPost("Cancel/{bookingId}")]
        [Authorize(Roles = "Passenger")]
        public async Task<ActionResult<CancellationEntity>> CancelBooking(int bookingId)
        {
            try
            {
                _logger.LogInformation($"Attempting to cancel booking with ID {bookingId}.");
                var result = await _cancellationService.CancelBookingAsync(bookingId);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(
                    ex,
                    $"Cancellation failed for booking ID {bookingId}: {ex.Message}"
                );
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    $"An error occurred while cancelling booking with ID {bookingId}."
                );
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Approves a cancellation by ID.
        /// Authorized for: Admin
        /// </summary>
        /// <param name="cancellationId">Cancellation ID.</param>
        /// <returns>Confirmation message.</returns>
        [HttpPut("ApproveCancellation/{cancellationId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveCancellation([FromRoute]int cancellationId)
        {

           
            _logger.LogInformation(
                $"Received request to approve cancellation with ID {cancellationId}."
            );

            try
            {
                bool isConfirm = await _cancellationService.ApproveCancellationAsync(
                    cancellationId
                );
                if (isConfirm)
                {
                    _logger.LogInformation(
                        $"Cancellation with ID {cancellationId} approved successfully."
                    );
                    return Ok(new { message = "Booking is successfully cancelled." });
                }

                _logger.LogWarning($"Failed to approve cancellation with ID {cancellationId}.");
                return BadRequest("Booking cancellation is not approved.");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    $"An error occurred while approving cancellation with ID {cancellationId}."
                );
                return StatusCode(500, "An internal server error occurred.");
            }
        }
    }
}