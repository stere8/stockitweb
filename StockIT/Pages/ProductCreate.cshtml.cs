using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StockIT.BLL.Services;
using StockIT.Models;
using static System.Net.Mime.MediaTypeNames;

namespace StockIT.Pages;

public class ProductCreateModel : PageModel
{
    [BindProperty] public ProductCreateViewModel ProductModel { get; set; } = new();
    private readonly IWebHostEnvironment _environment;
    public List<Category> Categories { get; set; }
    private readonly IProductService _productService;

    private readonly ICategoryService _categoryService;

    public ProductCreateModel(IProductService productService, ICategoryService categoryService, IWebHostEnvironment environment)
    {
        _productService = productService;
        _categoryService = categoryService;
        _environment = environment;
    }


    public IActionResult OnGet()
    {
        Categories = _categoryService.GetAllCategories();
        if (!Categories.Any())
        {
            TempData["Success"] = "false";
            TempData["Message"] = "Please add some categories first.";
            return RedirectToPage("Index");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            var errors = new List<string>();
            foreach (var modelStateEntry in ModelState.Values)
            {
                if (modelStateEntry.ValidationState == ModelValidationState.Invalid)
                {
                    foreach (var error in modelStateEntry.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
            }

            TempData["Success"] = "false";
            TempData["Message"] = "Please correct validation errors.";
            errors.ForEach(error => ViewData["Message"] += $"\n {error}");
            return RedirectToPage("OperationComplete"); // Redirect to operationcomplete.cshtml
        }

        if (ProductModel.Image != null)
        {
            string fileName = Path.GetRandomFileName() + Path.GetExtension(ProductModel.Image.FileName);
            string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            string filePath = Path.Combine(uploadsFolder, fileName);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await ProductModel.Image.CopyToAsync(fileStream);
            }

            var newProduct = new Product
            {
                Name = ProductModel.Name,
                Description = ProductModel.Description,
                Quantity = ProductModel.Quantity,
                CategoryId = ProductModel.CategoryId, // Assuming CategoryId is populated in ProductCreateViewModel
                Price = ProductModel.Price,
                ImagePaths = fileName
            };

            try
            {
                _productService.AddProduct(newProduct);
                TempData["Success"] = "true";
                TempData["Message"] = "Oepration succedd!";
                return RedirectToPage("OperationComplete"); // Redirect to OperationComplete.cshtml
            }
            catch (Exception ex)
            {
                TempData["Success"] = "false";
                TempData["Message"] = $"There was an error while adding {ex}"; // Generic error message
                return RedirectToPage("OperationComplete"); // Redirect to OperationComplete.cshtml
            }
        }

        return Page();
    }
}