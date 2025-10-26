using APItask.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APItask.Service
{
    public interface IProductByStoreService
    {
        Task<IEnumerable<ProductByStore>> GetProductsInStore();
        Task<ProductByStore?> GetProductById(int productId, int storeId);
        Task AddProductToStore(ProductByStore productByStore);
        Task UpdateProductInStore(ProductByStore productByStore);
        Task DeleteProductFromStore(int productId, int storeId);
    }
}
