using System.Security.Claims;
using BookNest.DataAccess.Data;
using BookNest.DataAccess.Repository;
using BookNest.DataAccess.Repository.IRepository;
using BookNest.Models;
using BookNest.Models.ViewModels;
using BookNest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace BookNest.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IShoppingCartRepository _shoppingCartRepo;
		private readonly IOrderRepository _orderRepo;
		private readonly IOrderItemRepository _orderItemRepo;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly VnPayService _vnPayService;

		private ShoppingCartViewModel _shoppingCartVM { get; set; }

        public CartController(
			IShoppingCartRepository shoppingCartRepo,
			IOrderRepository orderRepo,
			IOrderItemRepository orderItemRepo,
		    UserManager<ApplicationUser> userManager,
			VnPayService vnPayService)
		{
            _shoppingCartRepo = shoppingCartRepo;
			_orderRepo = orderRepo;
			_orderItemRepo = orderItemRepo;
			_userManager = userManager;
			_vnPayService = vnPayService;
		}

		public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            _shoppingCartVM = new ShoppingCartViewModel()
            {
                ShoppingCartList = _shoppingCartRepo.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product").ToList()
            };

            foreach (var cart in _shoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                _shoppingCartVM.OrderTotal += (cart.Price * cart.Count);
            }

            return View(_shoppingCartVM);
        }

		public async Task<IActionResult> Summary()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

			// Load cart
			_shoppingCartVM = new ShoppingCartViewModel()
			{
				ShoppingCartList = _shoppingCartRepo.GetAll(
					u => u.ApplicationUserId == userId,
					includeProperties: "Product"
				).ToList()
			};

			foreach (var cart in _shoppingCartVM.ShoppingCartList)
			{
				cart.Price = GetPriceBasedOnQuantity(cart);
				_shoppingCartVM.OrderTotal += (cart.Price * cart.Count);
			}

			// Load user from Identity system
			var user = await _userManager.FindByIdAsync(userId);

			// Pre-fill shipping information
			_shoppingCartVM.Name = user.Name;
			_shoppingCartVM.Phone = user.PhoneNumber;
			_shoppingCartVM.StreetAddress = user.StreetAddress;
			_shoppingCartVM.City = user.City;
			_shoppingCartVM.State = user.State;
			_shoppingCartVM.PostalCode = user.PostalCode;

			return View(_shoppingCartVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult SummaryPOST(ShoppingCartViewModel model)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (userId == null)
				return Json(new { success = false, message = "User not logged in" });

			var cartItems = _shoppingCartRepo.GetAll(
				u => u.ApplicationUserId == userId,
				includeProperties: "Product"
			).ToList();

			if (!cartItems.Any())
			{
				return Json(new { success = false, message = "Cart is empty" });
			}

			// Compute unit prices server-side (secure)
			foreach (var item in cartItems)
			{
				item.Price = item.Count <= 50 ? item.Product.Price :
							 item.Count <= 100 ? item.Product.Price50 :
							 item.Product.Price100;
			}

			// Create order
			var order = new Order
			{
				Id = Guid.NewGuid(),
				UserId = userId,
				OrderDate = DateTime.UtcNow,
				PaymentMethod = model.PaymentMethod,
				Status = "Pending",

				Name = model.Name,
				Phone = model.Phone,
				StreetAddress = model.StreetAddress,
				City = model.City,
				State = model.State,
				PostalCode = model.PostalCode,

				TotalAmount = cartItems.Sum(i => (decimal)i.Price * i.Count)
			};

			_orderRepo.Add(order);
			_orderRepo.Save();

			// Add order items
			foreach (var item in cartItems)
			{
				_orderItemRepo.Add(new OrderItem
				{
					Id = Guid.NewGuid(),
					OrderId = order.Id,
					ProductId = item.ProductId,
					Quantity = item.Count,
					UnitPrice = (decimal)item.Price
				});
			}

			_orderItemRepo.Save();

			// Clear cart
			_shoppingCartRepo.RemoveRange(cartItems);
			_shoppingCartRepo.Save();

			// VNPay handling
			if (model.PaymentMethod == "VNPay")
			{
				string paymentUrl = _vnPayService.CreateVnPayPayment(order);
				return Json(new { success = true, redirectUrl = paymentUrl });
			}

			// Cash on Delivery
			return Json(new { success = true, orderId = order.Id });
		}

		public IActionResult Plus(Guid cartId)
        {
            var cartFromDb = _shoppingCartRepo.Get(u => u.Id == cartId);
            cartFromDb.Count += 1;
            _shoppingCartRepo.Update(cartFromDb);
            _shoppingCartRepo.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(Guid cartId)
        {
            var cartFromDb = _shoppingCartRepo.Get(u => u.Id == cartId);
            if (cartFromDb.Count == 1)
            {
                _shoppingCartRepo.Remove(cartFromDb);
            }
            else
            {
                cartFromDb.Count -= 1;
                _shoppingCartRepo.Update(cartFromDb);
            }

            _shoppingCartRepo.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(Guid cartId)
        {
            var cartFromDb = _shoppingCartRepo.Get(u => u.Id == cartId);
            _shoppingCartRepo.Remove(cartFromDb);

            _shoppingCartRepo.Save();
            return RedirectToAction(nameof(Index));
        }

        private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Count <= 50)
            {
                return shoppingCart.Product.Price;
            }
            else
            {
                if (shoppingCart.Count <= 100)
                {
                    return shoppingCart.Product.Price50;
                }
                else
                {
                    return shoppingCart.Product.Price100;
                }
            }
        }
		public IActionResult PaymentCallbackVnpay(string orderId)
		{
			Guid id = Guid.Parse(orderId);

			var order = _orderRepo.Get(o => o.Id == id);

			if (order == null)
				return Content("Order not found");

			// Check if VNPay says success
			string vnp_ResponseCode = HttpContext.Request.Query["vnp_ResponseCode"];
			string vnp_TransactionStatus = HttpContext.Request.Query["vnp_TransactionStatus"];

			if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
			{
				order.Status = "Paid";
			}
			else
			{
				order.Status = "Payment Failed";
			}

			_orderRepo.Update(order);
			_orderRepo.Save();

			return RedirectToAction("Details", "Order", new { id = order.Id, area = "Customer" });
		}
	}
}
