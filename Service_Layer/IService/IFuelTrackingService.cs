using Domain_Layer.Models;

namespace Service_Layer.IService
{
    public interface IFuelTrackingService
    {
        Task<int?> GetMonthlyFuelCount(int? year, int? month, int vehicleId, ApplicationUser currentUser);
        Task<double?> GetMonthlyFuelSpendingsAsync(int? year, int? month, int vehicleId, ApplicationUser currentUser);
        Task<double?> GetFuelConsumptionAsync(int vehicleId, ApplicationUser currentUser);
        Task<double?> GetMonthlyDistanceAsync(int? year, int? month, int vehicleId, ApplicationUser currentUser);
        Task<double?> GetMonthlyLitersAsync(int? year, int? month, int vehicleId, ApplicationUser currentUser);
    } // end namespace
} // end namespace
    