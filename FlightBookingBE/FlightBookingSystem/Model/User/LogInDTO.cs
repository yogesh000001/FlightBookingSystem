using System.ComponentModel.DataAnnotations;

namespace FlightBookingSystem.Model.User
{
    public class LogInDTO
    {
        [Required]
        public string Email {get; set;}
        [Required]
        public string Password {get; set;}   
    }
}