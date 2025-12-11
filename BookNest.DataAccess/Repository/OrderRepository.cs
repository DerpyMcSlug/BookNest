using BookNest.DataAccess.Data;
using BookNest.DataAccess.Repository.IRepository;
using BookNest.Models;

namespace BookNest.DataAccess.Repository
{
	public class OrderRepository : Repository<Order>, IOrderRepository
	{
		private ApplicationDbContext _db;

		public OrderRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(Order order)
		{
			_db.Orders.Update(order);
		}
	}
}