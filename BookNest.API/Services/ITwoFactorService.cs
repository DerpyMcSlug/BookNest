using System.Threading.Tasks;
using BookNest.Models;

namespace BookNest.Services
{
    public interface ITwoFactorService
    {
        Task<string> GenerateAndSendAsync(ApplicationUser user);
        Task<bool> ValidateAsync(ApplicationUser user, string code);
		Task<bool> CanResendAsync(ApplicationUser user);
		Task<int> GetRemainingCooldownAsync(ApplicationUser user);
		Task SetResendTimestampAsync(ApplicationUser user);
	}
}