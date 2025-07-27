using System.Security.Claims;
using FlightBookingSystem.Model.Passenger;
using FlightBookingSystem.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightBookingSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PassengerController : ControllerBase
    {
        private readonly IPassengerService _passengerService;
        private readonly ILogger<PassengerController> _logger;

        public PassengerController(
            IPassengerService passengerService,
            ILogger<PassengerController> logger
        )
        {
            _passengerService = passengerService;
            _logger = logger;
        }

        /// <summary>
        /// Fetches all passengers.
        /// Authorized for: Admin
        /// </summary>
        /// <returns>List of all passengers.</returns>
        [HttpGet]
        [Authorize(Roles = "Admin, Passenger")]
        public async Task<ActionResult<IEnumerable<PassengerDTO>>> GetAllPassengers()
        {
            try
            {
                _logger.LogInformation("Fetching all passengers.");
                var passengers = await _passengerService.GetAllPassengersAsync();
                return Ok(passengers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all passengers.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Fetches a passenger by ID.
        /// Authorized for: Admin, Passenger
        /// </summary>
        /// <param name="id">Passenger ID.</param>
        /// <returns>Passenger details.</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Passenger")]
        public async Task<ActionResult<PassengerDTO>> GetPassengerById(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching passenger with ID {id}.");
                var passenger = await _passengerService.GetPassengerByIdAsync(id);
                if (passenger == null)
                {
                    _logger.LogWarning($"Passenger with ID {id} not found.");
                    return NotFound($"Passenger with ID {id} not found.");
                }
                return Ok(passenger);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching passenger with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Adds a new passenger.
        /// Authorized for: Passenger
        /// </summary>
        /// <param name="passenger">Passenger details.</param>
        /// <returns>Confirmation message.</returns>
        [HttpPost]
        [Authorize(Roles = "Passenger")]
        public async Task<ActionResult> AddPassenger(PassengerDTO passenger)
        {
            if (passenger.UserID.ToString() != HttpContext.User.FindFirst("UserId").Value)
            {
                return BadRequest("The user id of log in user is wrong!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation($"Adding new passenger with user ID {passenger.UserID}.");
                await _passengerService.AddPassengerAsync(passenger);
                return CreatedAtAction(
                    nameof(GetPassengerById),
                    new { id = passenger.UserID },
                    passenger
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new passenger.");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates a passenger by ID.
        /// Authorized for: Passenger
        /// </summary>
        /// <param name="id">Passenger ID.</param>
        /// <param name="passenger">Updated passenger details.</param>
        /// <returns>No content.</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Passenger")]
        public async Task<ActionResult> UpdatePassenger(int id, UpdatePassengerDTO passenger)
        {
            try
            {
                _logger.LogInformation($"Updating passenger with ID {id}.");
                var result = await _passengerService.UpdatePassengerAsync(id, passenger);
                if (!result)
                {
                    _logger.LogWarning($"Passenger with ID {id} not found.");
                    return NotFound($"Passenger with ID {id} not found.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating passenger with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Deletes a passenger by ID.
        /// Authorized for: Admin
        /// </summary>
        /// <param name="id">Passenger ID.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeletePassenger(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting passenger with ID {id}.");
                var result = await _passengerService.DeletePassengerAsync(id);
                if (!result)
                {
                    _logger.LogWarning($"Passenger with ID {id} not found.");
                    return NotFound($"Passenger with ID {id} not found.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting passenger with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
