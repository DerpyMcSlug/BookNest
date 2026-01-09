using BookNest.DataAccess.Data;
using BookNest.DataAccess.Repository.IRepository;
using BookNest.Models;

namespace BookNest.DataAccess.Repository
{
	public class WishlistRepository : Repository<Wishlist>, IWishlistRepository
	{
		private readonly ApplicationDbContext _db;

		public WishlistRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void RemoveByUserAndProduct(string userId, Guid productId)
		{
			var item = _db.Wishlists
				.FirstOrDefault(w => w.ApplicationUserId == userId && w.ProductId == productId);

			if (item != null)
				_db.Wishlists.Remove(item);
		}
	}
}
