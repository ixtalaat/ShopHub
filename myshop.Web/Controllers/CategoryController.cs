using Microsoft.AspNetCore.Mvc;
using myshop.Entities.Models;
using ShopHub.Business.Interfaces.Services;

namespace myshop.Web.Areas.Admin.Controllers
{
    public class CategoryController(ICategoryService categoryService) : Controller
    {
        private readonly ICategoryService _categoryService = categoryService;

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var categories = await _categoryService.GetAllAsync(cancellationToken);
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                await _categoryService.CreateAsync(category, cancellationToken);
                TempData["Create"] = "Item has Created Successfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id, CancellationToken cancellationToken)
        {
            if (id == null | id == 0)
                return BadRequest();
            
            var categoryIndb = await _categoryService.GetByIdAsync(id!.Value, cancellationToken);

            if (categoryIndb == null)
                return NotFound();

            return View(categoryIndb);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                await _categoryService.UpdateAsync(category);

                TempData["Update"] = "Data has Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id, CancellationToken cancellationToken)
        {
            if (id == null | id == 0)
                return BadRequest();

            var categoryIndb = await _categoryService.GetByIdAsync(id!.Value, cancellationToken);

            if (categoryIndb is null)
                return NotFound();
            
            return View(categoryIndb);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int? id, CancellationToken cancellationToken)
        {
            
            await _categoryService.DeleteAsync(id!.Value);

            TempData["Delete"] = "Item has Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
