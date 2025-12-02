using BookNest.DataAccess.Repository.IRepository;
using BookNest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

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

		public IActionResult Index(string? search)
		{
			// Load products + category
			var allProducts = _productRepo.GetAll(includeProperties: "Category").ToList();

			// Apply search filter
			if (!string.IsNullOrWhiteSpace(search))
			{
				allProducts = allProducts.Where(p =>
					p.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
					p.Category.Name.Contains(search, StringComparison.OrdinalIgnoreCase)
				).ToList();
			}

			// Group products by category and remove empty categories
			var grouped = allProducts
				.GroupBy(p => p.Category.Name)
				.OrderBy(g => g.Key)
				.ToDictionary(g => g.Key, g => g.ToList());

			return View(grouped);
		}

		public IActionResult Details(Guid productId)
        {
            ShoppingCart shoppingCart = new ShoppingCart() 
            { 
                Product = _productRepo.Get(u => u.Id == productId, includeProperties: "Category"),
                Count = 1,
                ProductId = productId
            };
            return View(shoppingCart);
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
