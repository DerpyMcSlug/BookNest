using System.Diagnostics;
using System.Security.Claims;
using BookNest.Areas.Identity.Pages.Account;
using BookNest.DataAccess.Repository.IRepository;
using BookNest.Models;
using BookNest.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BookNest.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _productRepo;
        private readonly IShoppingCartRepository _shoppingCartRepo;

        public HomeController(ILogger<HomeController> logger, IProductRepository productRepo, IShoppingCartRepository shoppingCartRepo)
        {
            _logger = logger;
            _productRepo = productRepo;
            _shoppingCartRepo = shoppingCartRepo;
        }

		public IActionResult Index(string? search, string? category, int page = 1)
		{
			const int pageSize = 12; // 6 per row × 2 rows

			var products = _productRepo
				.GetAll(includeProperties: "Category")
				.AsQueryable();

			// 🔍 SEARCH
			if (!string.IsNullOrWhiteSpace(search))
			{
				products = products.Where(p =>
					p.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
					p.Category.Name.Contains(search, StringComparison.OrdinalIgnoreCase)
				);

				ViewBag.Title = $"Search results for \"{search}\"";
			}

			// 📚 CATEGORY
			if (!string.IsNullOrWhiteSpace(category))
			{
				products = products.Where(p =>
					p.Category.Name.Equals(category, StringComparison.OrdinalIgnoreCase)
				);

				ViewBag.Title = category;
			}

			// 🔢 PAGINATION
			var totalItems = products.Count();

			var pagedProducts = products
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToList();

			var model = new PagedResult<Product>
			{
				Items = pagedProducts,
				Page = page,
				PageSize = pageSize,
				TotalItems = totalItems
			};

			// If filtering/searching → grid view
			if (!string.IsNullOrWhiteSpace(search) || !string.IsNullOrWhiteSpace(category))
			{
				ViewBag.Search = search;
				ViewBag.Category = category;

				return View("ProductGrid", model);
			}

			// 🏠 HOME (unchanged)
			var grouped = products
				.ToList()
				.GroupBy(p => p.Category.Name)
				.OrderBy(g => g.Key)
				.ToDictionary(g => g.Key, g => g.ToList());

			return View(grouped);
		}

		public IActionResult Details(Guid productId, string? returnUrl)
		{
			var cart = new ShoppingCart
			{
				Product = _productRepo.Get(
					p => p.Id == productId,
					includeProperties: "Category"
				),
				ProductId = productId,
				Count = 1
			};

			ViewBag.ReturnUrl = returnUrl;

			return View(cart);
		}

		[HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;

            ShoppingCart cartDb = _shoppingCartRepo.Get(u => u.ApplicationUserId == userId && u.ProductId == shoppingCart.ProductId);
            Product product = _productRepo.Get(u => u.Id == shoppingCart.ProductId);

            if (cartDb == null)
            {
                _shoppingCartRepo.Add(shoppingCart);
            }
            else
            {
                cartDb.Count += shoppingCart.Count;
                _shoppingCartRepo.Update(cartDb);
            }

            _shoppingCartRepo.Save();
            TempData["success"] = $"{product.Title} is added to the cart";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}