using FlightBookingSystem.Model.Cancellation;
using FlightBookingSystem.Repository.Entity;

namespace FlightBookingSystem.Repository.Interface
{
    public interface ICancellationRepository
    {
        Task<CancellationEntity> CancelBookingAsync(int bookingId);
        Task<bool> ApproveCancellationAsync(int cancellationID);
    }
}