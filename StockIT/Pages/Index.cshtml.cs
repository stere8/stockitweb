using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StockIT.BLL.Services;
using StockIT.Models;

namespace StockIT.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        public IndexViewModel ViewModel { get; set; } = new();

        public IndexModel(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }
        
        public void OnGet()
        {
            ViewModel.Categories = _categoryService.GetAllCategories().ToList();

            if (!ViewModel.Categories.Any())
            {
                ViewModel.Categories = new List<Category>();
            }
        }

        public IActionResult OnPostAddCategory(string CategoryName)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed, populate categories from OnGet
                ViewModel.Categories = _categoryService.GetAllCategories().ToList();
                return Page();
            }

            ViewModel.Categories = _categoryService.GetAllCategories().ToList();


            if (ViewModel.Categories.Any(cat => cat.Name == CategoryName))
            {
                ModelState.AddModelError("CategoryName", "Category FirstName already exists.");
            }
            _categoryService.AddCategory(new Category { Name = CategoryName });

            return RedirectToPage(); // Redirect to the same page to refresh the list
        }

        public IActionResult OnPostSelectCategory(int categoryId)
        {
            ViewModel.SelectedCategoryId = categoryId;
            ViewModel.Products = _productService.GetProductsByCategory(categoryId).ToList();
            return Page(); // Render the same page with updated view model
        }
    }
}