using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NLayer.Core.DTOs;
using NLayer.Core.Services;
using NLayer.Core;
using NLayer.Core.Models;

namespace Web.MVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductServices _services;
        private readonly ICategoryServices _categoryServices;
        private readonly IMapper _mapper;

        public ProductController(IProductServices services, ICategoryServices categoryServices, IMapper mapper)
        {
            _services = services;
            _categoryServices = categoryServices;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View((await _services.GetProductWithCategory()).Data);
        }
        public async Task<IActionResult> Save()
        {
            var categories = await _categoryServices.GetAllAsync();
            var categoriesDto = _mapper.Map<List<CategoryDTO>>(categories.ToList());
            ViewBag.Categories = new SelectList(categoriesDto, "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Save(ProductDTO productDTO)
        {
            if (ModelState.IsValid)
            {

                await _services.AddAsync(_mapper.Map<Product>(productDTO));


                return RedirectToAction(nameof(Index));
            }
            var categories = await _categoryServices.GetAllAsync();
            var categoriesDto = _mapper.Map<List<CategoryDTO>>(categories.ToList());
            ViewBag.Categories = new SelectList(categoriesDto, "Id", "Name");

            return View();
        }
        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _services.GetByIdAsync(id);
            var categories = await _categoryServices.GetAllAsync();
            var categoriesDto = _mapper.Map<List<CategoryDTO>>(categories.ToList());
            ViewBag.Categories = new SelectList(categoriesDto, "Id", "Name", product.CategoryId);
            return View(_mapper.Map<ProductDTO>(product));
        }
        [HttpPost]
        public async Task<IActionResult> Update(ProductDTO productDTO)
        {
            if (ModelState.IsValid)
            {

                await _services.UpdateAsync(_mapper.Map<Product>(productDTO));


                return RedirectToAction(nameof(Index));
            }
            var categories = await _categoryServices.GetAllAsync();
            var categoriesDto = _mapper.Map<List<CategoryDTO>>(categories.ToList());
            ViewBag.Categories = new SelectList(categoriesDto, "Id", "Name", productDTO.CategoryId);
            return View(productDTO);

        }

        public async Task<IActionResult> Remove(int id)
        {
            var product=await _services.GetByIdAsync(id);
            await _services.RemoveAsync(product);
            return RedirectToAction(nameof(Index));
        }
    }
}
