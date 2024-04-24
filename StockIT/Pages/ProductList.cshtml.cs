using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StockIT.BLL.Services;
using StockIT.Models;

namespace StockIT.Pages
{
    public class ProductListModel : PageModel
    {
        private readonly IProductService _productService;
        public List<Product> ProductsList { get; set; } = [];

        public ProductListModel(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public void OnGet(int categoryId, string searchQuery) // Filters by Category and search query
        {
            ProductsList = _productService.SearchProducts(categoryId, searchQuery);
        }
    }

}