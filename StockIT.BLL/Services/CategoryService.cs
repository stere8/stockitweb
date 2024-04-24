    using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StockIT.BLL.Exceptions;
using StockIT.Models;

namespace StockIT.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly StockITContext _stockItContext;
        private readonly ILogger<CategoryService> _logger; // Inject logger

        public CategoryService(StockITContext context, ILogger<CategoryService> logger)
        {
            _stockItContext = context;
            _logger = logger;
        }

        public List<Category> GetAllCategories()
        {
            try
            {
                return _stockItContext.Categories.ToList();
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Error retrieving categories");
                throw new CategoryServiceException("An error occurred while retrieving categories.");
            }
        }

        public Category GetCategoryById(int id)
        {
            try
            {
                return _stockItContext.Categories.FirstOrDefault(c => c.Id == id);
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, $"Error getting category with ID {id}");
                throw new CategoryServiceException($"Category with ID {id} not found.");
            }
        }

        public void AddCategory(Category category)
        {
            try
            {
                _stockItContext.Categories.Add(category);
                _stockItContext.SaveChanges();
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, $"Error adding category: {category.Name}"); // Log category FirstName
                throw new CategoryServiceException("An error occurred while adding a category.");
            }
        }

        public void UpdateCategory(Category category)
        {
            try
            {
                _stockItContext.Categories.Attach(category);
                _stockItContext.Entry(category).State = EntityState.Modified;
                _stockItContext.SaveChanges();
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, $"Error updating category with ID {category.Id}");
                throw new CategoryServiceException($"An error occurred while updating category with ID {category.Id}."); // Use category.Id here
            }
        }

        public void DeleteCategory(int id)
        {
            try
            {
                var category = _stockItContext.Categories.Find(id);
                if (category != null)
                {
                    // Consider handling potential related products before deleting
                    _stockItContext.Categories.Remove(category);
                    _stockItContext.SaveChanges();
                }
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, $"Error deleting category with ID {id}");
                throw new CategoryServiceException($"An error occurred while deleting category with ID {id}.");
            }
        }
    }
}
