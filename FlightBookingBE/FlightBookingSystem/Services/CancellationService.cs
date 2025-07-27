using FlightBookingSystem.Repository.Entity;
using FlightBookingSystem.Repository.Interface;

namespace FlightBookingSystem.Service
{
    public class CancellationService : ICancellationService
    {
        private readonly ICancellationRepository _cancellationRepository;
        private readonly ILogger<CancellationService> _logger;

        public CancellationService(
            ICancellationRepository cancellationRepository,
            ILogger<CancellationService> logger
        )
        {
            _cancellationRepository = cancellationRepository;
            _logger = logger;
        }

        public async Task<CancellationEntity> CancelBookingAsync(int bookingId)
        {
            try
            {
                _logger.LogInformation($"Attempting to cancel booking with ID {bookingId}.");
                return await _cancellationRepository.CancelBookingAsync(bookingId);
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
            System.Console.WriteLine("-----------------------------fbwef=---------------------------------------");
            try
            {
                _logger.LogInformation(
                    $"Attempting to approve cancellation with ID {cancellationID}."
                );

                bool isApproved = await _cancellationRepository.ApproveCancellationAsync(cancellationID);
                if (isApproved)
                {
                    _logger.LogInformation(
                        $"Cancellation with ID {cancellationID} approved successfully."
                    );
                    return true;
                }

                _logger.LogWarning($"Failed to approve cancellation with ID {cancellationID}.");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    $"An error occurred while approving cancellation with ID {cancellationID}."
                );
                return false;
            }
        }
    }
}
