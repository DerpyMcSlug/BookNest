using BookNest.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookNest.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = StaticDetails.ROLE_ADMIN)]
	public class ContentManagementController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}