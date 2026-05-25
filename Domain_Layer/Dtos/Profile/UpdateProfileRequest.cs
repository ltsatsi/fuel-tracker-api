using Microsoft.AspNetCore.Http;

namespace Domain_Layer.Dtos.Profile
{
    public class UpdateProfileRequest
    {
        public IFormFile Photo { get; set; }
        public string Bio { get; set; }
    } // end class
} // end namespace
