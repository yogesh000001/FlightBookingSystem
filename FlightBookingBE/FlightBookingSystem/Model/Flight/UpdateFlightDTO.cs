using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlightBookingSystem.Model.Flight
{
    public class UpdateFlightDTO
    {
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
        public int TotalSeats {get;set;}
    }
}