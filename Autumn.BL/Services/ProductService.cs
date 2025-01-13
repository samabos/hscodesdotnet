using Autumn.Domain.Models;
using Autumn.Repository.Interface;
using Autumn.Service.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autumn.Service
{
    public class ProductService : BaseService<Product>, IProductService
    {
        protected readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository) : base(productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<Product>> GetByTagsAsync(string tag) =>
            await _productRepository.GetByTagsAsync(tag);

        public async Task<List<Product>> GetByKeywordAsync(string keyword) =>
            await _productRepository.GetByKeywordAsync(keyword);

        public async Task<List<Product>> GetLikeKeywordAsync(string keyword) =>
            await _productRepository.GetLikeKeywordAsync(keyword);

    }
}
