using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;

namespace BookNest.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            if (string.IsNullOrEmpty(email)) throw new ArgumentNullException(nameof(email));

            var section = _configuration.GetSection("EmailSettings");
            var host = section["SmtpServer"];
            var portStr = section["SmtpPort"];
            var user = section["SmtpUser"];
            var pass = section["SmtpPass"];
            var fromEmail = section["SenderEmail"] ?? user;
            var fromName = section["SenderName"] ?? "BookNest";
            var enableSsl = true;

            if (!int.TryParse(portStr, out var port))
            {
                port = 587;
            }

            if (string.IsNullOrWhiteSpace(host))
                throw new InvalidOperationException("SMTP server not configured (EmailSettings:SmtpServer).");

            var fromAddress = new MailAddress(fromEmail, fromName);

            using var message = new MailMessage
            {
                From = fromAddress,
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };
            message.To.Add(email);

            using var client = new SmtpClient(host, port)
            {
                EnableSsl = enableSsl
            };

            if (!string.IsNullOrEmpty(user))
            {
                client.Credentials = new NetworkCredential(user, pass);
            }

            await client.SendMailAsync(message);
        }
    }
}
