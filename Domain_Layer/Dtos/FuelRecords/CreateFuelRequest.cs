using System.ComponentModel.DataAnnotations;

namespace Domain_Layer.Dtos.FuelRecords
{
    public class CreateFuelRequest
    {
        [Required]
        [Range(1, double.MaxValue)]
        public double Price { get; set; }

        [Required]
        [StringLength(52, MinimumLength = 3, ErrorMessage = "Station name must be between 3 to 52 characters")]
        public string Station { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        public double Liter { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        public double Odometer { get; set; }
    } // end class
} // end namespace
