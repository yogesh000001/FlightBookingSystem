using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightBookingSystem.Repository.Entity
{
    public class CancellationEntity
    {
        [Key]
        public int CancellationID { get; set; }

        [ForeignKey("BookingEntity")]
        public int BookingID { get; set; }

        [Required]
        public DateTime CancellationDate { get; set; }

        [Required]
        public string CancellationStatus { get; set; }

        [Required]
        [MaxLength(50)]
        public string RefundStatus { get; set; }

        public virtual BookingEntity Booking { get; set; }
    }
}
