using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StockIT.BLL.Services;

namespace StockIT.Pages
{
    public class ProductDeleteModel : PageModel
    {
        private IProductService _productService;
        public ProductDeleteModel(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult OnGet(int productId)
        {
            try
            {
                _productService.DeleteProduct(productId);
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
