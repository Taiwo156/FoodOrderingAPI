using APItask.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using APItask.Core.Models;

namespace APItask.Service
{
    public class ProductByStoreService : IProductByStoreService
    {
        private readonly IProductByStoreRepository _repository;

        public ProductByStoreService(IProductByStoreRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductByStore>> GetProductsInStore()
        {
            return await _repository.GetProductsInStore();
        }

        public async Task<ProductByStore?> GetProductById(int productId, int storeId)
        {
            return await _repository.GetProductById(productId, storeId);
        }

        public async Task AddProductToStore(ProductByStore productByStore)
        {
            await _repository.AddProductToStore(productByStore);
        }

        public async Task UpdateProductInStore(ProductByStore productByStore)
        {
            await _repository.UpdateProductInStore(productByStore);
        }

        public async Task DeleteProductFromStore(int productId, int storeId)
        {
            await _repository.DeleteProductFromStore(productId, storeId);
        }
    }
}
