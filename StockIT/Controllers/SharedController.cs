using System.Text;
using Microsoft.AspNetCore.Mvc;
using StockIT.BLL.Services;

namespace StockIT.Controllers;

public class HomeController : Controller
{
    private IProductService _productService { get; set; }

    public HomeController(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IActionResult> DownloadInventory()
    {
        // 1. Retrieve product data from your service
        var products = await _productService.GetAllProductsAsync();

        // 2. Prepare data for CSV/Excel generation
        var productSummaries = products.Select(product => new
        {
            ProductId = product.Id,
            ProductModelId = product.CategoryId,
            ProductName = product.Name,
            Number = product.Quantity // Assuming "Number" refers to quantity
        });

        // 3. Determine download format based on request parameter
        var format = Request.Query["format"].ToString().ToLower();

        // 4. Generate content based on format
        if (format == "csv")
        {
            // Generate CSV content
            var csvContent = "";
            csvContent = string.Join(",", "Product ID", "Product Model(ID)", "Product Name", "Number") + Environment.NewLine; // Header row
            foreach (var productSummary in productSummaries)
            {
                csvContent += string.Join(",", productSummary.ProductId, productSummary.ProductModelId, productSummary.ProductName, productSummary.Number) + Environment.NewLine;
            }

            return Content(csvContent, "text/csv", Encoding.UTF8);
        }
        else if (format == "excel")
        {
            // Generate Excel content (optional)
            // You can use a library like EPPlus to generate Excel files: https://github.com/JanKallman/EPPlus
            // ... code to generate Excel content ...

            return Content("...", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Encoding.UTF8); // Replace with actual Excel content
        }
        else
        {
            return BadRequest("Invalid format specified. Supported formats: csv, excel");
        }
    }

}