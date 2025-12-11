using System;
using System.Threading.Tasks;
using BookNest.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BookNest.Services
{
    public class TwoFactorService : ITwoFactorService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public TwoFactorService(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public async Task<string> GenerateAndSendAsync(ApplicationUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var code = new Random().Next(100000, 999999).ToString("D6");

            user.TwoFactorCode = code;
            user.TwoFactorExpiry = DateTime.UtcNow.AddMinutes(5);

            await _userManager.UpdateAsync(user);

            var subject = "Your BookNest 2FA code";
            var body = $"Your verification code is: <strong>{code}</strong>. It expires in 5 minutes.";

            await _emailSender.SendEmailAsync(user.Email!, subject, body);

            return code;
        }

        public async Task<bool> ValidateAsync(ApplicationUser user, string code)
        {
            if (user == null || string.IsNullOrWhiteSpace(code))
                return false;

            if (user.TwoFactorCode == code &&
                user.TwoFactorExpiry.HasValue &&
                user.TwoFactorExpiry.Value > DateTime.UtcNow)
            {
                user.TwoFactorCode = null;
                user.TwoFactorExpiry = null;

                await _userManager.UpdateAsync(user);
                return true;
            }

            return false;
        }
    }
}
