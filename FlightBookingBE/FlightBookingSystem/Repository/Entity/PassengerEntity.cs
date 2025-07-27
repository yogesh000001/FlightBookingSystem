using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightBookingSystem.Repository.Entity
{
    public class PassengerEntity
    {
        [Key]
        public int PassengerID { get; set; }
        public string Name { get; set; }
        [ForeignKey("User")]
        public int BookedByUser { get; set; }
        public string ContactDetails { get; set; }
        public int Code { get; set; }
        public string Gender { get; set; }
        public virtual UserEntity User { get; set; }
        public virtual ICollection<BookingEntity> Bookings { get; set; }
    }
}