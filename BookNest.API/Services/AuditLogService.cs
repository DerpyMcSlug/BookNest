using BookNest.Models;
using BookNest.DataAccess.Repository.IRepository;

public class AuditLogService
{
	private readonly IAuditLogRepository _repo;
	private readonly IHttpContextAccessor _http;

	public AuditLogService(
		IAuditLogRepository repo,
		IHttpContextAccessor http)
	{
		_repo = repo;
		_http = http;
	}

	public void Log(
		string actionType,
		string entityName,
		string? entityId,
		string description,
		string? userId)
	{
		_repo.Add(new AuditLog
		{
			ActionType = actionType,
			EntityName = entityName,
			EntityId = entityId,
			Description = description,
			UserId = userId,
			IpAddress = _http.HttpContext?.Connection.RemoteIpAddress?.ToString(),
			Timestamp = DateTime.Now
		});

		_repo.Save();
	}
}