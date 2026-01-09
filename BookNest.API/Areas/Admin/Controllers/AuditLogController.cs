using BookNest.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookNest.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class AuditLogController : Controller
	{
		private readonly IAuditLogRepository _auditRepo;

		public AuditLogController(IAuditLogRepository auditRepo)
		{
			_auditRepo = auditRepo;
		}

		public IActionResult Index(string tab = "books")
		{
			var logs = _auditRepo.GetAll(includeProperties: "User")
								 .OrderByDescending(l => l.Timestamp);

			ViewBag.Tab = tab;
			return View(logs);
		}
	}
}