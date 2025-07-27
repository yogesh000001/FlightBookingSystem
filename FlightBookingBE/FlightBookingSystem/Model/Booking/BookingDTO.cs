using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlightBookingSystem.Model.Booking
{
    public class BookingDTO
    {
        [Required]
        public int FlightID { get; set; }
        [Required]
        public int PassengerID { get; set; }
        [Required]
        public DateTime BookingDate { get; set; }

    }
}