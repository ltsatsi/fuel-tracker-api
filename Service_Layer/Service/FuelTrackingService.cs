using Domain_Layer.Models;
using FuelTrackerAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Service_Layer.IService;

namespace Service_Layer.Service
{
    public class FuelTrackingService : IFuelTrackingService
    {
        private readonly ICustomService<Fuel> _fuelService;
        public FuelTrackingService(ICustomService<Fuel> fuelService)
        {
            _fuelService = fuelService ?? throw new ArgumentNullException(nameof(fuelService));
        }

        public async Task<double?> GetFuelConsumptionAsync(int vehicleId, ApplicationUser currentUser)
        {
            var fuelEntries = await _fuelService.GetAllAsync(new QueryObject());    

            if (!fuelEntries.Any(x =>
                x.VehicleId == vehicleId &&
                x.Vehicle.ApplicationUserId == currentUser.Id))
            {
                return null;
            }

            var userFuelEntries = fuelEntries
                .Where(x => 
                x.VehicleId == vehicleId && 
                x.Vehicle.ApplicationUserId == currentUser.Id);

            if (userFuelEntries.Count() < 2)
                return null;

            var currentOdometer = fuelEntries
                .Where(f => 
                f.VehicleId == vehicleId &&
                f.Vehicle.ApplicationUserId == currentUser.Id)
                .OrderByDescending(f => f.CreatedDate)
                .FirstOrDefault()!.Odometer;

            var previousOdometer = fuelEntries
                .Where(f =>
                f.VehicleId == vehicleId &&
                f.Vehicle.ApplicationUserId == currentUser.Id)
                .OrderByDescending(f => f.CreatedDate)
                .Skip(1)
                .FirstOrDefault()!.Odometer; 

            double liters = fuelEntries
                .Where(f =>
                f.VehicleId == vehicleId &&
                f.Vehicle.ApplicationUserId == currentUser.Id)
                .OrderByDescending(f => f.CreatedDate)
                .FirstOrDefault()!.Liter;

            double distanceTraveled = currentOdometer - previousOdometer;

            return Math.Round((liters / distanceTraveled) * 100, 2);
        }

        public async Task<double?> GetMonthlyDistanceAsync(int? year, int? month, int vehicleId, ApplicationUser currentUser)
        {
            var monthlyFuelEntries = await GetMonthlyEntriesAsync(year, month, vehicleId, currentUser);

            if (monthlyFuelEntries == null)
                return null;

            if (!monthlyFuelEntries.Any())
                return 0;

            var highestOdometer = await monthlyFuelEntries.MaxAsync(f => f.Odometer);
            var lowestOdometer = await monthlyFuelEntries.MinAsync(f => f.Odometer);

            var monthlyDistance = highestOdometer - lowestOdometer;

            return monthlyDistance;
        }

        public async Task<int?> GetMonthlyFuelCount(int? year, int? month, int vehicleId, ApplicationUser currentUser)
        {
            var monthlyFuelEntries = await GetMonthlyEntriesAsync(year, month, vehicleId, currentUser);

            if (monthlyFuelEntries == null)
                return null;

            return monthlyFuelEntries.Count();
        }

        public async Task<double?> GetMonthlyFuelSpendingsAsync(int? year, int? month, int vehicleId, ApplicationUser currentUser)
        {
            var monthlyFuelEntries = await GetMonthlyEntriesAsync(year, month, vehicleId, currentUser);

            if (monthlyFuelEntries == null)
                return null;

            var totalPrice =  await monthlyFuelEntries.SumAsync(f => f.Price);
            return totalPrice;
        }

        public async Task<double?> GetMonthlyLitersAsync(int? year, int? month, int vehicleId, ApplicationUser currentUser)
        {
            var monthlyFuelEntries = await GetMonthlyEntriesAsync(year, month, vehicleId, currentUser);

            if (monthlyFuelEntries == null)
                return null;

            var totalLiters = await monthlyFuelEntries.SumAsync(f => f.Liter);
            return totalLiters;
        }
    
        // Helper
        private async Task<IQueryable<Fuel>?> GetMonthlyEntriesAsync(int? year, int? month, int vehicleId, ApplicationUser currentUser)
        {
            var now = DateTime.UtcNow;
            var startOfMonth = new DateTime(year ?? now.Year, month ?? now.Month, 1);
            var startOfNextMonth = startOfMonth.AddMonths(1);

            var fuelEntries = await _fuelService.GetAllAsync(new QueryObject());

            if (fuelEntries.Count() <= 0)
                return null;

            var monthlyFuelEntries = fuelEntries
                .Where(f =>
                f.VehicleId == vehicleId &&
                f.Vehicle.ApplicationUserId == currentUser.Id &&
                f.CreatedDate >= startOfMonth &&
                f.CreatedDate < startOfNextMonth);

            return monthlyFuelEntries;
        }
    } // end class
} // end namespace
