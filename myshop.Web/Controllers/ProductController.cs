using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using myshop.Entities.ViewModels;
using ShopHub.Business.Dtos.Product;
using ShopHub.Business.Interfaces.Services;
using ShopHub.Entities.Constants;

namespace myshop.Web.Areas.Admin.Controllers;

[Authorize(Policy = Policies.AdminAccess)]
public class ProductController(
    IWebHostEnvironment webHostEnvironment,
    IProductService productService,
    ICategoryService categoryService) : Controller
{
    private readonly IProductService _productService = productService;
    private readonly ICategoryService _categoryService = categoryService;
    private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

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
    public async Task<IActionResult> Create(ProductViewModel productViewModel, IFormFile file)
    {
        if (ModelState.IsValid)
        {
            string RootPath = _webHostEnvironment.WebRootPath;
            if (file != null)
            {
                string filename = Guid.NewGuid().ToString();
                var Upload = Path.Combine(RootPath, @"Images\Products");
                var ext = Path.GetExtension(file.FileName);

                using (var filestream = new FileStream(Path.Combine(Upload,filename+ext),FileMode.Create))
                {
                    file.CopyTo(filestream);
                }
                productViewModel.ProductDto.Img = @"Images\Products\" + filename + ext;
            }

            await _productService.CreateAsync(productViewModel.ProductDto);

            TempData["Create"] = "Item has Created Successfully";
            return RedirectToAction("Index");
        }
        return View(productViewModel.ProductDto);
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
    public async Task<IActionResult> Edit(ProductViewModel productViewModel, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            string RootPath = _webHostEnvironment.WebRootPath;

            if (file != null)
            {
                string filename = Guid.NewGuid().ToString();
                var Upload = Path.Combine(RootPath, @"Images\Products");
                var ext = Path.GetExtension(file.FileName);

                if (productViewModel.ProductDto.Img != null)
                {
                    var oldimg = Path.Combine(RootPath, productViewModel.ProductDto.Img.TrimStart('\\'));

                    if (System.IO.File.Exists(oldimg))
                    {
                        System.IO.File.Delete(oldimg);
                    }
                }

                using (var filestream = new FileStream(Path.Combine(Upload, filename + ext), FileMode.Create))
                {
                    file.CopyTo(filestream);
                }

                productViewModel.ProductDto.Img = @"Images\Products\" + filename + ext;
            }

            await _productService.UpdateAsync(productViewModel.ProductDto);

            TempData["Update"] = "Data has Updated Successfully";
            return RedirectToAction("Index");
        }

        return View(productViewModel.ProductDto);
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

        var oldimg = Path.Combine(_webHostEnvironment.WebRootPath, productIndb.Img!.TrimStart('\\'));

        if (System.IO.File.Exists(oldimg))
        {
            System.IO.File.Delete(oldimg);
        }

        return Json(new { success = true, message = "file has been Deleted" });
    }


}
