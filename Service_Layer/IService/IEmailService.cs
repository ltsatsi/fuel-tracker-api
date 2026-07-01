using Domain_Layer.Models;

namespace Service_Layer.IService
{
    public interface IEmailService
    {
        Task SendEmailAsync(Email model);
    } // end interface
} // end namespace
