using Microsoft.AspNetCore.Mvc;
using StockIT.BLL.Services;

namespace StockIT.Controllers;

public class CategoryController : Controller
{
    private IProductService _productService { get; set; }
    private ICategoryService _categoryService { get; set; }


public CategoryController(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }
    public IActionResult DeleteCategory()
    {
        var productId = int.Parse(Request.Query["productId"]);
        try
        {
            _productService.DeleteProduct(productId);
            TempData["Success"] = "true";
            TempData["Message"] = "Products deleted successfully!";
            return RedirectToPage("/OperationComplete");
        }
        catch (Exception ex)
        {
            TempData["Success"] = "false";
            TempData["Message"] = "Products delete failed: " + ex.Message; // More specific error message
            return RedirectToPage("/OperationComplete");
        }
    }
}