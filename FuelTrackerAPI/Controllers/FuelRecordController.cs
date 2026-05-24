using Domain_Layer.Dtos.FuelRecords;
using Domain_Layer.Mappers;
using Domain_Layer.Models;
using FuelTrackerAPI.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service_Layer.IService;

namespace FuelTrackerAPI.Controllers
{
    [Route("api/fuelrecords")]
    [ApiController]
    public class FuelRecordController : ControllerBase
    {
        private readonly ICustomService<Fuel> _fuelService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICustomService<Vehicle> _vehicleService;
        public FuelRecordController(ICustomService<Fuel> fuelService, ICustomService<Vehicle> vehicleService, ICurrentUserService currentUserService)
        {
            _fuelService = fuelService ?? throw new ArgumentNullException(nameof(fuelService));
            _vehicleService = vehicleService ?? throw new ArgumentNullException(nameof(vehicleService));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index([FromQuery] QueryObject query)
        {
            var user = await _currentUserService.GetCurrentUserAsync();

            if (user == null)
                return Unauthorized();

            var fuelrecords = await _fuelService.GetAllAsync(query);
            var fuelrecordsDto = await fuelrecords
                .Where(f => f.Vehicle!.ApplicationUserId == user.Id)
                .Select(f => f.ToFuelDto())
                .ToListAsync();

            return Ok(fuelrecordsDto);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var fuel = await _fuelService.GetByIdAsync(id);

            if (fuel == null)
                return NotFound();

            return Ok(fuel.ToFuelDto());
        }

        [Authorize]
        [HttpPost("{vehicleId}")]
        public async Task<IActionResult> Create([FromRoute]int vehicleId, [FromBody] CreateFuelRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _currentUserService.GetCurrentUserAsync();

            if (user == null)
                return Unauthorized();

            var vehicle = await _vehicleService.GetByIdAsync(vehicleId);

            if (vehicle == null)
                return NotFound();

            if (vehicle.ApplicationUserId != user.Id)
                return NotFound();

            var fuel = request.ToFuelFromCreate(vehicle.Id);
            await _fuelService.CreateAsync(fuel);
            return CreatedAtAction(nameof(GetById), new { id = fuel.Id }, fuel.ToFuelDto());
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateFuelRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _currentUserService.GetCurrentUserAsync();

            if (user == null)
                return Unauthorized();

            var fuel = await _fuelService.GetByIdAsync(id);

            if (fuel == null)
                return NotFound();

            fuel.Price = request.Price;
            fuel.Station = request.Station;
            fuel.Liter = request.Liter;
            fuel.Odometer = request.Odometer;
            fuel.ModifiedOn = DateTime.UtcNow;

            await _fuelService.UpdateAsync(fuel);
            return Ok(fuel.ToFuelDto());
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var fuel = await _fuelService.GetByIdAsync(id);

            if (fuel == null)
                return NotFound();

            await _fuelService.DeleteAsync(fuel);
            return NoContent();
        }

    } // end class
} // end namespace
