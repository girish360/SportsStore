using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.WebUI.Controllers;
using SportsStore.Domain.Abstract;
using Moq;
using SportsStore.Domain.Entities;
using System.Linq;
using System.Web.Mvc;
using SportsStore.WebUI.Models;
using SportsStore.Domain.Concrete;

namespace SportsStore.UnitTests {
    [TestClass]
    public class CartControllerTests {
        private Mock<IProductRepository> mock;

        [TestInitialize]
        public void TestInitialize() {
            // Arrange
            mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product { ProductID = 1, Name = "P1", Category = "Cat1", Price = 11.17m },
                new Product { ProductID = 2, Name = "P2", Category = "Cat2", Price = 4.63m },
                new Product { ProductID = 3, Name = "P3", Category = "Cat1", Price = 9.40m },
                new Product { ProductID = 4, Name = "P4", Category = "Cat2", Price = 9.99m },
                new Product { ProductID = 5, Name = "P5", Category = "Cat3", Price = 75000m },
            }.AsQueryable());
        }

        [TestMethod]
        public void Can_Add_To_Cart() {
            // Arrange
            CartController target = new CartController(mock.Object, null);
            Cart cart = new Cart();

            // Act
            target.AddToCart(cart, 1, null);
            target.AddToCart(cart, 4, null);
            target.AddToCart(cart, 1, null);

            // Assert
            Assert.IsTrue(cart.Lines.Count() == 2);
            Assert.IsTrue(cart.Lines.ToArray()[0].Product.ProductID == 1);
            Assert.IsTrue(cart.Lines.ToArray()[0].Quantity == 2);
            Assert.IsTrue(cart.Lines.ToArray()[1].Product.ProductID == 4);
            Assert.IsTrue(cart.Lines.ToArray()[1].Quantity == 1);
        }

        [TestMethod]
        public void Adding_Product_To_Cart_Goes_To_Cart_Screen() {
            // Arrange
            CartController target = new CartController(mock.Object, null);
            Cart cart = new Cart();

            // Act
            RedirectToRouteResult result = target.AddToCart(cart, 1, "testUrl");

            // Assert
            Assert.IsTrue(result.RouteValues["action"].ToString() == "Index");
            Assert.IsTrue(result.RouteValues["returnUrl"].ToString() == "testUrl");
        }

        [TestMethod]
        public void Can_View_Cart_Contents() {
            // Arrange
            CartController target = new CartController(mock.Object, null);
            Cart cart = new Cart();

            // Act
            CartIndexViewModel viewModel = ((CartIndexViewModel)target.Index(cart, "testUrl").Model);

            // Assert
            Assert.AreSame(viewModel.Cart, cart);
            Assert.IsTrue(viewModel.ReturnUrl == "testUrl");

        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart() {
            // Arrange
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();     // Empty cart.
            ShippingDetails shippingDetails = new ShippingDetails();
            CartController target = new CartController(null, mock.Object);

            // Act
            ViewResult result = target.Checkout(cart, shippingDetails);

            // Assert - check that order hasn't been processed.
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never);
            // Assert - check that we are passing the invalid model back to view.
            Assert.IsTrue(result.ViewData.ModelState.IsValid == false);
            // Assert - check that the default view is returned.
            Assert.IsTrue(String.IsNullOrEmpty(result.ViewName));
        }

        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails() {
            // Arrange
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Product { ProductID = 1, Price = 1m }, 1);
            ShippingDetails shippingDetails = new ShippingDetails();
            CartController target = new CartController(null, mock.Object);
            // Arrange - add an error to the model.
            target.ModelState.AddModelError("error", "error");

            // Act
            ViewResult result = target.Checkout(cart, shippingDetails);

            // Assert - Order hasn't been processed.
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never);
            // Assert - Invalid model has been passed back to the view for rendering.
            Assert.IsTrue(result.ViewData.ModelState.IsValid == false);
            // Assert - check that the default view has been returned.
            Assert.IsTrue(result.ViewName == String.Empty);
        }

        [TestMethod]
        public void Can_Checkout_And_Submit_Order() {
            // Arrange
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Product { ProductID = 1, Price = 1m }, 1);
            ShippingDetails shippingDetails = new ShippingDetails();
            CartController target = new CartController(null, mock.Object);


            // Act
            ViewResult result = target.Checkout(cart, shippingDetails);

            // Assert - Order processed exactly once.
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Once);
            // Assert - "Completed" view has been returned.
            Assert.IsTrue(result.ViewName == "Completed");
            // Assert - Cart is empty.
            Assert.IsTrue(cart.Lines.Count() == 0);
            // Assert - We're passing a valid model to the view.
            Assert.IsTrue(result.ViewData.ModelState.IsValid == true);
        }
    }
}
