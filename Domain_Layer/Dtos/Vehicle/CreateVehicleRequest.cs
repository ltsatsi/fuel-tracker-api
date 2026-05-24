using System.ComponentModel.DataAnnotations;

namespace Domain_Layer.Dtos.Vehicle
{
    public class CreateVehicleRequest
    {
        [Required]
        [StringLength(8, MinimumLength = 3, ErrorMessage = "Registration must be between 3 to 8 characters")]
        public string Registration { get; set; }

        [Required]
        public string Image { get; set; }

        [Required]
        [StringLength(52, MinimumLength = 3, ErrorMessage = "Make must be between 3 to 52 characters")]
        public string Make { get; set; }

        [Required]
        [StringLength(52, MinimumLength = 3, ErrorMessage = "Model must be between 3 to 52 characters")]
        public string Model { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public int Year { get; set; }
    } // end class
} // end namespace
