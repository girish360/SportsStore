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
        public RedirectToRouteResult AddToCart(int productId, string returnUrl) {
            Product product = repository.Products.Where(p => p.ProductID == productId).FirstOrDefault();
            if (product != null) {
                GetCart().AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        // GET: Cart
        public RedirectToRouteResult RemoveFromCart(int productId, string returnUrl) {
            Product product = repository.Products.Where(p => p.ProductID == productId).FirstOrDefault();
            if (product != null) {
                GetCart().RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl = returnUrl });
        }

        public ViewResult Index(string returnUrl) {
            CartIndexViewModel viewModel = new CartIndexViewModel {
                Cart = GetCart(),
                ReturnUrl = returnUrl
            };
            return View(viewModel);
        }

        private Cart GetCart() {
            Cart cart = (Cart)Session["Cart"];
            if (cart == null) {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
    }
}