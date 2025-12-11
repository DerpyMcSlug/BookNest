using BookNest.Models;
using BookNest.DataAccess.Repository.IRepository;
using BookNest.DataAccess.Data;
namespace BookNest.DataAccess.Repository.IRepository
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        void Update(OrderDetail entity);
    }
}
