using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlightBookingSystem.Repository.Entity
{
    public class FlightEntity
    {

        [Key]
        public int FlightID { get; set; }

        [Required]
        [StringLength(50)]
        public string FlightNumber { get; set; }

        [Required]
        public DateTime DepartureTime { get; set; }

        [Required]
        public DateTime ArrivalTime { get; set; }

        [Required]
        [StringLength(100)]
        public string Origin { get; set; }

        [Required]
        [StringLength(100)]
        public string Destination { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        [Required]
        public int TotalSeats { get; set; }

        public ICollection<BookingEntity> Bookings { get; set; }

        public List<string> AvailableSeats { get; set; } = new List<string>();

    }
}
