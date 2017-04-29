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
    public class ProductTests {
        private Mock<IProductRepository> mock;

        [TestInitialize]
        public void TestInitialize() {
            // Arrange
            mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product { ProductID = 1, Name = "P1", Category = "Cat1" },
                new Product { ProductID = 2, Name = "P2", Category = "Cat2" },
                new Product { ProductID = 3, Name = "P3", Category = "Cat1", ImageData = new byte[] { }, ImageMimeType = "image/png" },
                new Product { ProductID = 4, Name = "P4", Category = "Cat2" },
                new Product { ProductID = 5, Name = "P5", Category = "Cat3" },
            }.AsQueryable());

        }

        [TestMethod]
        public void Can_Paginate() {
            // Arrange
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

        [TestMethod]
        public void Can_Filter_Products() {
            // Arrange
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            // Act
            ProductListViewModel result = (ProductListViewModel)controller.List("Cat2", 1).Model;
            Product[] products = result.Products.ToArray();

            // Assert
            Assert.AreEqual(products.Length, 2);
            Assert.IsTrue(products[0].ProductID == 2 && products[0].Category == "Cat2");
            Assert.IsTrue(products[1].ProductID == 4 && products[1].Category == "Cat2");
        }

        [TestMethod]
        public void Generate_Category_Specific_Product_Count() {
            // Arrange
            ProductController target = new ProductController(mock.Object);

            // Act
            Assert.IsTrue(((ProductListViewModel)target.List("Cat1").Model).PagingInfo.TotalItems == 2);
            Assert.IsTrue(((ProductListViewModel)target.List("Cat2").Model).PagingInfo.TotalItems == 2);
            Assert.IsTrue(((ProductListViewModel)target.List("Cat3").Model).PagingInfo.TotalItems == 1);
            Assert.IsTrue(((ProductListViewModel)target.List(null).Model).PagingInfo.TotalItems == 5);
        }

        [TestMethod]
        public void Can_Retrieve_Image_Data() {
            // Arrange
            ProductController target = new ProductController(mock.Object);

            // Act
            FileContentResult result = target.GetImage(3);

            // Assert - image was returned for a valid product.
            Assert.IsNotNull(result);
            // Assert - image was returned with a correct MIME type.
            Assert.IsTrue(result.ContentType == "image/png");
        }

        [TestMethod]
        public void Cannot_Retrieve_Image_Data_For_Invalid_Id() {
            // Arrange
            ProductController target = new ProductController(mock.Object);

            // Act
            FileContentResult result = target.GetImage(100);

            // Assert
            Assert.IsNull(result);
        }
    }
}
