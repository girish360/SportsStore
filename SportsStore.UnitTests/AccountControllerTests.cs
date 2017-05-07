using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Models;
using System.Web.Mvc;

namespace SportsStore.UnitTests {
    [TestClass]
    public class AccountControllerTests {
        [TestMethod]
        public void Can_Login_With_Valid_Credentials() {
            // Arrange
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin1", "admin")).Returns(true);
            AccountController target = new AccountController(mock.Object);
            LogOnViewModel viewModel = new LogOnViewModel { Username = "admin", Password = "admin" };

            // Act
            ActionResult result = target.LogOn(viewModel, "someUrl");

            // Assert - appropriate type is returned.
            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            // Assert - redirects to initial Url.
            Assert.IsTrue(((RedirectResult)result).Url == "someUrl");
        }

        [TestMethod]
        public void Cannot_Login_With_Invalid_Credentials() {
            // Arrange
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("badUsername", "badPassword")).Returns(false);
            AccountController target = new AccountController(mock.Object);
            LogOnViewModel viewModel = new LogOnViewModel { Username = "badUsername", Password = "badPassword" };

            // Act
            ActionResult result = target.LogOn(viewModel, "someUrl");

            // Assert - ViewResult.
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            // Assert - LogOn view returned from LogOn controller.
            Assert.IsTrue(((ViewResult)result).ViewName == "");
            // Assert - Model state is invalid.
            Assert.IsTrue(((ViewResult)result).ViewData.ModelState.IsValid == false);
        }
    }
}
