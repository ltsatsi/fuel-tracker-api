using Microsoft.AspNetCore.Http;
using Service_Layer.IService;

namespace Service_Layer.Service
{
    public class ImageService : IImageService
    {
        public async Task<string?> UploadImageAsync(IFormFile file, string folder)
        {
            if (file is null || file.Length == 0)
                return null;

            if (string.IsNullOrWhiteSpace(folder))
                folder = "uploads";

            string[] allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            const long maxSize = 5 * 1024 * 1024;

            if (file.Length > maxSize)
                return null;

            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                return null;

            var fileName = $"{Guid.NewGuid()}{extension}";

            var uploadsFolder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "uploads",
                folder);

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);

            await file.CopyToAsync(stream);

            return $"uploads/{folder}/{fileName}";
        } // end method
    } // end class
} // end namespace
