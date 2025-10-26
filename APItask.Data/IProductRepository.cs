using APItask.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APItask.Data
{
    public interface IProductRepository
    {
        Task<Product?> GetProductAsync(int productId);
        Task<Product?> UpdateProductAsync(Product product);
        Task<List<Product>> CreateProductsAsync(List<Product> products);
        Task<bool> DeleteProductByIdAsync(int productId);
        Product? GetProduct(int productId);
        List<Product> GetProducts(int noOfProducts);
        Task<List<Product>> GetProductsAsync(int noOfProducts);
    }
}
