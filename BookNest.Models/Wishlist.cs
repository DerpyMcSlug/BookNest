using System.ComponentModel.DataAnnotations.Schema;

namespace BookNest.Models
{
	public class Wishlist
	{
		public Guid Id { get; set; }

		public string ApplicationUserId { get; set; }

		[ForeignKey(nameof(ApplicationUserId))]
		public ApplicationUser ApplicationUser { get; set; }

		public Guid ProductId { get; set; }

		[ForeignKey(nameof(ProductId))]
		public Product Product { get; set; }
	}
}