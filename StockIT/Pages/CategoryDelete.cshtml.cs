using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StockIT.BLL.Services;
using StockIT.Models;

namespace StockIT.Pages
{
    public class CategoryDeleteModel : PageModel
    {
        private IProductService _productService;
        private ICategoryService _categoryService;
        private List<Product> ProductList;

        public CategoryDeleteModel(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public IActionResult OnGet(int categoryId)
        {
            try
            {
                ProductList = _productService.GetProductsByCategory(categoryId);
                ProductList.ForEach(x => _productService.DeleteProduct(x.Id));
                _categoryService.DeleteCategory(categoryId);
                TempData["Success"] = "true";
                TempData["Message"] = "Products deleted successfully!";
                return RedirectToPage("OperationComplete");
            }
            catch (Exception ex)
            {
                TempData["Success"] = "false";
                TempData["Message"] = "Products delete failed: " + ex.Message; // More specific error message
                return RedirectToPage("OperationComplete");
            }
        }
    }
}
