using System.Security.Claims;
using BookNest.DataAccess.Repository.IRepository;
using BookNest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookNest.Areas.Customer.Controllers
{
	[Area("Customer")]
	[Authorize]
	public class WishlistController : Controller
	{
		private readonly IWishlistRepository _wishlistRepo;

		public WishlistController(IWishlistRepository wishlistRepo)
		{
			_wishlistRepo = wishlistRepo;
		}

		// ⭐ View Wishlist
		public IActionResult Index()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var items = _wishlistRepo.GetAll(
				w => w.ApplicationUserId == userId,
				includeProperties: "Product,Product.Category"
			).ToList();

			return View(items);
		}

		// ⭐ Toggle (used by your JS)
		[HttpPost]
		public IActionResult Toggle(Guid productId)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var existing = _wishlistRepo
				.GetAll(w => w.ApplicationUserId == userId && w.ProductId == productId)
				.FirstOrDefault();

			if (existing != null)
			{
				_wishlistRepo.Remove(existing);
				_wishlistRepo.Save();

				return Json(new { added = false, message = "Removed from wishlist" });
			}

			_wishlistRepo.Add(new Wishlist
			{
				ProductId = productId,
				ApplicationUserId = userId
			});

			_wishlistRepo.Save();

			return Json(new { added = true, message = "Added to wishlist ❤️" });
		}
	}
}