using APItask.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APItask.Data
{
    public class StoreRepository : IStoreRepository
    {
        private readonly EssentialProductsDbContext _context;

        public StoreRepository(EssentialProductsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Store>> GetAllStoresAsync()
        {
            return await _context.Stores.ToListAsync();
        }

        public async Task<Store?> GetStoreByIdAsync(int storeId)
        {
            return await _context.Stores.FindAsync(storeId);
        }

        public async Task<Store> AddStoreAsync(Store store)
        {
            _context.Stores.Add(store);
            await _context.SaveChangesAsync();
            return store;
        }

        public async Task<bool> UpdateStoreAsync(Store store)
        {
            _context.Stores.Update(store);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveStoreAsync(int storeId)
        {
            var store = await GetStoreByIdAsync(storeId);
            if (store == null) return false;

            _context.Stores.Remove(store);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}