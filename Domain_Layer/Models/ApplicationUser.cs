using Microsoft.AspNetCore.Identity;

namespace Domain_Layer.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Photo { get; set; }
        public string? Bio { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();   
    } // end class
} // end namespace
