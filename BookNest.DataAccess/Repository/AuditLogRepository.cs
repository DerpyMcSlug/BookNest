using BookNest.DataAccess.Data;
using BookNest.DataAccess.Repository.IRepository;
using BookNest.Models;

namespace BookNest.DataAccess.Repository
{
	public class AuditLogRepository : Repository<AuditLog>, IAuditLogRepository
	{
		private readonly ApplicationDbContext _db;

		public AuditLogRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public IEnumerable<AuditLog> GetByEntity(string entityName)
		{
			return _db.AuditLogs
					 .Where(a => a.EntityName == entityName)
					 .OrderByDescending(a => a.Timestamp)
					 .ToList();
		}
	}
}