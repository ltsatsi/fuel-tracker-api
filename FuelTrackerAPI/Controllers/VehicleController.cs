using Domain_Layer.Dtos.Vehicle;
using Domain_Layer.Mappers;
using Domain_Layer.Models;
using FuelTrackerAPI.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service_Layer.IService;
using System.Threading.Tasks;

namespace FuelTrackerAPI.Controllers
{
    [Route("api/vehicles")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IImageService _imageService;
        private readonly ICustomService<Vehicle> _vehicleService;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        public VehicleController(ICustomService<Vehicle> vehicleService, UserManager<ApplicationUser> userManager, ICurrentUserService currentUserService, IImageService imageService)
        {
            _vehicleService = vehicleService ?? throw new ArgumentNullException(nameof(vehicleService));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index([FromQuery] QueryObject query)
        {
            var user = await _currentUserService.GetCurrentUserAsync();

            if (user == null)
                return Unauthorized();

            var vehicles = await _vehicleService.GetAllAsync(query);

            var vehiclesDto = await vehicles
                .Where(v => v.ApplicationUserId == user.Id)
                .Select(v => v.ToVehicleDto())
                .ToListAsync();

            return Ok(vehiclesDto);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var user = await _currentUserService.GetCurrentUserAsync();

            if (user == null)
                return Unauthorized();

            var vehicle = await _vehicleService.GetByIdAsync(id);

            if (vehicle == null)
                return NotFound();

            if (vehicle.ApplicationUserId != user.Id)
                return NotFound();

            return Ok(vehicle.ToVehicleDto());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] CreateVehicleRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _currentUserService.GetCurrentUserAsync();
            var imageUrl = await _imageService.UploadImageAsync(request.Image, "vehicles");

            if (user == null)
                return Unauthorized();

            if (imageUrl == null)
                return BadRequest();

            var vehicle = request.ToVehicleFromCreate(imageUrl);

            vehicle.ApplicationUserId = user.Id;
            await _vehicleService.CreateAsync(vehicle);


            return CreatedAtAction(nameof(GetById), new { id = vehicle.Id }, vehicle.ToVehicleDto());
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] UpdateVehicleRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var imageUrl = await _imageService.UploadImageAsync(request.Image, "vehicles");
            var user = await _currentUserService.GetCurrentUserAsync();

            if (user == null)
                return Unauthorized();

            if (imageUrl == null)
                return BadRequest();

            var vehicle = await _vehicleService.GetByIdAsync(id);

            if (vehicle == null)
                return NotFound();

            if (vehicle.ApplicationUserId != user.Id)
                return NotFound();


            vehicle.Registration = request.Registration.ToUpper();
            vehicle.Image = imageUrl;
            vehicle.Make = request.Make;
            vehicle.Model = request.Model;
            vehicle.Year = request.Year;

            vehicle.ModifiedOn = DateTime.UtcNow;
            vehicle.IsActive = true;


            await _vehicleService.UpdateAsync(vehicle);

            return Ok(vehicle.ToVehicleDto());
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var user = await _currentUserService.GetCurrentUserAsync();

            if (user == null)
                return Unauthorized();

            var vehicle = await _vehicleService.GetByIdAsync(id);

            if (vehicle == null)
                return NotFound();

            if (vehicle.ApplicationUserId != user.Id)
                return NotFound();

            await _vehicleService.DeleteAsync(vehicle);
            return NoContent();
        }
    } // end class
} // end namespace
