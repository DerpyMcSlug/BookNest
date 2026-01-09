using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookNest.Models
{
	public class AuditLog : BaseModel
	{
		public Guid Id { get; set; } = Guid.NewGuid();

		public string? UserId { get; set; }
		public ApplicationUser? User { get; set; }

		public string ActionType { get; set; }
		public string? EntityName { get; set; }
		public string? EntityId { get; set; }

		public string Description { get; set; }
		public string? IpAddress { get; set; }

		public DateTime Timestamp { get; set; }
	}
}
