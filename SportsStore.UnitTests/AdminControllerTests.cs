using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System.Web.Mvc;
using System.Linq;

namespace SportsStore.UnitTests {
    [TestClass]
    public class AdminControllerTests {
        private Mock<IProductRepository> mock;
        [TestInitialize]
        public void TestInitialize() {
            // Arrange
            mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product { ProductID = 1, Name = "P1", Category = "Cat1" },
                new Product { ProductID = 2, Name = "P2", Category = "Cat2" },
                new Product { ProductID = 3, Name = "P3", Category = "Cat1" },
                new Product { ProductID = 4, Name = "P4", Category = "Cat2" },
                new Product { ProductID = 5, Name = "P5", Category = "Cat3" },
            }.AsQueryable());
        }

        [TestMethod]
        public void Index_Contains_All_Products() {
            // Arrange
            AdminController target = new AdminController(mock.Object);
            
            // Act
            ViewResult result = target.Index();

            // Assert
            Assert.IsTrue(((IQueryable<Product>)result.Model).ToArray<Product>()[0].ProductID == 1);
            Assert.IsTrue(((IQueryable<Product>)result.Model).ToArray<Product>()[1].ProductID == 2);
            Assert.IsTrue(((IQueryable<Product>)result.Model).ToArray<Product>()[2].ProductID == 3);
            Assert.IsTrue(((IQueryable<Product>)result.Model).ToArray<Product>()[3].ProductID == 4);
            Assert.IsTrue(((IQueryable<Product>)result.Model).ToArray<Product>()[4].ProductID == 5);
        }

        [TestMethod]
        public void Can_Edit_Product() {
            // Arrange
            AdminController target = new AdminController(mock.Object);

            // Act
            Product p1 = target.Edit(1).Model as Product;
            Product p2 = target.Edit(2).Model as Product;
            Product p3 = target.Edit(3).Model as Product;
            Product p4 = target.Edit(4).Model as Product;
            Product p5 = target.Edit(5).Model as Product;

            // Assert
            Assert.IsTrue(1 == p1.ProductID);
            Assert.IsTrue(2 == p2.ProductID);
            Assert.IsTrue(3 == p3.ProductID);
            Assert.IsTrue(4 == p4.ProductID);
            Assert.IsTrue(5 == p5.ProductID);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Product() {
            // Arrange
            AdminController target = new AdminController(mock.Object);

            // Act
            Product result = target.Edit(6).Model as Product;

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Can_Save_Valid_Products() {
            // Arrange
            AdminController target = new AdminController(mock.Object);
            Product p1 = mock.Object.Products.Where(p => p.ProductID == 1).FirstOrDefault();

            // Act
            ActionResult result = target.Edit(p1, null);

            // Assert - repository method was called.
            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Once);
            // Assert - Index view was returned.
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.IsTrue(((RedirectToRouteResult)result).RouteValues["action"].ToString() == "Index");
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes() {
            // Arrange
            AdminController target = new AdminController(mock.Object);
            target.ModelState.AddModelError("", "someError");
            Product p1 = mock.Object.Products.Where(p => p.ProductID == 1).FirstOrDefault();

            // Act
            ActionResult result = target.Edit(p1, null);

            // Assert - Product has not been saved.
            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never);
            // Assert - Edit View returned.
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsTrue(((ViewResult)result).ViewName == "Edit");
            Assert.AreSame(p1, ((ViewResult)result).Model);
        }

        [TestMethod]
        public void Can_Delete_Valid_Product() {
            // Arrange
            AdminController target = new AdminController(mock.Object);
            Product p1 = mock.Object.Products.Where(p => p.ProductID == 1).FirstOrDefault();

            // Act
            RedirectToRouteResult result = target.Delete(p1.ProductID);

            // Assert - product is deleted.
            mock.Verify(m => m.DeleteProduct(p1), Times.Once);
            // Assert - redirected to appropriate action.
            Assert.IsTrue(result.RouteValues["action"].ToString() == "Index");

        }

        [TestMethod]
        public void Cannot_Delete_Invalid_Product() {
            // Arrange
            AdminController target = new AdminController(mock.Object);

            // Act
            RedirectToRouteResult result = target.Delete(6);

            // Assert - product not deleted.
            mock.Verify(m => m.DeleteProduct(It.IsAny<Product>()), Times.Never);
            // Assert - redirected to appropriate action.
            Assert.IsTrue(result.RouteValues["action"].ToString() == "Index");
        }
    }
}
