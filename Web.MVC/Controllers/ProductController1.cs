using Microsoft.AspNetCore.Mvc;
using NLayer.Core.Services;

namespace Web.MVC.Controllers
{
    public class ProductController1 : Controller
    {
        private readonly IProductServices _services;

        public ProductController1(IProductServices services)
        {
            _services = services;
        }

        public async Task< IActionResult> Index()
        {
            return View(await _services.GetProductWithCategory());
        }
    }
}
