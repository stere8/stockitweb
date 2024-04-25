using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StockIT.BLL.Services;
using StockIT.Models;

namespace StockIT.Test
{
    [TestFixture]
    public class CategoryServiceTests
    {
        private Mock<StockITContext> _mockContext;
        private CategoryService _categoryService;

        [SetUp]
        public void Setup()
        {
            _mockContext = new Mock<StockITContext>();
            _categoryService = new CategoryService(_mockContext.Object, new Mock<ILogger<CategoryService>>().Object);
        }

        [Test]
        public void GetAllCategories_ReturnsAllCategories()
        {
            // Arrange
            var data = new List<Category>
            {
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Clothing" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Category>>();
            mockSet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _mockContext.Setup(c => c.Categories).Returns(mockSet.Object);

            // Act
            var categories = _categoryService.GetAllCategories();

            // Assert
            Assert.AreEqual(2, categories.Count);
            Assert.AreEqual("Electronics", categories[0].Name);
            Assert.AreEqual("Clothing", categories[1].Name);
        }
    }
}
