using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository repository;

        public CartController(IProductRepository repo) {
            repository = repo;
        }

        // GET: Cart
        public RedirectToRouteResult AddToCart(Cart cart, int productId, string returnUrl) {
            Product product = repository.Products.Where(p => p.ProductID == productId).FirstOrDefault();
            if (product != null) {
                cart.AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        // GET: Cart
        public RedirectToRouteResult RemoveFromCart(Cart cart, int productId, string returnUrl) {
            Product product = repository.Products.Where(p => p.ProductID == productId).FirstOrDefault();
            if (product != null) {
                cart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl = returnUrl });
        }

        public PartialViewResult Summary(Cart cart) {
            return PartialView(cart);
        }

        public ViewResult Index(Cart cart, string returnUrl) {
            CartIndexViewModel viewModel = new CartIndexViewModel {
                Cart = cart,
                ReturnUrl = returnUrl
            };
            return View(viewModel);
        }
    }
}