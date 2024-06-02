using ApplicationCore.DTO;
using ApplicationCore.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Infrastructure.Services.EmailServices
{
    public class MailKitService : IEmailService
    {
        private readonly IConfiguration _config;

        public MailKitService(IConfiguration configuration)
        {
            _config = configuration;
        }

        public void SendEmail(EmailDto request)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_config["Email:SenderName"], _config["Email:SenderEmail"]));
            message.To.Add(new MailboxAddress("", request.To));
            message.Subject = request.Subject;
            message.Body = new TextPart("html") { Text = request.Body};

            using (var client = new SmtpClient())
            {
                client.Connect(_config["Email:Host"], 587, SecureSocketOptions.StartTls);
                client.Authenticate(_config["Email:SenderEmail"], _config["Email:Password"]);
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
