using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookNest.Models;

namespace BookNest.DataAccess.Repository.IRepository
{
	public interface IOrderRepository : IRepository<Order>
	{
		void Update(Order order);
	}
}
