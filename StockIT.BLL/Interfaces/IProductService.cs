using StockIT.Models;

namespace StockIT.BLL.Services;

public interface IProductService
{
    List<Product> GetAllProducts();
    Product GetProductById(int id);
    void AddProduct(Product product);
    void UpdateProduct(Product product);
    void DeleteProduct(int id);
    // Add methods for searching, filtering, etc. (optional)
    List<Product> GetProductsByCategory(int categoryId);
    List<Product> SearchProducts(int categoryId=0,string query = "");
    Task<Product> GetProductByIdAsync(int productId);
    Task UpdateProductAsync(Product product);
    Task<List<Product>> GetAllProductsAsync();
}