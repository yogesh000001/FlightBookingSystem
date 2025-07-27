using System.ComponentModel.DataAnnotations;

namespace FlightBookingSystem.Model.User
{
    public class GetUserDTO
    {
        [Required]
        public string FirstName {get; set;}
        public string LastName {get; set;}
        [Required]
        public string Email {get; set;}
        [Required]
        public string PhoneNo {get; set;}
        [Required]
        public string Role {get; set;}
    }

    public class GetUserByEmailDTO
    {

        public int Id {get;set;}
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNo { get; set; }
        [Required]
        public string Role { get; set; }
    }
}