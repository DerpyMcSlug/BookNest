using BookNest.DataAccess.Data;
using BookNest.DataAccess.Repository.IRepository;
using BookNest.Models;

namespace BookNest.DataAccess.Repository
{
	public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
	{
		private ApplicationDbContext _db;

		public OrderItemRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(OrderItem item)
		{
			_db.OrderItems.Update(item);
		}
	}
}