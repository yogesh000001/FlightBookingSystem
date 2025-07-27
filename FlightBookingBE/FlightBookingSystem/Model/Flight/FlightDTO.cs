using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlightBookingSystem.Model.Flight
{
    public class FlightDTO
    {
        [Required]
        public string FlightNumber { get; set; }
        [Required]
        public DateTime DepartureTime { get; set; }
        [Required]

        public DateTime ArrivalTime { get; set; }
        [Required]

        public string Origin { get; set; }
        [Required]

        public string Destination { get; set; }
        [Required]

        public string Status { get; set; }
        [Required]

        [Range(30, 300, ErrorMessage = "Total seats must be at least 30 and not exceed than 300.")]
        public int TotalSeats { get; set; }
    }
}
