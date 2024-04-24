using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StockIT.BLL.Services;
using StockIT.Models;

namespace StockIT.Pages
{
    public class CategoryEditModel : PageModel
    {
        [BindProperty] // Include "Id" in BindProperty
        public CategoryEditView CategoryModel { get; set; }

        public int id { get; set; }
        private ICategoryService _categoryService { get; set; }
        private IProductService _productService { get; set; }

        public CategoryEditModel(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        public void OnGet(int categoryId)
        {
            var viewCategory = _categoryService.GetCategoryById(categoryId);
            CategoryModel = new CategoryEditView()
            {
                Id = viewCategory.Id,
                Name = viewCategory.Name
            };

            HttpContext.Session.SetInt32("categoryId", CategoryModel.Id);
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                // Handle validation errors
                return Page();
            }

            var products = _productService.GetProductsByCategory(CategoryModel.Id);

            int categoryId = HttpContext.Session.GetInt32("categoryId") ?? -1; // Use default value if not found

            var categoryToSave = new Category()
            {
                Id = categoryId,
                Name = CategoryModel.Name,
                Products = products
            };

            try
            {
                _categoryService.UpdateCategory(categoryToSave);
                TempData["Success"] = "true";
                TempData["Message"] = "Category edited successfully!";
                return RedirectToPage("OperationComplete");
            }
            catch (Exception ex)
            {
                TempData["Success"] = "false";
                TempData["Message"] = "Edit category failed: " + ex.Message; // More specific error message
                return RedirectToPage("OperationComplete");
            }
        }
    }
}