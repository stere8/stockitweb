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
        try
        {
            var categoryId = int.Parse(Request.Query["categoryId"]);
            var productIdList =_productService.GetProductsByCategory(categoryId);
            foreach (var product in productIdList)
            {
                _productService.DeleteProduct(product.Id);
            }
            _categoryService.DeleteCategory(categoryId);
            TempData["Success"] = "true";
            TempData["Message"] = "Category deleted successfully!";
            return RedirectToPage("/OperationComplete");
        }
        catch (Exception ex)
        {
            TempData["Success"] = "false";
            TempData["Message"] = "Category delete failed: " + ex.Message; // More specific error message
            return RedirectToPage("/OperationComplete");
        }
    }
}