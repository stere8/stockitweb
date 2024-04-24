using StockIT.Models;

namespace StockIT.BLL.Services;

public interface ICategoryService
{
    List<Category> GetAllCategories();
    Category GetCategoryById(int id);
    void AddCategory(Category category);
    void UpdateCategory(Category category);
    void DeleteCategory(int id);
    // Add methods for searching, filtering, etc. (optional)   
}