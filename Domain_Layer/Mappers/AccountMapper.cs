using Domain_Layer.Dtos.Account;
using Domain_Layer.Models;

namespace Domain_Layer.Mappers
{
    public static class AccountMapper
    {
        public static ApplicationUser ToUserFromSignUp(this SignUpRequest request)
        {
            return new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
            };
        }
    } // end class
} // end namespace
