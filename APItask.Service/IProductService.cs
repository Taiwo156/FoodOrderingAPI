using System.Collections.Generic;
using System.Threading.Tasks;

namespace APItask.Service
{
    public interface IProductService
    {
        Product GetProduct(int productid);
        List<Product> GetProducts(int noOfProducts = 100);
        Task<Product> GetProductAsync(int productId);
        Task<Product> GetUpcAsync(string upc);
        Task<List<Product>> GetProductsAsync(int noOfProducts = 100);
        Task<List<Product>> CreateProduct(List<Product> product);
        Task<Product> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int productid);

    }
}
