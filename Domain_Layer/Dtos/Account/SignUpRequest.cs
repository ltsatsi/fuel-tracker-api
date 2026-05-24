using System.ComponentModel.DataAnnotations;

namespace Domain_Layer.Dtos.Account
{
    public class SignUpRequest
    {
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passords do not match")]
        public string ConfirmPassword { get; set; } 
    } // end class
} // end namespace
