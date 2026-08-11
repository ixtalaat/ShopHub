using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopHub.Web.ViewModels;
using ShopHub.Business.Dtos.Product;
using ShopHub.Business.Interfaces.Services;
using ShopHub.Entities.Constants;

namespace ShopHub.Web.Areas.Admin.Controllers;

[Authorize(Policy = Policies.AdminAccess)]
public class ProductController(
    IProductService productService,
    ICategoryService categoryService) : Controller
{
    private readonly IProductService _productService = productService;
    private readonly ICategoryService _categoryService = categoryService;

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetData()
    {
        var productListDto = await _productService.GetAllWithCategoryAsync();

        return Json(new { data = productListDto });
    }

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var categoriesDto = await _categoryService.GetAllAsync(cancellationToken);

        ProductViewModel productViewModel = new ProductViewModel()
        {
            ProductDto = new ProductDto(),
            CategoryList = categoriesDto.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            })
        };
        return View(productViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductViewModel productViewModel, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            await _productService.CreateAsync(productViewModel.ProductDto);

            TempData["Create"] = "Item has Created Successfully";
            return RedirectToAction("Index");
        }
        var categoriesDto = await _categoryService.GetAllAsync(cancellationToken);
        productViewModel.CategoryList = categoriesDto.Select(x => new SelectListItem
        {
            Text = x.Name,
            Value = x.Id.ToString()
        });
        return View(productViewModel);
    }
    [HttpGet]
    public async Task<IActionResult> Edit(int? id, CancellationToken cancellationToken)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var prodcutDto = await _productService.GetByIdAsync(id.Value, cancellationToken);
        var categories = await _categoryService.GetAllAsync(cancellationToken);
        ProductViewModel productViewModel = new ProductViewModel()
        {
            ProductDto = prodcutDto!,
            CategoryList = categories.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            })
        };

        return View(productViewModel);
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit(ProductViewModel productViewModel, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            await _productService.UpdateAsync(productViewModel.ProductDto, cancellationToken);

            TempData["Update"] = "Data has Updated Successfully";
            return RedirectToAction("Index");
        }
        var categories = await _categoryService.GetAllAsync(cancellationToken);
        productViewModel.CategoryList = categories.Select(x => new SelectListItem
        {
            Text = x.Name,
            Value = x.Id.ToString()
        });
        return View(productViewModel);
    }
    
    [HttpDelete]
    public async Task<IActionResult> Delete(int? id, CancellationToken cancellationToken)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var productIndb = await _productService.GetByIdAsync(id!.Value, cancellationToken);

        if (productIndb == null)
        {
            return Json(new { success = false, message = "Error while Deleting" });
        }

        await _productService.DeleteAsync(productIndb.Id);

        return Json(new { success = true, message = "file has been Deleted" });
    }


}
