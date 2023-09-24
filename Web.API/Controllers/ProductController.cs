using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;
using NLayer.Service.Services;
using Web.API.Filters;

namespace Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ValidateFilterAttribute]
    public class ProductController : CustomBaseController
    {
        private readonly IMapper _mapper;
        
        private readonly IProductServices _service;

        public ProductController(IMapper mapper,  IProductServices productServices)
        {
            _mapper = mapper;
           
            _service = productServices;
        }
        [HttpGet]
        public async Task<IActionResult> All()
        {
            var products = await _service.GetAllAsync();
            var productsDtos = _mapper.Map<List<ProductDTO>>(products.ToList());
            return CreateActionResult(CustomResponseDTO<List<ProductDTO>>.Success(200, productsDtos));
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetProductWithCategory()
        {
            return CreateActionResult(await _service.GetProductWithCategory());
        }

        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);
            
            var productsDto = _mapper.Map<ProductDTO>(product);
            return CreateActionResult(CustomResponseDTO<ProductDTO>.Success(200, productsDto));
        }
        [HttpPost()]
        public async Task<IActionResult> Save(ProductDTO productDto)
        {
            var product = await _service.AddAsync(_mapper.Map<Product>(productDto));
            var productsDto = _mapper.Map<ProductDTO>(product);
            return CreateActionResult(CustomResponseDTO<ProductDTO>.Success(201, productsDto));
        }
        [HttpPut()]
        public async Task<IActionResult> Update(ProductUpdateDTO productDto)
        {
             await _service.UpdateAsync(_mapper.Map<Product>(productDto));
           
            return CreateActionResult(CustomResponseDTO<NoContentDTO>.Success(204));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
           var product= await _service.GetByIdAsync(id);
            await _service.RemoveAsync(product);

            return CreateActionResult(CustomResponseDTO<NoContentDTO>.Success(204));
        }

    }
}
