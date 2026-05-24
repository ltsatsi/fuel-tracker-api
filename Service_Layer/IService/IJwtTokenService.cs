using Domain_Layer.Models;

namespace Service_Layer.IService
{
    public interface IJwtTokenService
    {
        string CreateJwtToken(ApplicationUser user);
    } // end interface
} // end namespace
