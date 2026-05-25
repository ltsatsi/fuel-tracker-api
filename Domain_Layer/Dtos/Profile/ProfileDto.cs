using Microsoft.AspNetCore.Http;

namespace Domain_Layer.Dtos.Profile
{
    public class ProfileDto
    {
        public string UserName { get; set; }
        public string Photo { get; set; }
        public string Bio { get; set; }
    } // end class
} // end namespace
