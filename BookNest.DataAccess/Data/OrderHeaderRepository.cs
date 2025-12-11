using BookNest.DataAccess.Data;
using BookNest.DataAccess.Repository.IRepository;
using BookNest.Models;

namespace BookNest.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderHeader entity)
        {
            _db.OrderHeaders.Update(entity);
        }
    }
}
