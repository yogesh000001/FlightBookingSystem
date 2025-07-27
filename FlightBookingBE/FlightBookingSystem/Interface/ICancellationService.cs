using System.Threading.Tasks;
using FlightBookingSystem.Model.Cancellation;
using FlightBookingSystem.Repository.Entity;

namespace FlightBookingSystem.Service
{
    public interface ICancellationService
    {
        Task<CancellationEntity> CancelBookingAsync(int bookingId);
        Task<bool> ApproveCancellationAsync(int cancellationID);
    }
}
