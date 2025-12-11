using System.Security.Claims;
using BookNest.DataAccess.Repository.IRepository;
using BookNest.Models;
using BookNest.Models.ViewModels;
using EllipticCurve.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookNest.Areas.Customer.Controllers
{
	[Area("Customer")]
	[Authorize]
	public class OrderController : Controller
	{
		private readonly IShoppingCartRepository _shoppingCartRepo;
		private readonly IOrderRepository _orderRepo;
		private readonly IOrderItemRepository _orderItemRepo;
		private readonly UserManager<ApplicationUser> _userManager;

		public OrderController(
			IShoppingCartRepository shoppingCartRepo,
			IOrderRepository orderRepo,
			IOrderItemRepository orderItemRepo,
			UserManager<ApplicationUser> userManager)
		{
			_shoppingCartRepo = shoppingCartRepo;
			_orderRepo = orderRepo;
			_orderItemRepo = orderItemRepo;
			_userManager = userManager;
		}

		public IActionResult Index()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

			var orders = _orderRepo.GetAll(o => o.UserId == userId)
									.OrderByDescending(o => o.OrderDate);

			return View(orders);
		}

		public IActionResult Details(Guid id)
		{
			var order = _orderRepo.Get(o => o.Id == id);

			if (order == null)
				return NotFound();

			var userId = _userManager.GetUserId(User);
			if (order.UserId != userId)
				return Unauthorized();

			var orderItems = _orderItemRepo.GetAll(oi => oi.OrderId == id,
												   includeProperties: "Product");

			var vm = new OrderDetailsViewModel
			{
				Order = order,
				OrderItems = orderItems.ToList()
			};

			return View(vm);
		}

		public IActionResult OrderConfirmation(Guid id)
		{
			var order = _orderRepo.Get(o => o.Id == id);

			if (order == null)
				return NotFound();

			return View(order);
		}

		[Authorize]
		public async Task<IActionResult> History()
		{
			var userId = _userManager.GetUserId(User);

			var orders = _orderRepo.GetAll(o => o.UserId == userId)
				.OrderByDescending(o => o.OrderDate)
				.ToList();

			return View(orders);
		}
	}
}