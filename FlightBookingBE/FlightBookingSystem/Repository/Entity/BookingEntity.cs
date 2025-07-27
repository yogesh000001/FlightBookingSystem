using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FlightBookingSystem.Repository.Entity
{
    public class BookingEntity
    {
        [Key]
        public int BookingID { get; set; }

        [ForeignKey("FlightEntity")]
        public int FlightID { get; set; }

        [ForeignKey("PassengerEntity")]
        public int PassengerID { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; }

        [Required]
        [MaxLength(50)]
        public string PaymentStatus { get; set; }

        [Required]
        [MaxLength(50)]
        public string SeatNumber { get; set; }

        public virtual FlightEntity Flight { get; set; }
        public virtual PassengerEntity Passenger { get; set; }
        public virtual ICollection<CancellationEntity> Cancellations { get; set; }

        public string UserId { get; set; }
    }
}
