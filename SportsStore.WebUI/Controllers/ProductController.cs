using SportsStore.Domain.Abstract;
using System.Web.Mvc;

namespace SportsStore.WebUI.Controllers {
    public class ProductController : Controller
    {
        private IProductRepository repository;

        public ProductController(IProductRepository productRepository) {
            repository = productRepository;
        }
        
        // GET: Product
        public ViewResult List()
        {
            System.Linq.IQueryable<Domain.Entities.Product> foo = repository.Products;
            return View(foo);
        }
    }
}