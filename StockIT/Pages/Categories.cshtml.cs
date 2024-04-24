using Microsoft.AspNetCore.Mvc.RazorPages;
using StockIT.BLL.Services;
using StockIT.Models;

namespace StockIT.Pages
{
    public class CategoriesModel : PageModel
    {
         private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public List<Category> Categories { get; set; } = [];
        public CategoriesModel(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

          public void OnGet()
        {
            Categories = _categoryService.GetAllCategories();
            for (var i = 0; i < Categories.Count; i++)
            {
                Categories[i].Products = _productService.GetProductsByCategory(Categories[i].Id);
            }
        }
    }
}
