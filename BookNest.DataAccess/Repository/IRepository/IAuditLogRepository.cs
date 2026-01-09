using BookNest.Models;

namespace BookNest.DataAccess.Repository.IRepository
{
	public interface IAuditLogRepository : IRepository<AuditLog>
	{
		IEnumerable<AuditLog> GetByEntity(string entityName);
	}
}