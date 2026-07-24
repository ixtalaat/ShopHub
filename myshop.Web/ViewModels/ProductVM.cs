using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using myshop.Entities.Models;

namespace myshop.Entities.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; } = null!;
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; } = null!;
    }
}
