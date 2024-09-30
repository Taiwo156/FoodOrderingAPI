using APItask.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APItask.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;

        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<List<Product>> CreateProduct(List<Product> product)
        {
            return await productRepository.CreateProductsAsync(product);
        }

        public async Task<bool> DeleteProductAsync(int productid)
        {
            return await productRepository.DeleteProductByIdAsync(productid);
        }

        public Product GetProduct(int productid)
        {
            return productRepository.GetProduct(productid);
        }

        public Task<Product> GetProductAsync(int productId)
        {
            return productRepository.GetProductAsync(productId);
        }
        public Task<Product> GetUpcAsync(string upc)
        {
            return productRepository.GetUpcAsync(upc);
        }

        public List<Product> GetProducts(int noOfProducts = 100)
        {
            return productRepository.GetProducts(noOfProducts);
        }

        public Task<List<Product>> GetProductsAsync(int noOfProducts = 100)
        {
            return productRepository.GetProductsAsync(noOfProducts);
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            return await productRepository.UpdateProductAsync(product);
        }
    }
}
