﻿using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NLayer.Core.DTOs;
using NLayer.Core.Services;
using NLayer.Core;
using NLayer.Core.Models;
using Web.MVC.Services;

namespace Web.MVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductApiService _productApiService;
        private readonly CategoryApiService _categoryApiService;

        public ProductController(CategoryApiService categoryApiService, ProductApiService productApiService)
        {
            _categoryApiService = categoryApiService;
            _productApiService = productApiService;
        }

        public async Task<IActionResult> Index()
        {

            return View(await _productApiService.GetProductsWithCategoryAsync());
        }

        public async Task<IActionResult> Save()
        {
            var categoriesDto = await _categoryApiService.GetAllAsync();



            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductDTO productDto)

        {


            if (ModelState.IsValid)
            {

                await _productApiService.SaveAsync(productDto);


                return RedirectToAction(nameof(Index));
            }

            var categoriesDto = await _categoryApiService.GetAllAsync();



            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name");
            return View();
        }


        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productApiService.GetByIdAsync(id);


            var categoriesDto = await _categoryApiService.GetAllAsync();



            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name", product.CategoryId);

            return View(product);

        }
        [HttpPost]
        public async Task<IActionResult> Update(ProductDTO productDto)
        {
            if (ModelState.IsValid)
            {

                await _productApiService.UpdateAsync(productDto);

                return RedirectToAction(nameof(Index));

            }

            var categoriesDto = await _categoryApiService.GetAllAsync();



            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name", productDto.CategoryId);

            return View(productDto);

        }


        public async Task<IActionResult> Remove(int id)
        {
            await _productApiService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
