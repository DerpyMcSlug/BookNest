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

		public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("", "The Category Name cannot be the same as the Display Order");
            }

            if (ModelState.IsValid)
            {
                _categoryRepo.Add(category);
                _categoryRepo.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        public IActionResult Edit(Guid? id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            Category? category = _categoryRepo.Get(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {                
                _categoryRepo.Update(category);
                _categoryRepo.Save();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction(nameof(Index));
            }

            return View();
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
