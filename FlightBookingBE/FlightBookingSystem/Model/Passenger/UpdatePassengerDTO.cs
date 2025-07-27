using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FlightBookingSystem.Repository.Entity;

namespace FlightBookingSystem.Model.Passenger
{
    public class UpdatePassengerDTO
    {
        [Required]
        public string ContactDetails { get; set; }
        [Required]
        public string Gender { get; set; }
    }
}