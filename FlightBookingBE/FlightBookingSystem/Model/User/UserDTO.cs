using System.ComponentModel.DataAnnotations;

namespace FlightBookingSystem.Model.User
{
    public class UserDTO
    {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]

        [EmailAddress]
        public string Email { get; set; }
        [Required]

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string PhoneNo { get; set; }

        [RegularExpression(
            @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must be at least 8 characters long and contain at least one letter, one number, and one special character."
        )]
        [Required]
        public string Password { get; set; }
        [Required]

        public string Role { get; set; }
    }

    public class ResetPasswordModel{
        public string Email {get;set;}
        public string NewPassword {get;set;}
        public string ConfirmPassword {get;set;}
    }

    public class ForgotPasswordModel{
        public string Email {get;set;}
    }
}
