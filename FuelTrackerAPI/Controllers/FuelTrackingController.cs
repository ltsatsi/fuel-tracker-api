using Domain_Layer.Dtos.FuelTracking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service_Layer.IService;

namespace FuelTrackerAPI.Controllers
{
    [Route("api/fuel-tracking")]
    [ApiController]
    public class FuelTrackingController : ControllerBase
    {
        private readonly IFuelTrackingService _fuelTrackingService;
        private readonly ICurrentUserService _currentUserService;
        public FuelTrackingController(IFuelTrackingService fuelTrackingService, ICurrentUserService currentUserService)
        {
            _fuelTrackingService = fuelTrackingService ?? throw new ArgumentNullException(nameof(fuelTrackingService));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        [Authorize]
        [HttpGet("monthly-distance/{vehicleId}")]
        public async Task<IActionResult> GetMonthlyDistance([FromRoute] int vehicleId, [FromQuery] int? year, [FromQuery] int? month)
        {
            var user = await _currentUserService.GetCurrentUserAsync();

            if (user == null)
                return Unauthorized();

            var monthlyDistance = await _fuelTrackingService.GetMonthlyDistanceAsync(year, month, vehicleId, user);
            return Ok(new MonthlyDistanceDto
            {
                Distance = monthlyDistance ?? 0,
                HasData = monthlyDistance != 0 && monthlyDistance != null ? true : false
            });
        }

        [Authorize]
        [HttpGet("fuel-consumption/{vehicleId}")]
        public async Task<IActionResult> GetFuelConsumption([FromRoute] int vehicleId)
        {
            var user = await _currentUserService.GetCurrentUserAsync();

            if (user == null)
                return Unauthorized();

            double? consumption = await _fuelTrackingService.GetFuelConsumptionAsync(vehicleId, user);

            if (consumption == null)
                return BadRequest(new FuelConsumptionDto
                {
                    Success = false,
                    Message = "At least 2 fuel entries are required.",
                    Consumption = null
                });

            return Ok(new FuelConsumptionDto
            {
                Success = true,
                Message = "The lower the consumption the more efficient: Consumption is in l/100km",
                Consumption = consumption
            });
        }

        [Authorize]
        [HttpGet("monthly-spending/{vehicleId}")]
        public async Task<IActionResult> GetMonthlyFuelSpending([FromRoute] int vehicleId, [FromQuery] int? year, [FromQuery] int? month)
        {
            var user = await _currentUserService.GetCurrentUserAsync();

            if (user == null)
                return Unauthorized();

            var spendings = await _fuelTrackingService.GetMonthlyFuelSpendingsAsync(year, month, vehicleId, user);

            if (spendings == null)
                spendings = 0;

            return Ok(new { spendings });
        }

        [Authorize]
        [HttpGet("monthly-analytics/{vehicleId}")]    
        public async Task<IActionResult> GetMonthlyAnalytics([FromRoute] int vehicleId, [FromQuery] int? year, [FromQuery] int? month)
        {
            var user = await _currentUserService.GetCurrentUserAsync();

            if (user == null)
                return Unauthorized();

            var totalDistance = await _fuelTrackingService.GetMonthlyDistanceAsync(year, month, vehicleId, user);
            var totalFuelCost = await _fuelTrackingService.GetMonthlyFuelSpendingsAsync(year, month, vehicleId, user);
            var totalLiters = await _fuelTrackingService.GetMonthlyLitersAsync(year, month, vehicleId, user);
            var fuelEntryCount = await _fuelTrackingService.GetMonthlyFuelCount(year, month, vehicleId, user);
            
            return Ok(new MonthlyAnalytics
            {
                VehicleId = vehicleId,
                Year = year ?? DateTime.UtcNow.Year,
                Month = month ?? DateTime.UtcNow.Month,

                TotalFuelCost = totalFuelCost ?? 0,
                TotalDistanceCovered = totalDistance ?? 0,
                TotalLitersPurchased = totalLiters ?? 0,
                FuelEntryCount = fuelEntryCount ?? 0
            });
        }
    } // end class
} // end namespace
