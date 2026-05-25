using Domain_Layer.Dtos.FuelRecords;
using Domain_Layer.Models;

namespace Domain_Layer.Mappers
{
    public static class FuelMapper
    {
        public static Fuel ToFuel(this FuelDto fuelDto)
        {
            return new Fuel
            {
                Id = fuelDto.Id,
                Price = fuelDto.Price,
                Station = fuelDto.Station,
                Liter = fuelDto.Liter,
                Odometer = fuelDto.Odometer,
                VehicleId = fuelDto.VehicleId,
            };
        }

        public static Fuel ToFuelFromCreate(this CreateFuelRequest request, int vehicleId)
        {
            return new Fuel
            {
                Price = request.Price,
                Station = request.Station,
                Liter = request.Liter,
                Odometer = request.Odometer,
                VehicleId = vehicleId,

                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };
        }

        public static FuelDto ToFuelDto(this Fuel fuel)
        {
            return new FuelDto
            {
                Id = fuel.Id,
                Price = fuel.Price,
                Station = fuel.Station,
                Liter = fuel.Liter,
                Odometer = fuel.Odometer,
                VehicleId = fuel.VehicleId,
            };
        }
    } // end class
} // end namespace
