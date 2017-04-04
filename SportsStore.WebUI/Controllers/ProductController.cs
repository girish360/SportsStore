using SportsStore.Domain.Abstract;
using SportsStore.WebUI.Models;
using System.Linq;
using System.Web.Mvc;

namespace SportsStore.WebUI.Controllers {
    public class ProductController : Controller
    {
        public int PageSize = 4;
        private IProductRepository repository;

        public ProductController(IProductRepository productRepository) {
            repository = productRepository;
        }
        
        // GET: Product
        public ViewResult List(string category, int page = 1)
        {
            ProductListViewModel viewModel = new ProductListViewModel {
                Products = repository.Products
                    .Where(p => category == null || p.Category == category)
                    .OrderBy(p => p.ProductID)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = repository.Products.Count()
                },
                CurrentCategory = category
            };
            return View(viewModel);
        }
    }
}