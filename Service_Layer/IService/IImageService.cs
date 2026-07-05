using Microsoft.AspNetCore.Http;

namespace Service_Layer.IService
{
    public interface IImageService
    {
        Task<string?> UploadImageAsync(IFormFile file, string folder = "misc");
    } // end interface
} // end namespace
