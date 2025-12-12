using BookNest.Models;
using BookNest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookNest.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly BookNest.Services.ITwoFactorService _twoFactorService;

		public AccountController(
	        UserManager<ApplicationUser> userManager,
	        SignInManager<ApplicationUser> signInManager,
	        BookNest.Services.ITwoFactorService twoFactorService) // <-- fix parameter name
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_twoFactorService = twoFactorService; // <-- assigns correctly
		}

		[HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Send2FACode(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null) return NotFound();

            await _twoFactorService.GenerateAndSendAsync(user);

            // Use TempData so the Identity Razor Page can read the email
            TempData["Email2FA"] = userEmail;

            // Redirect to the Identity Razor Page that handles code entry/validation
            return Redirect("/Identity/Account/Enter2FA");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Verify2FA(string code, string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null) return NotFound();

            var ok = await _twoFactorService.ValidateAsync(user, code);
            if (ok)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home", new { area = "Customer" });
            }

            TempData["error"] = "Mã 2FA không đúng hoặc đã hết hạn!";
            TempData["Email2FA"] = userEmail;
            return Redirect("/Identity/Account/Enter2FA");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Enter2FA()
        {
            // Keep for compatibility, but redirect to the Identity Razor Page
            return Redirect("/Identity/Account/Enter2FA");
        }
    }
}
