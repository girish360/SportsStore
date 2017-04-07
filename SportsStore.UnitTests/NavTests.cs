using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Abstract;
using Moq;
using SportsStore.Domain.Entities;
using System.Linq;
using SportsStore.WebUI.Controllers;
using System.Collections.Generic;

namespace SportsStore.UnitTests {
    [TestClass]
    public class NavTests {
        private Mock<IProductRepository> mock;

        [TestInitialize]
        public void InitializeRepo() {
            // Arrange
            mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product { ProductID = 1, Name = "P1", Category = "Cat1" },
                new Product { ProductID = 2, Name = "P2", Category = "Cat3" },
                new Product { ProductID = 3, Name = "P3", Category = "Cat1" },
                new Product { ProductID = 4, Name = "P4", Category = "Cat2" },
                new Product { ProductID = 5, Name = "P5", Category = "Cat2" },
            }.AsQueryable());
        }

        [TestMethod]
        public void Can_Create_Categories() {
            // Act
            NavController target = new NavController(mock.Object);

            // Assert
            string[] result = ((IEnumerable<string>)target.Menu().Model).ToArray<string>();
            Assert.IsTrue(result.Length == 3);
            Assert.IsTrue(result[0] == "Cat1");
            Assert.IsTrue(result[1] == "Cat2");
            Assert.IsTrue(result[2] == "Cat3");
        }
    }
}
