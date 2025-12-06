using APItask.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APItask.Data
{
    public interface IProductRepository
    {
<<<<<<< HEAD
        Task<Product?> GetProductAsync(int productId);
        Task<Product?> UpdateProductAsync(Product product);
=======
        Task<Product> GetProductAsync(int productId);
        Task<Product> GetUpcAsync(string upc);
        Task<Product> UpdateProductAsync(Product product);
>>>>>>> 6c79d9140c502456a00bc0950ae536f0f7d2003f
        Task<List<Product>> CreateProductsAsync(List<Product> products);
        Task<bool> DeleteProductByIdAsync(int productId);
        Product? GetProduct(int productId);
        List<Product> GetProducts(int noOfProducts);
        Task<List<Product>> GetProductsAsync(int noOfProducts);
    }
}
