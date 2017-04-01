using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Entities;
using SportsStore.Domain.Abstract;
using Moq;
using System.Linq;
using SportsStore.WebUI.Controllers;
using System.Collections.Generic;

namespace SportsStore.UnitTests {
    [TestClass]
    public class ProductControllerTests {
        [TestMethod]
        public void Can_Paginate() {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product { ProductID = 1, Name = "P1"},
                new Product { ProductID = 2, Name = "P2"},
                new Product { ProductID = 3, Name = "P3"},
                new Product { ProductID = 4, Name = "P4"},
                new Product { ProductID = 5, Name = "P5"},
            }.AsQueryable());
            ProductController target = new ProductController(mock.Object);

            // Act
            target.PageSize = 3;
            Product[] prodArray = ((IEnumerable<Product>)target.List(2).Model).ToArray<Product>();

            // Assert
            Assert.IsTrue(prodArray.Length == 2);
            Assert.IsTrue(prodArray[0].ProductID == 4);
            Assert.IsTrue(prodArray[1].ProductID == 5);
        }
    }
}
