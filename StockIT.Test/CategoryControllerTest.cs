using Moq;
using StockIT.BLL.Services;
using StockIT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockIT.Controllers;

namespace StockIT.Test
{
    [TestFixture]
    public class CategoryControllerTests
    {
        private Mock<ICategoryService> _mockCategoryService;
        private CategoryController _controller;

        [SetUp]
        public void Setup()
        {
            _mockCategoryService = new Mock<ICategoryService>();
            _controller = new CategoryController(null, _mockCategoryService.Object);
        }

        [Test]
        public void DeleteCategory_ValidId_DeletesCategory()
        {
            // Arrange
            _mockCategoryService.Setup(service => service.DeleteCategory(It.IsAny<int>())).Verifiable();

            // Act
            var result = _controller.DeleteCategory(new Category (){Id = 1});

            // Assert
            _mockCategoryService.Verify(service => service.DeleteCategory(It.IsAny<int>()), Times.Once);
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }
    }
}
