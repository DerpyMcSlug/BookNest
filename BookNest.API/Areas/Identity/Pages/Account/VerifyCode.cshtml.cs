#nullable disable
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BookNest.Models;
using BookNest.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookNest.Areas.Identity.Pages.Account
{
	public class VerifyCodeModel : PageModel
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ITwoFactorService _twoFactorService;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public VerifyCodeModel(
			UserManager<ApplicationUser> userManager,
			ITwoFactorService twoFactorService,
			SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_twoFactorService = twoFactorService;
			_signInManager = signInManager;
		}

		[BindProperty]
		public InputModel Input { get; set; }
		public string ReturnUrl { get; set; }
		public string Email { get; set; }
		public bool RememberMe { get; set; }


		public class InputModel
		{
			[Required]
			public string Email { get; set; }

			[Required]
			[Display(Name = "Verification Code")]
			public string Code { get; set; }
		}

		public void OnGet(string email, bool rememberMe, string returnUrl = null)
		{
			Email = email;
			RememberMe = rememberMe;
			ReturnUrl = returnUrl;
			Input = new InputModel { Email = email };
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
				return Page();

			var user = await _userManager.FindByEmailAsync(Input.Email);
			if (user == null)
			{
				ModelState.AddModelError(string.Empty, "Invalid verification attempt.");
				return Page();
			}

			// ✅ VALIDATE OTP
			var valid = await _twoFactorService.ValidateAsync(user, Input.Code);
			if (!valid)
			{
				ModelState.AddModelError(string.Empty, "Invalid or expired code.");
				return Page();
			}

			// ✅ OTP PASSED → REMEMBER IT
			user.LastOtpVerifiedAt = DateTime.UtcNow;
			await _userManager.UpdateAsync(user);

			// ✅ SIGN IN
			await _signInManager.SignInAsync(user, RememberMe);

			return !string.IsNullOrEmpty(ReturnUrl)
				? LocalRedirect(ReturnUrl)
				: Redirect("~/");
		}

		// -----------------------------
		// 🔥 RESEND OTP HANDLER HERE
		// -----------------------------
		public async Task<IActionResult> OnPostResendAsync()
		{
			var user = await _userManager.FindByEmailAsync(Input.Email);
			if (user == null)
			{
				ModelState.AddModelError(string.Empty, "User not found.");
				return Page();
			}

			// Fallback cooldown logic since ITwoFactorService does not define CanResendAsync
			const int cooldownSeconds = 30;
			var now = DateTime.UtcNow;
			var lastSent = user.LastOtpSentAt ?? DateTime.MinValue;
			var secondsSinceLast = (int)(now - lastSent).TotalSeconds;
			if (secondsSinceLast < cooldownSeconds)
			{
				int remaining = cooldownSeconds - secondsSinceLast;
				ModelState.AddModelError(string.Empty, $"You can resend a new code in {remaining} seconds.");
				ViewData["CooldownRemaining"] = remaining;
				return Page();
			}

			// Send new OTP
			await _twoFactorService.GenerateAndSendAsync(user);

			// Update LastOtpSentAt
			user.LastOtpSentAt = now;
			await _userManager.UpdateAsync(user);

			ModelState.AddModelError(string.Empty, "A new code has been sent.");
			ViewData["CooldownRemaining"] = cooldownSeconds; // JS uses this
			return Page();
		}

	}
}
