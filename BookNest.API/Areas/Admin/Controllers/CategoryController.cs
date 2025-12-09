using BookNest.DataAccess.Repository.IRepository;
using BookNest.Models;
using BookNest.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookNest.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.ROLE_ADMIN)]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;
        public CategoryController(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

		public IActionResult Index()
		{
			List<Category> categories = _categoryRepo.GetAll().ToList();
			return View(categories);
		}

		public IActionResult Upsert(Guid? id)
		{
			if (id == null || id == Guid.Empty)
			{
				ViewBag.IsCreate = true;
				return PartialView("_CategoryForm", new Category());
			}

			var category = _categoryRepo.Get(c => c.Id == id);
			if (category == null)
				return NotFound();

			ViewBag.IsCreate = false;
			return PartialView("_CategoryForm", category);
		}

		[HttpPost]
		public IActionResult Upsert(Category category)
		{
			if (!ModelState.IsValid)
			{
				// Return HTML back to the modal
				return PartialView("_CategoryForm", category);
			}

			if (category.Id == Guid.Empty)
				_categoryRepo.Add(category);
			else
				_categoryRepo.Update(category);

			_categoryRepo.Save();

			return Json(new { success = true });
		}

		public IActionResult Create()
        {
			return PartialView("_CategoryForm", new Category { Id = Guid.Empty });
		}


		[HttpDelete]
		public IActionResult Delete(Guid id)
		{
			var category = _categoryRepo.Get(c => c.Id == id);
			if (category == null)
			{
				return Json(new { success = false, message = "Category not found." });
			}

			_categoryRepo.Remove(category);
			_categoryRepo.Save();

			return Json(new { success = true, message = "Category deleted successfully." });
		}

		[HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(Guid? id)
        {
            Category? category = _categoryRepo.Get(c => c.Id == id);

            if (id == Guid.Empty)
            {
                return NotFound();
            }
            _categoryRepo.Remove(category);
            _categoryRepo.Save();
            TempData["success"] = "Category deleted successfully";

            return RedirectToAction(nameof(Index));
        }

		[HttpGet]
		public IActionResult GetAll()
		{
			var categoryList = _categoryRepo.GetAll().ToList();
			return Json(new { data = categoryList });
		}
	}
}
