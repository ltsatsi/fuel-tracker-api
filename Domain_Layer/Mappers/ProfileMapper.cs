using Domain_Layer.Dtos.Profile;
using Domain_Layer.Models;

namespace Domain_Layer.Mappers
{
    public static class ProfileMapper
    {
        public static ProfileDto ToProfileDto(this ApplicationUser user, string? publicImageUrl)
        {
            return new ProfileDto
            {
                UserName = user.UserName,
                Bio = user.Bio ?? "no bio yet",
                Photo = publicImageUrl ?? user.Photo,
            };
        }
    } // end class
} // end namespace
