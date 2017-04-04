using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Entities;
using SportsStore.Domain.Abstract;
using Moq;
using System.Linq;
using SportsStore.WebUI.Controllers;
using System.Collections.Generic;
using SportsStore.WebUI.HtmlHelpers;
using System.Web.Mvc;
using SportsStore.WebUI.Models;

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
            target.PageSize = 3;

            // Act
            ProductListViewModel viewModel = (ProductListViewModel)target.List(null, 2).Model;
            Product[] prodArray = ((IEnumerable<Product>)viewModel.Products).ToArray<Product>();

            // Assert
            Assert.IsTrue(prodArray.Length == 2);
            Assert.IsTrue(prodArray[0].ProductID == 4);
            Assert.IsTrue(prodArray[1].ProductID == 5);
        }

        [TestMethod]
        public void Can_Generate_Paging_Links() {
            // Arrange
            HtmlHelper myHelper = null;
            PagingInfo pagingInfo = new PagingInfo {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            // Act
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, (i => "Page" + i));

            // Assert
            Assert.AreEqual(result.ToString(), @"<a href=""Page1"">1</a><a class=""selected"" href=""Page2"">2</a><a href=""Page3"">3</a>");
        }

        [TestMethod]
        public void Can_Send_Pagination_Data() {
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
            target.PageSize = 3;

            // Act
            ProductListViewModel model = (ProductListViewModel)target.List(null, 2).Model;

            // Assert
            Assert.IsTrue(model.PagingInfo.CurrentPage == 2);
            Assert.IsTrue(model.PagingInfo.ItemsPerPage == 3);
            Assert.IsTrue(model.PagingInfo.TotalItems == 5);
            Assert.IsTrue(model.PagingInfo.TotalPages == 2);
        }
    }
}
