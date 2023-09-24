using AutoMapper;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Service.Services
{
    public class ProductService : Service<Product>, IProductServices
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Product> _repository;

        public ProductService(
            IGenericRepository<Product> repository,
            IUnitOfWork unitOfWork,
            IProductRepository productRepository,
            IMapper mapper)
            : base(repository, unitOfWork)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = repository;
        }





        public async Task<CustomResponseDTO<List<ProductWithCategoryDTO>>> GetProductWithCategory()
        {
            var products= await _productRepository.GetProductWithCategory();
            var productsDto=_mapper.Map<List<ProductWithCategoryDTO>>(products);
            return CustomResponseDTO<List<ProductWithCategoryDTO>>.Success(200, productsDto);
        }

        
    }
}
