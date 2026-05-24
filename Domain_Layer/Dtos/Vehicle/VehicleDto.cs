using Domain_Layer.Dtos.FuelRecords;
using Domain_Layer.Models;

namespace Domain_Layer.Dtos.Vehicle
{
    public class VehicleDto
    {
        public int Id { get; set; } 
        public string Registration { get; set; }
        public string Image { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public bool IsActive { get; set; }  
        public List<FuelDto> FuelRecords { get; set; } = new List<FuelDto>();
    } // end class
} // end namespace
