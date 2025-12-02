using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BookNest.Utility
{
    public class EmailSender : IEmailSender
    {
		private readonly IConfiguration _config;

		public EmailSender(IConfiguration config)
		{
			_config = config;
		}

		public async Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			var apiKey = _config["SendGrid:ApiKey"];
			var client = new SendGridClient(apiKey);

			var fromAddress = _config["SendGrid:FromEmail"];
			var fromName = _config["SendGrid:FromName"];

			var from = new EmailAddress(fromAddress, fromName);
			var to = new EmailAddress(email);

			var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);
			await client.SendEmailAsync(msg);
		}
	}
}
