using APItask.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APItask.Service
{
    public interface IProductService
    {
        Product GetProduct(int productId);
        List<Product> GetProducts(int noOfProducts = 100);
        Task<Product> GetProductAsync(int productId);
        Task<List<Product>> GetProductsAsync(int noOfProducts = 100);
        Task<List<Product>> CreateProduct(List<Product> products);
        Task<Product> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int productId);
    }
}
