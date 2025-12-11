using System.Security.Claims;
using BookNest.DataAccess.Repository.IRepository;
using BookNest.Models;
using BookNest.Models.ViewModels;
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


		private ShoppingCartViewModel _shoppingCartVM { get; set; }

        public CartController(
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
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

			var cartItems = _shoppingCartRepo.GetAll(
				u => u.ApplicationUserId == userId,
				includeProperties: "Product"
			).ToList();

			if (cartItems.Count == 0)
			{
				return RedirectToAction("Index");
			}

			// FIX: Calculate unit prices (server-side!)
			foreach (var item in cartItems)
			{
				item.Price = item.Count <= 50
					? item.Product.Price
					: item.Count <= 100
						? item.Product.Price50
						: item.Product.Price100;
			}

			var order = new Order
			{
				UserId = userId,
				OrderDate = DateTime.UtcNow,
				Status = "Pending",
				PaymentMethod = model.PaymentMethod,
				TotalAmount = cartItems.Sum(i => (decimal)i.Price * i.Count),

				Name = model.Name,
				Phone = model.Phone,
				StreetAddress = model.StreetAddress,
				City = model.City,
				State = model.State,
				PostalCode = model.PostalCode
			};

			_orderRepo.Add(order);
			_orderRepo.Save();

			foreach (var item in cartItems)
			{
				_orderItemRepo.Add(new OrderItem
				{
					OrderId = order.Id,
					ProductId = item.ProductId,
					UnitPrice = (decimal)item.Price,
					Quantity = item.Count
				});
			}

			_orderItemRepo.Save();

			_shoppingCartRepo.RemoveRange(cartItems);
			_shoppingCartRepo.Save();

			if (model.PaymentMethod == "VNPay")
			{
				return Json(new { success = true, redirectUrl = "/MockVNPay/Pay?orderId=" + order.Id });
			}

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
    }
}
