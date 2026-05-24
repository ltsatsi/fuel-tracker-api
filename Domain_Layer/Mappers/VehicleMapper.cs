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

        public static Vehicle ToVehicleFromCreate(this CreateVehicleRequest request)
        {
            return new Vehicle
            {
                Registration = request.Registration.ToUpper(),
                Image = request.Image,
                Make = request.Make,
                Model = request.Model,
                Year = request.Year,

                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };
        }

        public static VehicleDto ToVehicleDto(this Vehicle vehicle)
        {
            return new VehicleDto
            {
                Id = vehicle.Id,
                Registration = vehicle.Registration,
                Image = vehicle.Image,
                Make = vehicle.Make,
                Model = vehicle.Model,
                Year = vehicle.Year,
                IsActive = vehicle.IsActive,
                FuelRecords = vehicle.FuelRecords.Select(f => f.ToFuelDto()).ToList(),
            };
        }
    } // end class
} // end namespace
