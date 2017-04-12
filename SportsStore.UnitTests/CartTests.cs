using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using SportsStore.Domain.Entities;
using System.Linq;

namespace SportsStore.UnitTests {
    [TestClass]
    public class CartTests {
        [TestMethod]
        public void Can_Add_New_Lines() {
            // Arrange
            Cart cart = new Cart();

            // Act
            cart.AddItem(new Product { ProductID = 1, Price = 100m }, 1);
            cart.AddItem(new Product { ProductID = 2, Price = 40m }, 3);

            // Assert
            Assert.IsTrue(((List<CartLine>)cart.Lines).Count == 2);
            Assert.IsTrue(((List<CartLine>)cart.Lines)[0].Product.ProductID == 1);
            Assert.IsTrue(((List<CartLine>)cart.Lines)[0].Product.Price == 100m);
            Assert.IsTrue(((List<CartLine>)cart.Lines)[0].Quantity == 1);
            Assert.IsTrue(((List<CartLine>)cart.Lines)[1].Product.ProductID == 2);
            Assert.IsTrue(((List<CartLine>)cart.Lines)[1].Product.Price == 40m);
            Assert.IsTrue(((List<CartLine>)cart.Lines)[1].Quantity == 3);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines() {
            // Arrange
            Cart cart = new Cart();
            cart.AddItem(new Product { ProductID = 1 }, 1);
            cart.AddItem(new Product { ProductID = 2 }, 5);
            cart.AddItem(new Product { ProductID = 3 }, 3);

            // Act
            cart.AddItem(new Product { ProductID = 1 }, 3);
            cart.AddItem(new Product { ProductID = 3 }, 9);

            // Assert
            Assert.IsTrue(((List<CartLine>)cart.Lines).Count == 3);
            Assert.IsTrue(((List<CartLine>)cart.Lines)[0].Product.ProductID == 1);
            Assert.IsTrue(((List<CartLine>)cart.Lines)[0].Quantity == 4);
            Assert.IsTrue(((List<CartLine>)cart.Lines)[1].Product.ProductID == 2);
            Assert.IsTrue(((List<CartLine>)cart.Lines)[1].Quantity == 5);
            Assert.IsTrue(((List<CartLine>)cart.Lines)[2].Product.ProductID == 3);
            Assert.IsTrue(((List<CartLine>)cart.Lines)[2].Quantity == 12);
        }

        [TestMethod]
        public void Can_Remove_Line() {
            // Arrange
            Cart cart = new Cart();
            cart.AddItem(new Product { ProductID = 1 }, 1);
            cart.AddItem(new Product { ProductID = 2 }, 3);
            cart.AddItem(new Product { ProductID = 3 }, 3);
            cart.AddItem(new Product { ProductID = 4 }, 3);
            Product p2 = new Product { ProductID = 2 };
            Product p4 = new Product { ProductID = 4 };

            // Act
            cart.RemoveLine(p2);
            cart.RemoveLine(p4);

            // Assert
            Assert.IsTrue(((List<CartLine>)cart.Lines).Count == 2);
            Assert.IsTrue(((List<CartLine>)cart.Lines)[0].Product.ProductID == 1);
            Assert.IsTrue(((List<CartLine>)cart.Lines)[1].Product.ProductID == 3);
            Assert.IsTrue(((List<CartLine>)cart.Lines).Where(l => l.Product.ProductID == p2.ProductID).Count() == 0);
            Assert.IsTrue(((List<CartLine>)cart.Lines).Where(l => l.Product.ProductID == p4.ProductID).Count() == 0);
        }

        [TestMethod]
        public void Calculate_Cart_Total() {
            // Arrange
            Cart cart = new Cart();
            cart.AddItem(new Product { ProductID = 1, Price = 100m }, 3);
            cart.AddItem(new Product { ProductID = 2, Price = 40m }, 4);
            cart.AddItem(new Product { ProductID = 3, Price = 60m }, 5);
            cart.AddItem(new Product { ProductID = 4, Price = 79.99m }, 6);

            // Act
            decimal totalValue = cart.ComputeTotalValue();

            //
            Assert.IsTrue(totalValue == 1239.94m);
        }

        [TestMethod]
        public void Can_Clear_Contents() {
            // Arrange
            Cart cart = new Cart();
            cart.AddItem(new Product { ProductID = 1 }, 1);
            cart.AddItem(new Product { ProductID = 2 }, 3);
            cart.AddItem(new Product { ProductID = 3 }, 3);
            cart.AddItem(new Product { ProductID = 4 }, 3);

            // Act
            cart.Clear();

            //
            Assert.IsTrue(cart.Lines.Count() == 0);
        }
    }
}
