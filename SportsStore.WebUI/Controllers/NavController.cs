using SportsStore.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.WebUI.Controllers
{
    public class NavController : Controller
    {
        private IProductRepository repository;

        public NavController(IProductRepository repo) {
            repository = repo;
        }

        public ViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;
            return View(repository.Products.Select(s => s.Category).Distinct().OrderBy(x => x));
        }
    }
}