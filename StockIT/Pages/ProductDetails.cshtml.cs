using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StockIT.BLL.Services;

namespace StockIT.Pages;

public class ProductDetailsModel : PageModel
{
    public ProductDetailsViewModel ProductModel { get; set; }
    private IProductService _productService { get; set; }
    private ICategoryService _categoryService { get; set; }
    public int ProductId { get; set; }
    private readonly IWebHostEnvironment _environment;
    private bool noImage { get; set; } 

    public ProductDetailsModel(IProductService productService, ICategoryService categoryService, IWebHostEnvironment environment)
    {
        _productService = productService;
        _categoryService = categoryService;
        _environment = environment;
    }
    public async Task<IActionResult> OnGetAsync(int productId)
    {
        ProductId = productId;
        var viewProduct = await _productService.GetProductByIdAsync(productId);

        if (viewProduct == null)
        {
            TempData["Success"] = "false";
            TempData["Message"] = "Product not found.";  // More specific error message
            return RedirectToPage("OperationComplete"); // Redirect to a dedicated error page
        }

        var category = _categoryService.GetCategoryById(viewProduct.CategoryId);

        if (category == null)
        {
            TempData["Success"] = "false";
            TempData["Message"] = "Category not found.";  // More specific error message
            return RedirectToPage("OperationComplete"); // Redirect to a dedicated error page
        }

        string uploadsFolder = "/uploads";

        noImage = false;
        if (string.IsNullOrEmpty(viewProduct.ImagePaths))
        {
            noImage = true;
            viewProduct.ImagePaths = "noimage.jpeg";
        }

        ProductModel = new ProductDetailsViewModel()
        {
            Name = viewProduct.Name,
            Quantity = viewProduct.Quantity,
            Description = viewProduct.Description,
            CategoryId = viewProduct.CategoryId,
            Price = viewProduct.Price,
            category = category,
            ImagePath = $"/uploads/{viewProduct.ImagePaths}",
            noImage = noImage
        };

        // Additional logic to populate Categories list (if needed)

        return Page(); // Render the page with populated data
    }
}