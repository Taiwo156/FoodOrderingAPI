using APItask.Core.Models;
using APItask.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APItask.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<Product>> CreateProduct(List<Product> products)
        {
            return await _productRepository.CreateProductsAsync(products);
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            return await _productRepository.DeleteProductByIdAsync(productId);
        }

        public Product GetProduct(int productId)
        {
            return _productRepository.GetProduct(productId);
        }

        public Task<Product> GetProductAsync(int productId)
        {
            return _productRepository.GetProductAsync(productId);
        }

        public List<Product> GetProducts(int noOfProducts = 100)
        {
            return _productRepository.GetProducts(noOfProducts);
        }

        public Task<List<Product>> GetProductsAsync(int noOfProducts = 100)
        {
            return _productRepository.GetProductsAsync(noOfProducts);
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            return await _productRepository.UpdateProductAsync(product);
        }
    }
}
