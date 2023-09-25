using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core.DTOs;
using NLayer.Core.Services;
using NLayer.Service.Services;
using Web.API.Filters;

namespace Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ValidateFilterAttribute]
    public class CategoryController : CustomBaseController
    {
        private readonly ICategoryServices _services;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryServices services, IMapper mapper)
        {
            _services = services;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            var categories = await _services.GetAllAsync();

            var categoriesDto = _mapper.Map<List<CategoryDTO>>(categories.ToList());
            return CreateActionResult(CustomResponseDTO<List<CategoryDTO>>.Success(200, categoriesDto));
        }


        [HttpGet("[action]/{categoryId}")]
        public async Task<IActionResult> GetSingleCategoryByIdWithProductsAsync(int categoryId)
        {
            return CreateActionResult(await _services.GetSingleCategoryByIdWithProductsAsync(categoryId));
        }
    }
}
