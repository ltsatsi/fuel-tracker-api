using Microsoft.AspNetCore.Http;

namespace Service_Layer.IService
{
    public interface IStorageService
    {
        Task<string> GetImageUrlAsync(string path);
        Task<string> UploadImageAsync(IFormFile file, string folder);
        Task DeleteImageAsync(string path);
    } // end interface
} // end namespace
