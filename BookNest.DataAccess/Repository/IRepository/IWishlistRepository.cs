using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookNest.Models;

namespace BookNest.DataAccess.Repository.IRepository
{
	public interface IWishlistRepository : IRepository<Wishlist>
	{
		void RemoveByUserAndProduct(string userId, Guid productId);
	}
}