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
		private const int CooldownSeconds = 30;

		public TwoFactorService(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
		{
			_userManager = userManager;
			_emailSender = emailSender;
		}

		public async Task<string> GenerateAndSendAsync(ApplicationUser user)
		{
			// Generate 6-digit OTP
			var random = new Random();
			string code = random.Next(100000, 999999).ToString();

			// Store in user record
			user.TwoFactorCode = code;
			user.TwoFactorExpiry = DateTime.UtcNow.AddMinutes(5);

			await _userManager.UpdateAsync(user);

			// Send email
			await _emailSender.SendEmailAsync(
				user.Email,
				"BookNest Verification Code",
				$"Your verification code is: <b>{code}</b><br>This code expires in 5 minutes."
			);

			return code;
		}

		public async Task<bool> ValidateAsync(ApplicationUser user, string code)
		{
			if (string.IsNullOrEmpty(user.TwoFactorCode) ||
				user.TwoFactorExpiry == null ||
				user.TwoFactorExpiry < DateTime.UtcNow)
				return false;

			if (user.TwoFactorCode != code)
				return false;

			// Clear after successful validation
			user.TwoFactorCode = null;
			user.TwoFactorExpiry = null;
			await _userManager.UpdateAsync(user);

			return true;
		}
		public Task<bool> CanResendAsync(ApplicationUser user)
		{
			if (user.LastOtpSentAt == null) return Task.FromResult(true);
			var elapsed = DateTime.UtcNow - user.LastOtpSentAt.Value;
			return Task.FromResult(elapsed.TotalSeconds >= 30);
		}

		public Task<int> GetRemainingCooldownAsync(ApplicationUser user)
		{
			if (user.LastOtpSentAt == null) return Task.FromResult(0);
			var elapsed = DateTime.UtcNow - user.LastOtpSentAt.Value;
			int remaining = 30 - (int)elapsed.TotalSeconds;
			if (remaining < 0) remaining = 0;
			return Task.FromResult(remaining);
		}

		public async Task SetResendTimestampAsync(ApplicationUser user)
		{
			user.LastOtpSentAt = DateTime.UtcNow;
			await _userManager.UpdateAsync(user);
		}

	}
}