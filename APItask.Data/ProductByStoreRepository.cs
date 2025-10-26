using APItask.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APItask.Data
{
    public class ProductByStoreRepository : IProductByStoreRepository
    {
        private readonly EssentialProductsDbContext _context;

        public ProductByStoreRepository(EssentialProductsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductByStore>> GetProductsInStore()
        {
            return await _context.Set<ProductByStore>().ToListAsync();
        }

        public async Task<ProductByStore?> GetProductById(int productId, int storeId)
        {
            return await _context.Set<ProductByStore>()
                .FirstOrDefaultAsync(p => p.ProductId == productId && p.StoreId == storeId);
        }

        public async Task AddProductToStore(ProductByStore productByStore)
        {
            await _context.Set<ProductByStore>().AddAsync(productByStore);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductInStore(ProductByStore productByStore)
        {
            _context.Set<ProductByStore>().Update(productByStore);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductFromStore(int productId, int storeId)
        {
            var product = await _context.Set<ProductByStore>()
                .FirstOrDefaultAsync(p => p.ProductId == productId && p.StoreId == storeId);

            if (product != null)
            {
                _context.Set<ProductByStore>().Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
