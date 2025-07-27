using System.ComponentModel.DataAnnotations;

namespace FlightBookingSystem.Model.User
{
    public class UpdateDTO
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNo { get; set; }
    }
}
