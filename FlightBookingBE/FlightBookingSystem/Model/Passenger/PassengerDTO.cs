using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FlightBookingSystem.Repository.Entity;

namespace FlightBookingSystem.Model.Passenger
{
    public class PassengerDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]

        public int UserID { get; set; }
        [Required]

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string ContactDetails { get; set; }
        [Required]

        [Range(10000, 9999999999, ErrorMessage = "Code must be between 5 and 10 digits.")]
        public int Code { get; set; }
        [Required]
        public string Gender { get; set; }
    }
}
