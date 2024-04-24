using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StockIT.BLL.Services;
using StockIT.Models;
using System.IO;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace StockIT.Pages;

public class ProductEditModel : PageModel
{
    [BindProperty] public ProductEditViewModel ProductModel { get; set; }
    public List<Category> Categories { get; set; }
    public string existingImagePaths { get; set; }
    private IProductService _productService { set; get; }
    private ICategoryService _categoryService { set; get; }
    private IHttpContextAccessor _contextAccessor { set; get; }

    private IWebHostEnvironment _environment { get; set; }


    public ProductEditModel(IProductService productService, ICategoryService categoryService,
        IWebHostEnvironment environment, IHttpContextAccessor contextAccessor)
    {
        _productService = productService;
        _categoryService = categoryService;
        _environment = environment;
        _contextAccessor = contextAccessor;
    }


    public async Task<IActionResult> OnGetAsync(int productId)
    {
        // Asynchronously retrieve product data for potential database operations
        var viewProduct = await _productService.GetProductByIdAsync(productId);

        if (viewProduct == null)
        {
            TempData["Success"] = "false";
            TempData["Message"] = "Product not found."; // More specific error message
            return RedirectToPage("OperationComplete"); // Redirect to a dedicated error page
        }

        existingImagePaths =
            viewProduct.ImagePaths ?? string.Empty; // Use null-conditional operator for default empty string 


        ProductModel = new ProductEditViewModel()
        {
            Name = viewProduct.Name,
            CategoryId = viewProduct.CategoryId,
            Description = viewProduct.Description,
            Id = viewProduct.Id,
            Quantity = viewProduct.Quantity,
            Price = viewProduct.Price
        };

        using (var stream = System.IO.File.OpenRead($"{_environment.WebRootPath}/uploads/{existingImagePaths}"))
        {
            ProductModel.Image = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
            {
                Headers = new HeaderDictionary(),
                ContentType =
                    GetContentTypeFromExtension(
                        Path.GetExtension(existingImagePaths)) // Function to determine content type based on extension
            };
        }

        _contextAccessor.HttpContext?.Session.SetInt32("productId", ProductModel.Id);


        // Additional logic to populate Categories list (if needed)
        Categories = _categoryService.GetAllCategories(); // Assuming a GetCategoriesAsync method

        return Page(); // Render the page with populated data
    }

    public async Task<IActionResult> OnPostAsync()
    {
        bool noNewImage = false;
        int productId = HttpContext.Session.GetInt32("productId") ?? -1; // Use default value if not found
        var previousProductValue = _productService.GetProductById(productId);
        var y = ModelState[nameof(ProductModel.Image)];
        if (ModelState[nameof(ProductModel.Image)].ValidationState == ModelValidationState.Invalid)
        {
            noNewImage = true;
            ModelState[nameof(ProductModel.Image)].ValidationState = ModelValidationState.Valid;
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        string existingImagePaths = previousProductValue.ImagePaths; // Get existing paths

        var newFileName = string.Empty;
        // Handle image uploads (optional)
        if (ProductModel.Image != null)
        {
            // Generate a unique filename
            string fileName = Path.GetRandomFileName() + Path.GetExtension(ProductModel.Image.FileName);
            // Get the wwwroot path (adjust based on your storage location)
            string uploads = Path.Combine(_environment.WebRootPath, "uploads");
            // Create uploads directory if it doesn't exist
            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }

            // Save the uploaded image to the uploads folder
            string filePath = Path.Combine(uploads, fileName);
            await using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                await ProductModel.Image.CopyToAsync(fs);
            }

            newFileName = fileName;

            // Add the new image path
        }

        var imageToBeUsed = noNewImage ? previousProductValue.ImagePaths : newFileName;


        // Handle image deletion (optional)


        var product = new Product()
        {
            Id = productId,
            Name = ProductModel.Name,
            Price = ProductModel.Price,
            Quantity = ProductModel.Quantity,
            CategoryId = ProductModel.CategoryId,
            ImagePaths = imageToBeUsed,
            Description = ProductModel.Description,
        };

        try
        {
            await _productService.UpdateProductAsync(product);
            TempData["Success"] = "True";
            TempData["Message"] = "Edit product worked"; // More specific error message
            return RedirectToPage("OperationComplete");
        }
        catch
        {
            TempData["Success"] = "false";
            TempData["Message"] = "Edit product failed"; // More specific error message
            return RedirectToPage("OperationComplete");
        }
        // other product properties..., Model.ImagePaths);

        return RedirectToPage("./ProductList");
    }

    private string GetContentTypeFromExtension(string extension)
    {
        switch (extension.ToLower())
        {
            case ".jpg":
            case ".jpeg":
                return "image/jpeg";
            case ".png":
                return "image/png";
            // Add other mappings as needed
            default:
                return "application/octet-stream"; // Use a generic type for unknown formats
        }
    }
}