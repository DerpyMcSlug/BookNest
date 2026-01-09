using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookNest.Models.DTOs
{
	public class AddToCartDto
	{
		public Guid ProductId { get; set; }
		public int Count { get; set; }
		public string? ReturnUrl { get; set; }
	}
}
