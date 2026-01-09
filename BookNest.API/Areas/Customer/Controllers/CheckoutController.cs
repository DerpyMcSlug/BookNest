using Microsoft.AspNetCore.Mvc;
using BookNest.DataAccess.Repository.IRepository;
using BookNest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BookNest.Areas.Customer.Controllers
{

	[Area("Customer")]
	public class CheckoutController : Controller
	{
		private readonly IOrderRepository _orderRepo;
		private readonly IOrderItemRepository _orderItemRepo;
		private readonly IShoppingCartRepository _cartRepo;
		private readonly UserManager<ApplicationUser> _userManager;

		public CheckoutController(
			IOrderRepository orderRepo,
			IOrderItemRepository orderItemRepo,
			IShoppingCartRepository cartRepo,
			UserManager<ApplicationUser> userManager)
		{
			_orderRepo = orderRepo;
			_orderItemRepo = orderItemRepo;
			_cartRepo = cartRepo;
			_userManager = userManager;
		}

		[HttpPost]
		public IActionResult PlaceOrder()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var order = new Order
			{
				Id = Guid.NewGuid(),
				UserId = userId,
				Name = "Test User",
				Phone = "000000000",
				StreetAddress = "Test Address",
				City = "Ho Chi Minh",
				State = "VN",
				PostalCode = "700000",
				OrderDate = DateTime.UtcNow,
				Status = "Delivered",
				PaymentMethod = "VNPay",
				TotalAmount = 290000
			};

			_orderRepo.Add(order);
			_orderRepo.Save();

			// 🔥 CREATE AT LEAST ONE ORDER ITEM
			var orderItem = new OrderItem
			{
				Id = Guid.NewGuid(),
				OrderId = order.Id,
				ProductId = _cartRepo
								.GetAll(sc => sc.ApplicationUserId == userId)
								.First().ProductId,
				Quantity = 1,
				UnitPrice = order.TotalAmount
			};

			_orderItemRepo.Add(orderItem);
			_orderItemRepo.Save();

			return RedirectToAction("PaymentQR", new { orderId = order.Id });
		}
		public IActionResult PaymentQR(string orderId)
		{
			ViewBag.OrderId = orderId;
			return View("~/Areas/Customer/Views/Checkout2/PaymentQR.cshtml");
		}

		public IActionResult PaymentSuccess(string orderId)
		{
			var order = _orderRepo.Get(o => o.Id == Guid.Parse(orderId));

			if (order != null)
			{
				order.Status = "Delivered"; // or "Paid"
				_orderRepo.Update(order);
				_orderRepo.Save();
			}

			ViewBag.OrderId = orderId;
			return View("~/Areas/Customer/Views/Checkout2/PaymentSuccess.cshtml");
		}
	}
}