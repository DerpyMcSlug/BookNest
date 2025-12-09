using BookNest.DataAccess.Repository.IRepository;
using BookNest.Models;
using BookNest.Models.ViewModels;
using BookNest.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public ProductController(IProductRepository productRepo, ICategoryRepository categoryRepo, IWebHostEnvironment webHostEnvironment)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _webHostEnvironment = webHostEnvironment;
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

			return PartialView("_ProductForm", productVM);
		}

		[HttpPost]
		public IActionResult Upsert(ProductViewModel productVM, IFormFile? file)
		{
			if (!ModelState.IsValid)
			{
				productVM.CategoryList = CategoryList();
				return PartialView("_ProductForm", productVM);
			}

			string wwwRootPath = _webHostEnvironment.WebRootPath;

			if (file != null)
			{
				string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(file.FileName);
				string productPath = Path.Combine(wwwRootPath, PRODUCT_PATH);

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

			if (productVM.Product.Id == Guid.Empty)
				_productRepo.Add(productVM.Product);
			else
				_productRepo.Update(productVM.Product);

			_productRepo.Save();

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
        public IActionResult Delete(Guid? id)
        {
            Product? product = _productRepo.Get(c => c.Id == id);
            if (product == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, product.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _productRepo.Remove(product);
            _productRepo.Save();

            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion
    }
}
