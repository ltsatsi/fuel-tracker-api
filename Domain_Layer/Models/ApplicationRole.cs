using Microsoft.AspNetCore.Identity;

namespace Domain_Layer.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string roleName)
            :base(roleName)
        {
            
        }
        public string Desccription { get; set; }
        public bool IsActive { get; set; }  
    } // end class
} // end namespace
