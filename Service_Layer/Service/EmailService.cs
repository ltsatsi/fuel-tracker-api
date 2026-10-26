using Domain_Layer.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Service_Layer.IService;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Service_Layer.Service
{
    public class EmailService : IEmailService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public EmailService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }


        public async Task SendEmailAsync(Email model)
        {
            var sender = _configuration["Email:Sender"];
            var apiKey = _configuration["Email:EmailApiKey"];
            var senderName = _configuration["Email:SenderName"] ?? "Fuel Tracker";

            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://api.brevo.com/v3/smtp/email");

            request.Headers.Add("api-key", apiKey);
            request.Headers.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            request.Content = JsonContent.Create(new
            {
                sender = new { name = senderName, email = sender },
                to = new[] { new { email = model.To } },
                subject = model.Subject,
                htmlContent = model.Body
            });

            using var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var details = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException(
                    $"Brevo failed ({(int)response.StatusCode}): {details}");
            }
        }
    } // end class
} // end namespace
