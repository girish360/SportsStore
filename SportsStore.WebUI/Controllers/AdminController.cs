using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private IProductRepository repository;

        public AdminController(IProductRepository repo) {
            repository = repo;
        }

        public ViewResult Index() {
            return View(repository.Products);
        }

        public ViewResult Edit(int productId) {
            Product product = repository.Products.Where(p => p.ProductID == productId).FirstOrDefault();
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product, HttpPostedFileBase image) {
            if (ModelState.IsValid) {
                if (image != null) {
                    product.ImageMimeType = image.ContentType;
                    product.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(product.ImageData, 0, image.ContentLength);
                }
                repository.SaveProduct(product);
                TempData["message"] = String.Format("{0} has been saved.", product.Name);
                return RedirectToAction("Index");
            } else {
                return View("Edit", product);
            }
        }

        public ViewResult Create() {
            return View("Edit", new Product());
        }

        public RedirectToRouteResult Delete(int productId) {
            Product product = repository.Products.Where(p => p.ProductID == productId).FirstOrDefault();
            if (product != null) {
                TempData["message"] = String.Format("{0} has been deleted.", product.Name);
                repository.DeleteProduct(product);
            }
            return RedirectToAction("Index");
        }
    }
}
