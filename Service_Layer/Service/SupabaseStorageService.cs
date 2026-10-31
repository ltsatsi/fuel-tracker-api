using Microsoft.AspNetCore.Http;
using Service_Layer.IService;

namespace Service_Layer.Service
{
    public class SupabaseStorageService : IStorageService
    {
        private const string _bucket = "images";    
        private readonly Supabase.Client _supabase;
        public SupabaseStorageService(Supabase.Client supabase)
        {
            _supabase = supabase ?? throw new ArgumentNullException(nameof(supabase));
        }

            

        public async Task<string> GetImageUrlAsync(string path)
        {
            var image =  _supabase.Storage.From(_bucket).GetPublicUrl(path);
            return image.TrimEnd('?');
        }

        public async Task DeleteImageAsync(string path)
        {
            await _supabase.Storage.From(_bucket).Remove(new List<string> { path });
        }

        public async Task<string> UploadImageAsync(IFormFile file, string folder)
        {
            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{folder}/{Guid.NewGuid()}{extension}";

            using var stream = new MemoryStream();

            await file.CopyToAsync(stream);

            var bytes = stream.ToArray();

            await _supabase.Storage.From(_bucket).Upload(bytes, fileName);

            return fileName;
        }
    }
}
