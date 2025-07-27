using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlightBookingSystem.Model.Booking
{
    public class BookingSendDTO
    {
        [Required]
        public int BookingID { get; set; }
        [Required]
        public int FlightID { get; set; }
        [Required]
        public int PassengerID { get; set; }
        [Required]
        public DateTime BookingDate { get; set; }
        public string Status { get; set; }
        [Required]
        public string PaymentStatus { get; set; }
        [Required]
        public string SeatNumber { get; set; }

        public string UserId { get; set; }
    }
}
