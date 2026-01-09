using BookNest.DataAccess.Repository.IRepository;
using BookNest.Models;
using BookNest.Models.ViewModels;
using BookNest.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Security.Claims;
using System.Linq;

namespace BookNest.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.ROLE_ADMIN)]
    public class ProductController : Controller
    {
        private const string PRODUCT_PATH = @"images\product";
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly IAuditLogRepository _auditRepo;

		public ProductController(IProductRepository productRepo, ICategoryRepository categoryRepo, IWebHostEnvironment webHostEnvironment, IAuditLogRepository auditRepo)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _webHostEnvironment = webHostEnvironment;
			_auditRepo = auditRepo;
		}

        public IActionResult Index()
        {
            List<Product> objProductList = _productRepo.GetAll(includeProperties: "Category").ToList();
            return View(objProductList);
        }

		public IActionResult Upsert(Guid? id)
		{
			ProductViewModel productVM = new()
			{
				CategoryList = CategoryList(),
				Product = new Product()
			};

			if (id == null || id == Guid.Empty)
			{
				ViewBag.IsCreate = true;
				return PartialView("_ProductForm", productVM);
			}

			ViewBag.IsCreate = false;
			productVM.Product = _productRepo.Get(p => p.Id == id);
			if (productVM.Product == null)
			{
				return NotFound();
			}

			return PartialView("_ProductForm", productVM);
		}

		[HttpPost]
		[IgnoreAntiforgeryToken]
		public IActionResult Upsert([FromForm] ProductViewModel productVM, IFormFile? file)
		{
			if (!ModelState.IsValid)
			{
				productVM.CategoryList = CategoryList();
				return PartialView("_ProductForm", productVM);
			}

			string wwwRootPath = _webHostEnvironment.WebRootPath;
			bool isCreate = productVM.Product.Id == Guid.Empty;

			if (file != null)
			{
				string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(file.FileName);
				string productPath = Path.Combine(wwwRootPath, PRODUCT_PATH);

				if (!Directory.Exists(productPath))
				{
					Directory.CreateDirectory(productPath);
				}
				if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
				{
					string oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
					if (System.IO.File.Exists(oldImagePath))
						System.IO.File.Delete(oldImagePath);
				}

				using (FileStream fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
				{
					file.CopyTo(fileStream);
				}

				productVM.Product.ImageUrl = @"\" + PRODUCT_PATH + @"\" + fileName;
			}

			if (isCreate)
				_productRepo.Add(productVM.Product);
			else
			{
				var productFromDb = _productRepo.Get(p => p.Id == productVM.Product.Id);

				if (productFromDb == null)
					return Json(new { success = false, message = "Product not found" });

				productFromDb.Title = productVM.Product.Title;
				productFromDb.Description = productVM.Product.Description;
				productFromDb.ISBN = productVM.Product.ISBN;
				productFromDb.Author = productVM.Product.Author;
				productFromDb.ListPrice = productVM.Product.ListPrice;
				productFromDb.Price = productVM.Product.Price;
				productFromDb.Price50 = productVM.Product.Price50;
				productFromDb.Price100 = productVM.Product.Price100;
				productFromDb.CategoryId = productVM.Product.CategoryId;

				// only overwrite image if new file uploaded
				if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
				{
					productFromDb.ImageUrl = productVM.Product.ImageUrl;
				}
			}

			_productRepo.Save();

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "SYSTEM";
			_auditRepo.Add(new AuditLog
			{
				UserId = userId ?? null,
				ActionType = isCreate ? "Create" : "Update",
				EntityName = "Product",
				EntityId = productVM.Product.Id.ToString(),
				Description = isCreate
					? $"Created product '{productVM.Product.Title}'"
					: $"Updated product '{productVM.Product.Title}'",
				IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
				Timestamp = DateTime.Now
			});

			_auditRepo.Save();

			return Json(new { success = true });
		}

		private IEnumerable<SelectListItem> CategoryList()
        {
            return _categoryRepo.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString(),
            });
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _productRepo.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }

		[HttpDelete]
		[IgnoreAntiforgeryToken]
		public IActionResult Delete(Guid? id)
		{
			if (id == null)
				return Json(new { success = false, message = "Invalid product ID" });

			var product = _productRepo.Get(p => p.Id == id, includeProperties: "OrderItems");
			if (product == null)
				return Json(new { success = false, message = "Product not found" });

			_productRepo.Remove(product);
			_productRepo.Save();

			return Json(new { success = true, message = "Delete successful" });
		}
		#endregion
	}
}
