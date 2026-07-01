using Domain_Layer.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Service_Layer.IService;

namespace Service_Layer.Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
            
        public async Task SendEmailAsync(Email model)
        {
            MimeMessage email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(_configuration["Email:Sender"]));
            email.To.Add(MailboxAddress.Parse(model.To));
            email.Subject = model.Subject;

            email.Body = new TextPart("Html")
            {
                Text = model.Body,
            };

            using SmtpClient? smtp = new SmtpClient();

            await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_configuration["Email:Sender"], _configuration["Email:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    } // end class
} // end namespace
