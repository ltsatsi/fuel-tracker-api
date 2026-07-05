using Domain_Layer.Dtos.Profile;
using Domain_Layer.Models;

namespace Domain_Layer.Mappers
{
    public static class ProfileMapper
    {
        public static ProfileDto ToProfileDto(this ApplicationUser user)
        {
            return new ProfileDto
            {
                UserName = user.UserName,
                Bio = user.Bio ?? "no bio yet",
                Photo = user.Photo ?? "default.png",
            };
        }
    } // end class
} // end namespace
