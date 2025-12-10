using System.Security.Claims;
using BookNest.DataAccess.Repository.IRepository;
using BookNest.Models;
using BookNest.Models.ViewModels;
using EllipticCurve.Utils;
using Microsoft.AspNetCore.Authorization;
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

		public OrderController(
			IShoppingCartRepository shoppingCartRepo,
			IOrderRepository orderRepo,
			IOrderItemRepository orderItemRepo)
		{
			_shoppingCartRepo = shoppingCartRepo;
			_orderRepo = orderRepo;
			_orderItemRepo = orderItemRepo;
		}

		[HttpPost]
		public IActionResult PlaceOrder()
		{
			// 1) Get logged-in user ID
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

			// 2) Load cart with product
			var cartItems = _shoppingCartRepo.GetAll(
				u => u.ApplicationUserId == userId,
				includeProperties: "Product"
			).ToList();

			if (!cartItems.Any())
			{
				TempData["error"] = "Your cart is empty.";
				return RedirectToAction("Index", "Cart");
			}

			// 3) Calculate total price
			decimal total = 0;
			foreach (var item in cartItems)
			{
				double price = item.Count <= 50 ? item.Product.Price :
							   item.Count <= 100 ? item.Product.Price50 :
												   item.Product.Price100;

				total += (decimal)(price * item.Count);
			}

			// 4) Create Order
			var order = new Order
			{
				UserId = userId,
				OrderDate = DateTime.Now,
				TotalAmount = total,
				Status = "Pending"
			};

			_orderRepo.Add(order);
			_orderRepo.Save();

			// 5) Create Order Items
			foreach (var item in cartItems)
			{
				var orderItem = new OrderItem
				{
					OrderId = order.Id,
					ProductId = item.ProductId,
					Quantity = item.Count,
					UnitPrice = (decimal)item.Product.Price
				};

				_orderItemRepo.Add(orderItem);
			}

			_orderItemRepo.Save();

			// 6) Clear shopping cart
			_shoppingCartRepo.RemoveRange(cartItems);
			_shoppingCartRepo.Save();

			TempData["success"] = "Order placed successfully!";
			return RedirectToAction("Details", new { id = order.Id });
		}

		public IActionResult Details(Guid id)
		{
			var order = _orderRepo.Get(o => o.Id == id);

			if (order == null)
			{
				return NotFound();
			}

			var orderItems = _orderItemRepo.GetAll(oi => oi.OrderId == id,
												   includeProperties: "Product");

			var vm = new OrderDetailsViewModel
			{
				Order = order,
				OrderItems = orderItems.ToList()
			};

			return View(vm);
		}
	}
}