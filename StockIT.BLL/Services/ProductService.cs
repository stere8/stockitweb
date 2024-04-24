using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StockIT.BLL.Exceptions;
using StockIT.Models;

namespace StockIT.BLL.Services;

public class ProductService : IProductService
{
    private StockITContext _stockItContext;
    private readonly ILogger<ProductService> _logger; // Injecting logger

    public ProductService(StockITContext context, ILogger<ProductService> logger)
    {
        _stockItContext = context;
        _logger = logger;
    }

    public List<Product> GetAllProducts()
    {
        try
        {
            return _stockItContext.Products.ToList();
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, "Error retrieving products");
            throw new ProductServiceException("An error occurred while retrieving products.");
        }
    }

    public Product GetProductById(int id)
    {
        try
        {
            return _stockItContext.Products.FirstOrDefault(p => p.Id == id);
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, $"Error getting product with ID {id}");
            throw new ProductServiceException($"Product with ID {id} not found."); // Throw a custom exception
        }
    }


    public void AddProduct(Product product)
    {
        try
        {
            _stockItContext.Products.Add(product);
            _stockItContext.SaveChanges();
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, $"Error adding product: {product.Name}"); // Log product FirstName
            throw new ProductServiceException("An error occurred while adding a product."); // Throw a generic exception
        }
    }

    public void UpdateProduct(Product product)
    {
        try
        {
            _stockItContext.Products.Attach(product);
            _stockItContext.Entry(product).State = EntityState.Modified;
            _stockItContext.SaveChanges();
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, $"Error updating product with ID {product.Id}");
            throw new ProductServiceException(
                $"An error occurred while updating product with ID {product.Id}."); // Throw a specific exception
        }
    }

    public void DeleteProduct(int id)
    {
        try
        {
            var product = _stockItContext.Products.Find(id);
            if (product != null)
            {
                _stockItContext.Products.Remove(product);
                _stockItContext.SaveChanges();
            }
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, $"Error deleting product with ID {id}");
            throw new ProductServiceException(
                $"An error occurred while deleting product with ID {id}."); // Throw a specific exception
        }
    }

    public List<Product> GetProductsByCategory(int categoryId)
    {
        try
        {
            var category = _stockItContext.Categories.FirstOrDefault(cat => cat.Id == categoryId);
            if (category != null)
            {
                return _stockItContext.Products.Where(prod => prod.CategoryId == category.Id).ToList();
            }

            return new List<Product>();
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, $"Error deleting product with ID {categoryId}");
            throw new ProductServiceException(
                $"An error occurred while deleting product with ID {categoryId}."); // Throw a specific exception
        }
    }

    public List<Product> SearchProducts(int categoryId = 0, string query = "")
    {
        try
        {
            var outputSearch = new List<Product>();

            // Get products based on category or all products if no category specified
            var products = categoryId <= 0 ? GetAllProducts() : GetProductsByCategory(categoryId);

            if (!products.Any()) return outputSearch;

            // Apply search filter based on query string

            var refinedSearch = string.IsNullOrEmpty(query) ? products : products.Where(prod => prod.Name.ToLower().Contains(query.ToLower()) || prod.Description.ToLower().Contains(query.ToLower()));
            var enumerable = refinedSearch.ToList();
            if (enumerable.Any())
            {
                outputSearch = enumerable.ToList();
            }

            return outputSearch;
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, $"Error searching products with categoryId: {categoryId} and query: {query}");
            throw new ProductServiceException(
                $"An error occurred while searching products with categoryId: {categoryId} and query: {query}."); // Throw a specific exception with more details
        }
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        try
        {
            return await _stockItContext.Products.FirstOrDefaultAsync(p => p.Id == id);
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, $"Error getting product with ID {id}");
            throw new ProductServiceException($"Product with ID {id} not found."); // Throw a custom exception
        }
    }

    public async Task UpdateProductAsync(Product product)
    {
        try
        {

            // Find the existing product in the database
            var existingProduct = await _stockItContext.Products.FindAsync(product.Id);

            if (existingProduct != null)
            {
                // Update properties (excluding ImagePaths for now)
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Quantity = product.Quantity;
                existingProduct.Price = product.Price;
                existingProduct.CategoryId = product.CategoryId;

                existingProduct.ImagePaths = product.ImagePaths; // Update directly

                await _stockItContext.SaveChangesAsync();
            }

        }
        catch (DbException ex)
        {
            _logger.LogError(ex, $"Error getting product with ID {product.Id}");
            throw new ProductServiceException($"Product with ID {product.Id} not found."); // Throw a custom exception
        }
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        try
        {
            return await _stockItContext.Products.ToListAsync();
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, $"Error getting all products async");
            throw new ProductServiceException($"Error getting all products async"); // Throw a custom exception
        }
    }
}
