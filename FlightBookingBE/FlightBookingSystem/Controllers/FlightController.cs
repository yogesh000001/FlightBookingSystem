using FlightBookingSystem.Interface;
using FlightBookingSystem.Model.Flight;
using FlightBookingSystem.Repository.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightBookingSystem.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly IFlightService _flightService;
        private readonly ILogger<FlightController> _logger;

        public FlightController(IFlightService flightService, ILogger<FlightController> logger)
        {
            _flightService = flightService;
            _logger = logger;
        }

        /// <summary>
        /// Fetches all flights.
        /// Authorized for: Admin, Passenger
        /// </summary>
        /// <returns>List of all flights.</returns>
        [HttpGet]
        [Authorize(Roles = "Admin, Passenger")]
        public async Task<ActionResult<IEnumerable<FlightEntity>>> GetAllFlights()
        {
            try
            {
                _logger.LogInformation("Fetching all flights.");
                var flights = await _flightService.GetAllFlights();
                if (flights == null || !flights.Any())
                {
                    _logger.LogWarning("No flights available.");
                    return NotFound("No flights available.");
                }
                return Ok(flights);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all flights.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Fetches a flight by ID.
        /// Authorized for: Admin, Passenger
        /// </summary>
        /// <param name="id">Flight ID.</param>
        /// <returns>Flight details.</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Passenger")]
        public async Task<ActionResult<FlightEntity>> GetFlightById(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching flight with ID {id}.");
                var flight = await _flightService.GetFlightById(id);
                if (flight == null)
                {
                    _logger.LogWarning($"Flight with ID {id} not found.");
                    return NotFound($"Flight with ID {id} not found.");
                }
                return Ok(flight);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching flight with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Adds a new flight.
        /// Authorized for: Admin
        /// </summary>
        /// <param name="flight">Flight details.</param>
        /// <returns>Confirmation message.</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddFlight([FromBody] FlightDTO flight)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _logger.LogInformation($"Adding new flight with number {flight.FlightNumber}.");
                var createdFlight = await _flightService.AddFlight(flight);
                return CreatedAtAction(
                    nameof(GetFlightById),
                    new { id = createdFlight.FlightID },
                    createdFlight
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new flight.");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates a flight by ID.
        /// Authorized for: Admin
        /// </summary>
        /// <param name="id">Flight ID.</param>
        /// <param name="flight">Updated flight details.</param>
        /// <returns>No content.</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateFlight(int id, [FromBody] UpdateFlightDTO flight)
        {
            try
            {
                _logger.LogInformation($"Updating flight with ID {id}.");
                var result = await _flightService.UpdateFlight(id, flight);
                if (!result)
                {
                    _logger.LogWarning($"Flight with ID {id} not found.");
                    return NotFound($"Flight with ID {id} not found.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating flight with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Deletes a flight by ID.
        /// Authorized for: Admin
        /// </summary>
        /// <param name="id">Flight ID.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteFlight(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting flight with ID {id}.");
                var result = await _flightService.DeleteFlight(id);
                if (!result)
                {
                    _logger.LogWarning($"Flight with ID {id} not found.");
                    return NotFound($"Flight with ID {id} not found.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting flight with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}