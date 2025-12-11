using System.Threading.Tasks;
using BookNest.Models;

namespace BookNest.Services
{
    public interface ITwoFactorService
    {
        /// <summary>
        /// Generate a 6-digit code, persist it to user (TwoFactorCode/TwoFactorExpiry) and send it (email).
        /// Returns the generated code (useful for tests/logging).
        /// </summary>
        Task<string> GenerateAndSendAsync(ApplicationUser user);

        /// <summary>
        /// Validate the provided code for the user. If valid, clear stored code/expiry and return true.
        /// </summary>
        Task<bool> ValidateAsync(ApplicationUser user, string code);
    }
}