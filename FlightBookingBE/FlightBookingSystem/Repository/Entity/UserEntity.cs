using System.ComponentModel.DataAnnotations;

namespace FlightBookingSystem.Repository.Entity
{
    public class UserEntity
    {
        [Key]
        [Required]
        public int UserId {get; set;}
        [Required]
        public string FirstName {get; set;}
        [Required]
        public string LastName {get; set;}
        [Required]
        public string Email {get; set;}
        [Required]
        public string PhoneNo {get; set;}
        [Required]
        public string Password {get; set;}
        [Required]
        public string Role {get; set;}
        public virtual ICollection<PassengerEntity> Passengers { get; set; }
    }
}