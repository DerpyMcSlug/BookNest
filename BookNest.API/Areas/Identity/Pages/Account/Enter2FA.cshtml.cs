using System.ComponentModel.DataAnnotations;
using BookNest.Models;
using BookNest.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookNest.Areas.Identity.Pages.Account
{

    public class Enter2FAModel(UserManager<ApplicationUser> userManager,
                               ITwoFactorService twoFactorService,
                               SignInManager<ApplicationUser> signInManager) : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ITwoFactorService _twoFactorService = twoFactorService;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        [TempData]
        public string? Email2FA { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Please enter the 6-digit code")]
            [StringLength(6, MinimumLength = 6, ErrorMessage = "Code must be 6 digits")]
            public string? Code { get; set; }
        }

        public void OnGet()
        {
            // Email2FA is provided via TempData from login flow
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            if (string.IsNullOrEmpty(Email2FA))
            {
                ModelState.AddModelError(string.Empty, "Không có email để xác thực. Vui lòng đăng nhập lại.");
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Email2FA);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User account not found.");
                return Page();
            }

            var valid = await _twoFactorService.ValidateAsync(user, Input.Code ?? string.Empty);
            if (!valid)
            {
                ModelState.AddModelError(string.Empty, "Invalid or expired code.");
                return Page();
            }

            // Sign in the user (non-persistent here; change as needed)
            await _signInManager.SignInAsync(user, isPersistent: false);

            return LocalRedirect("~/");
        }

        /// <summary>
        /// Endpoint to resend the 2FA code (POST to same page with ?resend=1 or call via AJAX)
        /// </summary>
        public async Task<IActionResult> OnPostResendAsync()
        {
            if (string.IsNullOrEmpty(Email2FA))
            {
                return BadRequest("No email to resend to.");
            }

            var user = await _userManager.FindByEmailAsync(Email2FA);
            if (user == null) return NotFound();

            await _twoFactorService.GenerateAndSendAsync(user);
            TempData["StatusMessage"] = "A new code was sent to your email.";
            return RedirectToPage();
        }
    }
}