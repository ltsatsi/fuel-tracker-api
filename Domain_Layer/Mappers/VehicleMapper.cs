using Domain_Layer.Dtos.Vehicle;
using Domain_Layer.Models;

namespace Domain_Layer.Mappers
{
    public static class VehicleMapper
    {
        public static Vehicle ToVehicle(this VehicleDto vehicleDto)
        {
            return new Vehicle
            {
                Registration = vehicleDto.Registration.ToUpper(),
                Image = vehicleDto.Image,
                Make = vehicleDto.Make, 
                Model = vehicleDto.Model,
                Year = vehicleDto.Year,
            };   
        }

        public static Vehicle ToVehicleFromCreate(this CreateVehicleRequest request, string imageUrl)
        {
            return new Vehicle
            {
                Registration = request.Registration.ToUpper(),
                Image = imageUrl,
                Make = request.Make,
                Model = request.Model,
                Year = request.Year,

                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };
        }

        public static VehicleDto ToVehicleDto(this Vehicle vehicle, string? publicImageUrl)
        {
            return new VehicleDto
            {
                Id = vehicle.Id,
                Registration = vehicle.Registration,
                Image = publicImageUrl ?? vehicle.Image,
                Make = vehicle.Make,
                Model = vehicle.Model,
                Year = vehicle.Year,
                IsActive = vehicle.IsActive,
                FuelRecords = vehicle.FuelRecords.Select(f => f.ToFuelDto()).ToList(),
            };
        }
    } // end class
} // end namespace
