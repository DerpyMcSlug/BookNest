using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookNest.Models.ViewModels
{
	public class OrderDetailsViewModel
	{
		public Order Order { get; set; }
		public IEnumerable<OrderItem> OrderItems { get; set; }
	}
}