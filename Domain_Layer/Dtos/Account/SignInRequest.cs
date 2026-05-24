using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain_Layer.Dtos.Account
{
    public class SignInRequest
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    } // end class
} // end namespace
