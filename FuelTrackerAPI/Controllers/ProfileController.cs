using Domain_Layer.Dtos.Profile;
using Domain_Layer.Mappers;
using Domain_Layer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service_Layer.IService;

namespace FuelTrackerAPI.Controllers
{
    [Route("api/profile")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IImageService _imageService;
        private readonly IStorageService _storageService;
        public ProfileController(UserManager<ApplicationUser> userManager, ICurrentUserService currentUserService, IImageService imageService, IStorageService storageService)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var user = await _currentUserService.GetCurrentUserAsync();

            if (user == null)
                return NotFound();

            var publicImageUrl = await _storageService.GetImageUrlAsync(user.Photo);

            return Ok(user.ToProfileDto(publicImageUrl));
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> EditProfile([FromForm] UpdateProfileRequest request)
        {
            var user = await _currentUserService.GetCurrentUserAsync();

            if (user == null)
                return NotFound();

            var tempImage = user.Photo;

            user.Bio = request.Bio;
            user.Photo = await _imageService.UploadImageAsync(request.Photo, "profiles");

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                if (tempImage != null)
                {
                    await _storageService.DeleteImageAsync(tempImage);
                }
            }

            var publicImageUrl = await _storageService.GetImageUrlAsync(user.Photo);
            return Ok(user.ToProfileDto(publicImageUrl));
        }
    } // end class
} // end namespace
