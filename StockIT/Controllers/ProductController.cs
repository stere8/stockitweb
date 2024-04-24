using Microsoft.AspNetCore.Mvc;
using StockIT.BLL.Services;
using StockIT.Models;

namespace StockIT.Controllers
{
    public class ProductController : Controller
    {
        private IProductService _productService;
        public ProductController(IProductService productService)
        {
             _productService= productService;
        }
        public IActionResult DeleteProduct()
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
}
