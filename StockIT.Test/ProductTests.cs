using StockIT.Models;

namespace StockIT.Test
{

    [TestFixture]
    public class ProductTests
    {
        [Test]
        public void Product_SetValidValues_StoresValuesCorrectly()
        {
            // Arrange
            var product = new Product();

            // Act
            product.Name = "Laptop";
            product.Description = "High-end gaming laptop.";
            product.Quantity = 10;
            product.Price = 1500.00m;

            // Assert
            Assert.AreEqual("Laptop", product.Name);
            Assert.AreEqual("High-end gaming laptop.", product.Description);
            Assert.AreEqual(10, product.Quantity);
            Assert.AreEqual(1500.00m, product.Price);
        }
    }

}