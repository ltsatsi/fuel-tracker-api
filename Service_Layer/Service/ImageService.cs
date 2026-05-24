using Microsoft.AspNetCore.Http;
using Service_Layer.IService;

namespace Service_Layer.Service
{
    public class ImageService : IImageService
    {
        private readonly IStorageService _storageService;
        public ImageService(IStorageService storageService)
        {
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
        }

        public async Task<string?> UploadImageAsync(IFormFile file, string folder)
        {
            try
            {

                if (file == null || file.Length == 0)
                    return null;

                string[] allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

                var extension = Path.GetExtension(file.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                    return null;

                var maxSize = 5 * 1024 * 1024;

                if (file.Length > maxSize)
                    return null;

                return await _storageService.UploadImageAsync(file, folder);
            } catch (Exception)
            {
                throw;
            }
        }
    } // end class
} // end namespace
