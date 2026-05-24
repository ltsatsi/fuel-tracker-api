using Domain_Layer.Models;

namespace Service_Layer.IService
{
    public interface ICurrentUserService
    {
        Task<ApplicationUser?> GetCurrentUserAsync();
    } // end interface
} // end namespace
